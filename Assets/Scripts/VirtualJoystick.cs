using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
    private RectTransform _joystickBackgroundTransform;
    [SerializeField]
    private RectTransform _joystickButtonTransform;
    [SerializeField]
    private Image _bgImage;
    [SerializeField]
    private Image _joystickImage;

    private Vector3 _inputVector;
    
    public void Fade(float endValue, float duration) {
        _bgImage.DOFade(endValue, duration);
        _joystickImage.DOFade(endValue, duration);
    }

    public virtual void OnDrag(PointerEventData ped) {
        Vector2 distance = ped.position - (Vector2)_joystickBackgroundTransform.position;
        Vector2 distanceScaled = new Vector2(distance.x / (_joystickBackgroundTransform.sizeDelta.x / 4f),
            distance.y / (_joystickBackgroundTransform.sizeDelta.y / 4f));
        if (distanceScaled.sqrMagnitude > 1f) {
            distanceScaled.Normalize();
        }
        _joystickButtonTransform.anchoredPosition = Vector2.Scale(distanceScaled, _joystickButtonTransform.sizeDelta);
        _inputVector = distanceScaled;
    }
    public virtual void OnPointerDown(PointerEventData ped) {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped) {
        _joystickImage.rectTransform.anchoredPosition = Vector3.zero;
        _inputVector = Vector3.zero;
    }

    public float Horizontal() {
        return _inputVector.x;
    }
    public float Vertical() {
        return _inputVector.z;
    }
    public Vector2 GetDirection() {
        return _inputVector;
    }
}
