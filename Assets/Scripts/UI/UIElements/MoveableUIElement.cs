using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace BitcoinMiner {
    public class MoveableUIElement : AUIElement {
        [SerializeField]
        private RectTransform _showPosition;
        [SerializeField]
        private RectTransform _hidePosition;
        [SerializeField]
        private Ease _showEase = Ease.OutQuart;
        [SerializeField]
        private Ease _hideEase = Ease.InQuart;
        [SerializeField]
        private float _showDuration = 0.5f;
        [SerializeField]
        private float _hideDuration = 0.5f;
        [SerializeField]
        private bool _isShowMovementEnabled = true;
        [SerializeField]
        private bool _isHideMovementEnabled = true;

        public RectTransform showPosition { get { return _showPosition; } set { _showPosition = value; } }
        public RectTransform hidePosition { get { return _hidePosition; } set { _hidePosition = value; } }
        public Ease showEase { get { return _showEase; } set { _showEase = value; } }
        public Ease hideEase { get { return _hideEase; } set { _hideEase = value; } }
        public float showDuration { get { return _showDuration; } set { _showDuration = value; } }
        public float hideDuration { get { return _hideDuration; } set { _hideDuration = value; } }

        public class TweenList {
            private List<Tween> _tweenList;
            public void Add(Tween tween) {
                _tweenList.Add(tween);
            }
            public void KillAll(bool complete = false) {
                for (int i = 0; i < _tweenList.Count; i++) {
                    Tween tween = _tweenList[i];
                    //if (tween != null) {
                        //if (tween.IsPlaying()) {
                            tween.Kill(complete);
                        //}
                    //}
                }
                _tweenList.Clear();
            }
            public TweenList() {
                _tweenList = new List<Tween>();
            }
        }

        private TweenList _tweenList = new TweenList();

        protected override Tween Show(RectTransform controlledTransform) {
            _tweenList.KillAll();
            if (!controlledTransform.gameObject.activeSelf) {
                controlledTransform.anchoredPosition = _hidePosition.anchoredPosition;
                controlledTransform.gameObject.SetActive(true);
            }
            Tween tween = null;
            if (_isShowMovementEnabled) {
                tween = controlledTransform.DOAnchorPos(_showPosition.anchoredPosition, _showDuration).SetEase(_showEase);
            }
            else {
                controlledTransform.anchoredPosition = _showPosition.anchoredPosition;
                tween = DOVirtual.DelayedCall(_showDuration, null, false);
            }
            _tweenList.Add(tween);
            return tween;
        }
        protected override Tween Hide(RectTransform controlledTransform) {
            _tweenList.KillAll();
            _tweenList.Add(DOVirtual.DelayedCall(_hideDuration, () => { controlledTransform.gameObject.SetActive(false); }, false));
            Tween tween = null;
            if (_isHideMovementEnabled) {
                tween = controlledTransform.DOAnchorPos(_hidePosition.anchoredPosition, _hideDuration).SetEase(_hideEase);
            }
            else {
                _tweenList.Add(DOVirtual.DelayedCall(_hideDuration, () => {
                    controlledTransform.anchoredPosition = _hidePosition.anchoredPosition;
                }));
                tween = DOVirtual.DelayedCall(_hideDuration, null, false);
            }
            _tweenList.Add(tween);
            return tween;
        }
    }
}