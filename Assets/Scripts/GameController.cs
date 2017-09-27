using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    #region Coins panel
    [SerializeField]
    private RectTransform _coinsRectTransform;
    [SerializeField]
    private float _coinsPanelXShow = 0f;
    [SerializeField]
    private float _coinsPanelXHide = -600f;
    [SerializeField]
    private float _coinsPanelShowDelay = 0.5f;
    [SerializeField]
    private float _coinsPanelHideDelay = 0.5f;
    [SerializeField]
    private Ease _coinsPanelShowEase = Ease.OutElastic;
    [SerializeField]
    private Ease _coinsPanelHideEase = Ease.InQuad;

    private Tween ShowCoins() {
        return _coinsRectTransform
            .DOAnchorPos(new Vector2(_coinsPanelXShow, _coinsRectTransform.anchoredPosition.y), _coinsPanelShowDelay).SetEase(_coinsPanelShowEase);
    }
    private Tween HideCoins() {
        return _coinsRectTransform
            .DOAnchorPos(new Vector2(_coinsPanelXHide, _coinsRectTransform.anchoredPosition.y), _coinsPanelHideDelay).SetEase(_coinsPanelHideEase);
    }
    #endregion

    #region Bottom panel rect transform
    [SerializeField]
    private RectTransform _bottomPanelRectTransform;
    [SerializeField]
    private float _bottomPanelYShow = 0f;
    [SerializeField]
    private float _bottomPanelYHide = -400f;
    [SerializeField]
    private float _bottomPanelShowDelay = 0.5f;
    [SerializeField]
    private float _bottomPanelHideDelay = 0.5f;
    [SerializeField]
    private Ease _bottomPanelShowEase = Ease.OutElastic;
    [SerializeField]
    private Ease _bottomPanelHideEase = Ease.InQuad;

    private Tween ShowBottomPanel() {
        return _bottomPanelRectTransform
            .DOAnchorPos(new Vector2(_bottomPanelRectTransform.anchoredPosition.x, _bottomPanelYShow), _bottomPanelShowDelay).SetEase(_bottomPanelShowEase);
    }
    private Tween HideBottomPanel() {
        return _bottomPanelRectTransform
            .DOAnchorPos(new Vector2(_bottomPanelRectTransform.anchoredPosition.x, _bottomPanelYHide), _bottomPanelHideDelay).SetEase(_bottomPanelHideEase);
    }
    #endregion

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

    private void OnIntroComplete() {
        DOVirtual.DelayedCall(0.5f, () => {
            ShowCoins();
        });
        SwitchPage(PageN.Page_1);
        ShowBottomPanel();
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
