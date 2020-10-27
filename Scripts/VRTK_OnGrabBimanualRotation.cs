using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTK_OnGrabBimanualRotation : MonoBehaviour
{

    public VRTK_ControllerEvents controllerEvents;
    public VRTK_Pointer pointer;
    public VRTK_InteractGrab interactGrabR;
    public VRTK_InteractGrab interactGrabL;

    Transform objR;
    Transform objL;

    Vector3 prevPositionR;
    Vector3 prevPositionL;

    private void Update()
    {
        if(objR != null && objL != null && objR == objL)
        {

            Vector3 right = VRTK_DeviceFinder.GetControllerRightHand().transform.right;
            Vector3 up = VRTK_DeviceFinder.GetControllerRightHand().transform.up;

            Vector3 currentPositionR = VRTK_DeviceFinder.GetControllerRightHand().transform.position;
            Vector3 currentPositionL = VRTK_DeviceFinder.GetControllerLeftHand().transform.position;

            Vector3 currentLeftToRight = currentPositionR - currentPositionL;
            Vector3 prevLeftToRight = prevPositionR - prevPositionL;

            interactGrabL.ForceRelease();
            interactGrabR.ForceRelease();

            objL.Rotate(Vector3.up, Vector3.SignedAngle(prevLeftToRight, currentLeftToRight, up), Space.World);

            prevPositionL = currentPositionL;
            prevPositionR = currentPositionR;
        }
    }

    protected void Awake()
    {
        interactGrabR.ControllerStartGrabInteractableObject += InteractGrabR_ControllerStartGrabInteractableObject;
        interactGrabL.ControllerStartGrabInteractableObject += InteractGrabL_ControllerStartGrabInteractableObject;
        interactGrabR.GrabButtonReleased += InteractGrabR_GrabButtonReleased;
        interactGrabL.GrabButtonReleased += InteractGrabL_GrabButtonReleased;
    }

    private void InteractGrabL_GrabButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
        objL = null;
    }

    private void InteractGrabR_GrabButtonReleased(object sender, ControllerInteractionEventArgs e)
    {
        objR = null;
    }

    private void InteractGrabL_ControllerStartGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        objL = e.target.transform;
        prevPositionL = VRTK_DeviceFinder.GetControllerLeftHand().transform.position;
    }

    private void InteractGrabR_ControllerStartGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        objR = e.target.transform;
        prevPositionR = VRTK_DeviceFinder.GetControllerRightHand().transform.position;
    }



}
