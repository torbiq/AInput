using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BitcoinMiner {
    [Serializable]
    [RequireComponent(typeof(RectTransform))]
    public abstract class AUIElement : MonoBehaviour {
        [SerializeField]
        private RectTransform _controlledTransform;

        private bool _isActive;

        protected abstract Tween Show(RectTransform rectTransformList);
        protected abstract Tween Hide(RectTransform rectTransformList);

        public UnityEvent OnStartShow;
        public UnityEvent OnStartHide;

        public UnityEvent OnEndShow;
        public UnityEvent OnEndHide;

        public bool IsActive {
            get {
                return _isActive;
            }
            set {
                if (_isActive != value) {
                    _isActive = value;
                    if (_isActive) {
                        OnStartShow.Invoke();
                        Show(_controlledTransform).OnComplete(OnEndShow.Invoke);
                    }
                    else {
                        OnStartHide.Invoke();
                        Hide(_controlledTransform).OnComplete(OnEndHide.Invoke);
                    }
                }
            }
        }

        protected virtual void Awake() {
            if (_controlledTransform == null) {
                _controlledTransform = GetComponent<RectTransform>();
            }
        }
    }
}
