using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BitcoinMiner {
    [RequireComponent(typeof(Button))]
    public class ButtonClickHandler : MonoBehaviour {
        private Button _button;
        [SerializeField]
        private List<AudioClip> _clips;
        [SerializeField]
        private bool _visualPunchEnabled = true;
        [SerializeField]
        private Vector3 _punchScale = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField]
        private int _vibrateCount = 10;
        [SerializeField]
        private float _punchDuration = 0.5f;
        private void Awake() {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickHandler);
        }
        private void OnClickHandler() {
            foreach (var clip in _clips) {
                AudioController.Instance.Play(clip);
            }
            if (_visualPunchEnabled) {
                _button.transform.DOPunchScale(_punchScale, _punchDuration, _vibrateCount);
            }
        }
    }
}