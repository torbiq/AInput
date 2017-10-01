using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollSelection : MonoBehaviour {
    private ScrollRect _scrollRect;
    public ButtonEventHandler buttonEventHandler;
    private int _closestIndex = 0;

    void Awake () {
        _scrollRect = GetComponent<ScrollRect>();
        Select(0);
	}
	
    public void Select(int element) {
        var elementImage = _scrollRect.content.GetChild(_closestIndex).GetComponent<Image>();
        elementImage.DOKill();
        elementImage.DOColor(Color.white, 0.5f);

        _closestIndex = element;
        var nextImage = _scrollRect.content.GetChild(_closestIndex).GetComponent<Image>();
        nextImage.DOKill();
        nextImage.DOColor(Color.red, 0.5f);
    }

    //private Vector2 CenterDelta(RectTransform transform) {
    //    return _scrollRect.viewport.position - transform.position;
    //}
}
