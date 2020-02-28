using VRTK;

public static class ControllerEventRegistrator
{
    public enum Button
    {
        Trigger, TouchPad, Grip, ButtonOne, ButtonTwo, TouchPadTwo
    }
    public enum ButtonAction
    {
        Pressed, Released, TouchStart, TouchEnd, Clicked, Unclicked, AxisChanged
    }
    public static void Register(VRTK_ControllerEvents controllerEvent, Button button, ButtonAction buttonAction, ControllerInteractionEventHandler action)
    {
        switch (button)
        {
            case Button.Trigger:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.TriggerPressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.TriggerReleased += action;
                        break;
                    case ButtonAction.Clicked:
                        controllerEvent.TriggerClicked += action;
                        break;
                    case ButtonAction.Unclicked:
                        controllerEvent.TriggerUnclicked += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.TriggerTouchStart += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.TriggerTouchEnd += action;
                        break;
                }
                break;
            case Button.TouchPad:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.TouchpadPressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.TouchpadReleased += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.TouchpadTouchStart += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.TouchpadTouchEnd += action;
                        break;
                }
                break;
            case Button.Grip:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.GripPressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.GripReleased += action;
                        break;
                    case ButtonAction.Clicked:
                        controllerEvent.GripClicked += action;
                        break;
                    case ButtonAction.Unclicked:
                        controllerEvent.GripUnclicked += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.GripTouchStart += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.GripTouchEnd += action;
                        break;
                }
                break;
            case Button.ButtonOne:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.ButtonOnePressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.ButtonOneReleased += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.ButtonOneReleased += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.ButtonOneTouchEnd += action;
                        break;
                }
                break;
            case Button.ButtonTwo:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.ButtonTwoPressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.ButtonTwoReleased += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.ButtonTwoTouchStart += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.ButtonTwoTouchEnd += action;
                        break;
                }
                break;
            case Button.TouchPadTwo:
                switch (buttonAction)
                {
                    case ButtonAction.Pressed:
                        controllerEvent.TouchpadTwoPressed += action;
                        break;
                    case ButtonAction.Released:
                        controllerEvent.TouchpadTwoReleased += action;
                        break;
                    case ButtonAction.TouchStart:
                        controllerEvent.TouchpadTwoTouchStart += action;
                        break;
                    case ButtonAction.TouchEnd:
                        controllerEvent.TouchpadTwoTouchEnd += action;
                        break;
                    case ButtonAction.AxisChanged:
                        controllerEvent.TouchpadAxisChanged += action;
                        break;
                }
                break;
        }
    }
}