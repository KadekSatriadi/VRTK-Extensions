using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class VRTK_GrabMenu : MonoBehaviour
{
    public VRTK_InteractableObject grabObject;
    public VRTK_ControllerEventRegistrator.Button triggerButton;
    public List<VRTK_GrabMenuButton> buttons;

    [Range(0.15f, 0.5f)]
    public float radius = 0.15f;
    [Range(30f, 360f)]
    public float angle = 30f;
    public Vector3 buttonOffset;
    public AnimationCurve animationCurve;

    private bool isGrabbed = false;
    private bool isSelecting = false;
    private VRTK_ControllerEvents grabberEvents;
    private float animationDuration = 0.15f;
    private float scaleFactor = 2.5f;
    private float minDistanceToTrigger = 0.1f;
    private Vector3 initButtonScale;
    

    // Start is called before the first frame update
    void Start()
    {
        initButtonScale = buttons[0].transform.lossyScale;
        grabObject.InteractableObjectGrabbed += GrabObject_InteractableObjectGrabbed;
        grabObject.InteractableObjectUngrabbed += GrabObject_InteractableObjectUngrabbed;
        HideButtons();
    }

    private void GrabObject_InteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isSelecting) HideButtons();
        isGrabbed = false;
        grabberEvents = null;

    }

    private void GrabObject_InteractableObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = true;
        grabberEvents = e.interactingObject.GetComponent<VRTK_ControllerEvents>();
        VRTK_ControllerEventRegistrator.Register(grabberEvents, triggerButton, VRTK_ControllerEventRegistrator.ButtonAction.Pressed, GrabberEvents_ButtonPressed);
        VRTK_ControllerEventRegistrator.Register(grabberEvents, triggerButton, VRTK_ControllerEventRegistrator.ButtonAction.Released, GrabberEvents_ButtonReleased);
    }

    private void HideButtons()
    {
        foreach(VRTK_GrabMenuButton b in buttons)
        {
            Vector3 position = b.transform.position;
            StartCoroutine(MovementTween(b.transform, position,  grabObject.transform.position, delegate {
                b.transform.SetParent(transform);
                b.gameObject.SetActive(false);
            }));
        }

        isSelecting = false;
    }

    private void ShowButtons()
    {
        Vector3 leftVector = Quaternion.AngleAxis(angle * 0.5f, grabObject.transform.up) * grabberEvents.transform.forward * radius;
        float angleSegment = angle / buttons.Count;

        for(int i = 0; i < buttons.Count; i++)
        {
            VRTK_GrabMenuButton b = buttons[i];
            b.transform.SetParent(null);
            b.gameObject.SetActive(true);
            Vector3 translateVector = Quaternion.AngleAxis(-i * angleSegment, grabObject.transform.up) * leftVector;
            Vector3 position = grabObject.transform.position + translateVector + buttonOffset;
            StartCoroutine(MovementTween(b.transform, grabObject.transform.position, position, delegate {
                isSelecting = true;
            }));
        }

    }

    private void GrabberEvents_ButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (isGrabbed) ShowButtons();
    }

    private void GrabberEvents_ButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
       if(isGrabbed) HideButtons();
    }


    public IEnumerator MovementTween(Transform t, Vector3 s, Vector3 e, Action a)
    {
        float journey = 0f;
        while (journey <= animationDuration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / animationDuration);
            float curvePercent = animationCurve.Evaluate(percent);
            Vector3 current = Vector3.LerpUnclamped(s, e, curvePercent);
            t.position = current;
            yield return null;
        }
        t.position = e;
        a.Invoke();
    }

    private void HideOthers(VRTK_GrabMenuButton b)
    {
        foreach (VRTK_GrabMenuButton bh in buttons)
        {
            if (!b.Equals(bh)) bh.gameObject.SetActive(false);
        }
        StartCoroutine(MovementTween(b.transform, b.transform.position, b.transform.position + b.transform.up * 0.15f, delegate {
            StartCoroutine(MovementTween(b.transform, b.transform.position, grabObject.transform.position, delegate {
                b.OnTrigger.Invoke();
                b.gameObject.SetActive(false);
            }));
        }));
        isSelecting = false;
    }

    private float GetScale(float d)
    {
        float f =  Mathf.Log(radius / d) * scaleFactor;
        return (f > 1f) ? f : 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelecting)
        {
            for(int i = 0; i < buttons.Count; i++)
            {
                VRTK_GrabMenuButton b = buttons[i];
                float d = Vector3.Distance(grabObject.transform.position, b.transform.position);
                b.transform.localScale = initButtonScale * GetScale(d);

                if(d <= minDistanceToTrigger)
                {
                    HideOthers(b);
                    return;
                }
            }
        }
    }
}
