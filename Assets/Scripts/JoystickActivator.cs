using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickActivator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
    [SerializeField]
    private VirtualJoystick _virtualJoystick;

    private void Awake() {
        _virtualJoystick.Fade(0f, 0f);
    }

    public void OnPointerDown(PointerEventData eventData) {
        _virtualJoystick.transform.position = eventData.position;
        _virtualJoystick.Fade(1f, 0.5f);
    }
    public void OnPointerUp(PointerEventData eventData) {
        _virtualJoystick.Fade(0f, 0.5f);
        _virtualJoystick.OnPointerUp(eventData);
    }
    public void OnDrag(PointerEventData eventData) {
        _virtualJoystick.OnDrag(eventData);
    }
}
