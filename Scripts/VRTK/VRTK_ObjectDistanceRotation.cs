using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTK_ObjectDistanceRotation : MonoBehaviour
{
    public VRTK_ControllerEvents controller;
    public Transform obj;
    private float speed = 3f;
    private enum InteractionStatus
    {
        Null, DistanceRotate
    }
    private InteractionStatus status = InteractionStatus.Null;

    // Start is called before the first frame update
    void Start()
    {
        controller.TriggerPressed += Controller_TriggerPressed;
        controller.TriggerReleased += Controller_TriggerReleased;
    }

    private void Controller_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        status = InteractionStatus.Null;
    }

    private void Controller_TriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        status = InteractionStatus.DistanceRotate;
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case InteractionStatus.DistanceRotate:

                Vector3 velocity = VRTK_DeviceFinder.GetControllerVelocity(VRTK_ControllerReference.GetControllerReference(controller.gameObject));
                velocity = controller.transform.InverseTransformVector(velocity);

                obj.Rotate(Vector3.up, -velocity.x * speed, Space.World);
                break;
        }
    }
}
