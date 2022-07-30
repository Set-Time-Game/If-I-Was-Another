using System;
using Types.Classes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

namespace Systems.General.Control
{
    [Serializable]
    public class ControlStick : OnScreenStick
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private Vector2 limitRange = Vector2.zero;
        [SerializeField] private Canvas canvas;

        [SerializeField] public AxisOptions mode;

        [NonSerialized] public UnityEvent OnPointerDownEvent;
        [NonSerialized] public UnityEvent OnPointerUpEvent;
        [NonSerialized] public UnityEvent OnDragEvent;

        private Camera _camera;
        private Vector2 _input = Vector2.zero;

        private void Start() => _camera = canvas.worldCamera;

        public static Vector2 SnapInput(Vector2 input, AxisOptions mode)
        {
            var normal = input.normalized;
            input = mode switch
            {
                AxisOptions.Free => input,
                AxisOptions.Fixed => Mathf.Abs(normal.x) > Mathf.Abs(normal.y)
                    ? new Vector2(normal.x, 0)
                    : new Vector2(0, normal.y),
                _ => input
            };
            return input.magnitude > 0 ? input.normalized : Vector2.zero;
        }

        public void SetMode(AxisOptions option)
        {
            mode = option;
        }
    }
    
    [Serializable]
    public enum AxisOptions : byte
    {
        Fixed,
        Free
    }
}