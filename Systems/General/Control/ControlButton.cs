using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace Systems.General.Control
{
    public class ControlButton : OnScreenButton, IPointerDownHandler, IPointerUpHandler
    {
        public new void OnPointerUp(PointerEventData data)
        {
            SendValueToControl(0.0f);
        }

        public new void OnPointerDown(PointerEventData data)
        {
            SendValueToControl(1.0f);
        }
    }
}