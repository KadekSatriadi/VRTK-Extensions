using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTK_OnGrabObjectDistanceAdjustment : MonoBehaviour
{
    public VRTK_InteractGrab interactGrab;
    public VRTK_ControllerEvents controllerEvents;
    public VRTK_Pointer pointer;
    public float speed = 0.001f;
    public float rotationSpeed = 0.5f;

    public List<InteractionModule> overrideList;

    protected enum Status
    {
        Null, MoveToward, MoveAway, RotateLeft, RotateRight
    }

    protected bool isGrab = false;
    protected Status interactionStatus = Status.Null;
    protected Transform obj;

    protected bool padTouch = false;
    protected bool triggerPressed = false;
    private void Awake()
    {
        interactGrab.ControllerGrabInteractableObject += Grab;
        interactGrab.ControllerUngrabInteractableObject += Ungrab;
        interactGrab.GrabButtonReleased += InteractGrab_GrabButtonReleased;
        controllerEvents.TouchpadPressed += ControllerEvents_TouchpadPressed;
        controllerEvents.TouchpadReleased += ControllerEvents_TouchpadReleased;
        controllerEvents.TouchpadTouchStart += ControllerEvents_TouchpadTouchStart;
        controllerEvents.TouchpadTouchEnd += ControllerEvents_TouchpadTouchEnd;
        controllerEvents.TriggerPressed += ControllerEvents_TriggerPressed;
        controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;
    }

    private void ControllerEvents_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        triggerPressed = false;
    }

    private void ControllerEvents_TriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        triggerPressed = true;
    }

    private void ControllerEvents_TouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        padTouch = false;
    }

    private void ControllerEvents_TouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        padTouch = true;
    }

    private void InteractGrab_GrabButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
        isGrab = false;
    }

    private void ControllerEvents_TouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (isGrab)
        {
            interactionStatus = Status.Null;
            interactGrab.AttemptGrab();
            foreach (InteractionModule m in overrideList)
            {
                m.Enable();
            }
        }
    }

    private void Ungrab(object sender, ObjectInteractEventArgs e)
    {

    }

    private void Update()
    {
        if (!isGrab) return;

        Transform cursor = pointer.pointerRenderer.GetDestinationHit().transform;

        if (cursor == null) return;

        Vector3 controllerPosition = pointer.pointerRenderer.GetPointerObjects()[0].transform.position; //VRTK_DeviceFinder.GetActualController(controllerEvents.gameObject).transform.position;  //(VRTK_ControllerReference.GetControllerReference(controllerEvents.gameObject)).position;
        Vector3 targetPosition = cursor.position;

        if(interactionStatus != Status.Null)
        {

            switch (interactionStatus)
            {
                case Status.MoveToward:
                    cursor.position += (controllerPosition - targetPosition).normalized * speed;
                    break;
                case Status.MoveAway:
                    cursor.position -= (controllerPosition - targetPosition).normalized * speed;
                    break;
                case Status.RotateRight:
                    obj.Rotate(Vector3.up, -rotationSpeed, Space.World);
                    break;
                case Status.RotateLeft:
                    obj.Rotate(Vector3.up, rotationSpeed, Space.World);
                    break;
            }

        }

    }

    private void ControllerEvents_TouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (!isGrab) return;

        Vector2 axis = e.touchpadAxis;
        if(Mathf.Abs(axis.y) > Mathf.Abs(axis.x))
        {
            if (axis.y > 0)
            {
                interactionStatus = Status.MoveAway;
            }else
            {
                interactionStatus = Status.MoveToward;
            }
        }
        else
        {
            if (axis.x > 0)
            {
                interactionStatus = Status.RotateRight;
            }
            else
            {
                interactionStatus = Status.RotateLeft;
            }
        }
        if (isGrab) interactGrab.ForceRelease();
        foreach(InteractionModule m in overrideList)
        {
            m.Disable();
        }
    }

    private void Grab(object sender, ObjectInteractEventArgs e)
    {
        obj = e.target.transform;
        isGrab = true;
        //if(padTouch && triggerPressed)
        //{
        //    obj = e.target.transform;
        //    isGrab = true;
        //}
        //else
        //{
        //    interactGrab.ForceRelease();
        //}

    }

}
