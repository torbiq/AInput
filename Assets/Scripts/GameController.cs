using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BitcoinMiner {
    public enum AppState {
        Intro,

    }
    public enum PageN {
        Page_1,
        Page_2,
        Page_3,
        Page_4,
    }
    [Serializable]
    public class Page {
        public PageN pageN;
        public Button header;
        public RectTransform outline;
        public RectTransform content;
        public void SetActive(bool active) {
            outline.GetComponent<Image>().DOFillAmount(active ? 1f : 0f, 0.25f).SetEase(Ease.Linear);
            content.gameObject.SetActive(active);
        }
    }

    [Serializable]
    public class VideoCardInfo {
        public Sprite image;
        public float btcPerSecond;
        public float price;
        public string name;
    }

    [Serializable]
    public class PlayerData {
        public float btcCount;
        public float btcPerSecond;
    }

    public class GameController : MonoBehaviour {
        #region Intro
        [SerializeField]
        private Text _textBitcoin;
        [SerializeField]
        private Text _textMaker;

        public bool introEnabled = true;
        public float awakeDelay = 1f;
        public float fadeInDelay = 1f;
        public float fadeOutDelay = 1f;
        public float showDelay = 2.5f;
        public float endScale = 1.3f;
        #endregion

        [SerializeField]
        private AUIElement _coinsElement;
        [SerializeField]
        private AUIElement _bottomPanelElement;
        [SerializeField]
        private AUIElement _mainPage;
        
        #region Pages switch control
        [SerializeField]
        private Button _page1Button;
        [SerializeField]
        private Button _page2Button;
        [SerializeField]
        private Button _page3Button;
        [SerializeField]
        private Button _page4Button;

        public List<Page> pages;
        private PageN _lastPage = (PageN)(-1);

        private void SwitchPage(PageN pageN) {
            if (pageN == _lastPage)
                return;

            var pageFoundPrevious = pages.Find(page => page.pageN == _lastPage);
            if (pageFoundPrevious != null) {
                pageFoundPrevious.SetActive(false);
            }
            var pageFoundNext = pages.Find(page => page.pageN == pageN);
            if (pageFoundNext != null) {
                _lastPage = pageN;
                pageFoundNext.SetActive(true);
            }
        }

        private void SwitchPage1() {
            SwitchPage(PageN.Page_1);
        }
        private void SwitchPage2() {
            SwitchPage(PageN.Page_2);
        }
        private void SwitchPage3() {
            SwitchPage(PageN.Page_3);
        }
        private void SwitchPage4() {
            SwitchPage(PageN.Page_4);
        }
        #endregion

        #region Videocard info
        public List<VideoCardInfo> videoCardInfos;
        #endregion

        private void OnIntroComplete() {
            DOVirtual.DelayedCall(0.5f, () => {
                //_coinsElement.IsActive = true;
            });
            SwitchPage(PageN.Page_1);
            //_bottomPanelElement.IsActive = true;
            _mainPage.IsActive = true;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.C)) {
                _coinsElement.IsActive = !_coinsElement.IsActive;
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                _bottomPanelElement.IsActive = !_bottomPanelElement.IsActive;
            }
            if (Input.GetKeyDown(KeyCode.M)) {
                _mainPage.IsActive = !_mainPage.IsActive;
            }
        }

        private void Awake() {
            var startColorBitcoinText = _textBitcoin.color;
            startColorBitcoinText.a = 0f;
            _textBitcoin.color = startColorBitcoinText;

            var startColorMakerText = _textMaker.color;
            startColorMakerText.a = 0f;
            _textMaker.color = startColorMakerText;

            #region Button initialization
            _page1Button.onClick.AddListener(SwitchPage1);
            _page2Button.onClick.AddListener(SwitchPage2);
            _page3Button.onClick.AddListener(SwitchPage3);
            _page4Button.onClick.AddListener(SwitchPage4);
            #endregion

            if (introEnabled) {
                DOVirtual.DelayedCall(awakeDelay, () => {

                    //_textBitcoin.GetComponent<RectTransform>()
                    //    .DOSizeDelta(_textBitcoin.GetComponent<RectTransform>().sizeDelta * 2.5f, fadeInDelay + showDelay + fadeOutDelay);
                    //_textMaker.GetComponent<RectTransform>()
                    //    .DOSizeDelta(_textMaker.GetComponent<RectTransform>().sizeDelta * 2.5f, fadeInDelay + showDelay + fadeOutDelay);
                    _textBitcoin.transform.DOScale(endScale, fadeInDelay + showDelay + fadeOutDelay).SetEase(Ease.Linear);
                    _textMaker.transform.DOScale(endScale, fadeInDelay + showDelay + fadeOutDelay).SetEase(Ease.Linear);

                    _textBitcoin.DOFade(1f, fadeInDelay);
                    _textMaker.DOFade(1f, fadeInDelay);

                    DOVirtual.DelayedCall(fadeInDelay + showDelay, () => {
                        _textBitcoin.DOFade(0f, fadeOutDelay);
                        _textMaker.DOFade(0f, fadeOutDelay).OnComplete(OnIntroComplete);
                    });
                });
            }
            else {
                OnIntroComplete();
            }
        }
    }
}