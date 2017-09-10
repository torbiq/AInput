using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    private Image _bgImg;
    private Image _jsImg;
    private Vector3 _inputVector;

    // Use this for initialization
    void Start () {
        _bgImg = GetComponent<Image>();
        _jsImg = transform.GetChild(0).GetComponent<Image>();
	}
	
    public virtual void OnDrag(PointerEventData ped) {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos)) {
            pos.x = (pos.x / _bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _bgImg.rectTransform.sizeDelta.y);

            _inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            _jsImg.rectTransform.anchoredPosition = new Vector3(_inputVector.x * (_bgImg.rectTransform.sizeDelta.x / 2),
                _inputVector.z * (_bgImg.rectTransform.sizeDelta.y / 2));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped) {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped) {
        _inputVector = Vector3.zero;

        _jsImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal() {
        if (_inputVector.x != 0) return _inputVector.x;
        else return Input.GetAxis("Horizontal");
    }

    public float Vertical() {
        if (_inputVector.z != 0) return _inputVector.z;
        else return Input.GetAxis("Vertical");
    }

    public Vector2 GetDirection() {
        return new Vector2(_inputVector.x, _inputVector.z);
    }
}
