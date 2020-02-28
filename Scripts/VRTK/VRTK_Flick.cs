using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class VRTK_Flick : InteractionModule
{
    public VRTK_InteractGrab interactGrab;
    protected bool isGrabbing = false;
    public UnityEvent OnThrownUnity;
    public float delay = 1f;
    public delegate void OnThrownHandler(GameObject sender);
    public event OnThrownHandler Thrown;

    private float speed = 0.001f;
    protected Rigidbody grabbedObject;
    protected Vector3 lastPosition = Vector3.negativeInfinity;
    protected DateTime lastTime = DateTime.MaxValue;
    protected Queue<Vector3> positionBuffer = new Queue<Vector3>();
    protected bool isFloating = false;
   private void Start()
    {
        interactGrab.ControllerGrabInteractableObject += InteractGrab_ControllerGrabInteractableObject;
        interactGrab.controllerEvents.GripUnclicked += ControllerEvents_GripUnclicked; ;
    }

    private void Update()
    {
        if (grabbedObject != null)
        {
            if (positionBuffer.Count == 1000) positionBuffer.Dequeue();
            Vector3 angularVelocity = VRTK_DeviceFinder.GetControllerAngularVelocity(VRTK_ControllerReference.GetControllerReference(interactGrab.gameObject));
            positionBuffer.Enqueue(angularVelocity);

            if (isFloating)
            {
                grabbedObject.isKinematic = false;
                grabbedObject.AddForce(interactGrab.gameObject.transform.up * 0.5f);
            }
        }
    }

    private void ControllerEvents_GripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        if (!enable) return;

        Debug.Log(positionBuffer.Count);
        if (positionBuffer.Count > 0)
        {
            Vector3 last = positionBuffer.Dequeue();
            float sumAngular = 0;
            foreach (Vector3 val in positionBuffer.ToArray())
            {
                Quaternion valRot = Quaternion.Euler(val);
                Quaternion lastRot = Quaternion.Euler(last);
                sumAngular = Quaternion.Angle(valRot, lastRot);
                last = val;
            }

            float s = sumAngular / 1000;

            Debug.Log(speed);

            if (s > speed)
            {
                Debug.Log("Gone!!");
                interactGrab.ForceRelease();
                grabbedObject.isKinematic = false;
                grabbedObject.useGravity = true;
                grabbedObject.WakeUp();
                StartCoroutine(Do());
            }

            lastPosition = Vector3.negativeInfinity;
        }     
    }

    IEnumerator Do()
    {
        isFloating = true;
        yield return new WaitForSecondsRealtime(delay);
        Thrown(grabbedObject.gameObject);
        OnThrownUnity.Invoke();
        isFloating = false;
        grabbedObject.isKinematic = true;
        grabbedObject.useGravity = false;
    }


    public virtual void OnThrown(OnThrownHandler g)
    {
        Thrown(grabbedObject.gameObject);
    }

    private void InteractGrab_ControllerGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        grabbedObject = e.target.GetComponent<Rigidbody>();
        lastPosition = e.target.transform.position;
        lastTime = DateTime.Now;
        positionBuffer.Clear();
    }

   
}
