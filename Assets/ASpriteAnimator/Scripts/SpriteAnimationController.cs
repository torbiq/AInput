using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteAnimationController : MonoBehaviour {
    [SerializeField]
    private bool _isOwnTimeScale;
    [SerializeField]
    private bool _isUnscaledDeltaTime;
    [SerializeField]
    private float _timeScale;
    [SerializeField]
    private List<SpriteAnimation> _animations;
    [SerializeField]
    private SpriteRenderer _renderer;

    private SpriteAnimation _lastAnimation;
    private int _lastFrameIndex;
    //private float _lastUpdTime;
    private float _nextUpdTime;

    public bool isOwnTimeScale { get { return _isOwnTimeScale; } private set { _isOwnTimeScale = value; } }
    public bool isUnscaledDeltaTime { get { return _isUnscaledDeltaTime; } private set { _isUnscaledDeltaTime = value; } }
    public float timeScale { get { return _timeScale; } private set { _timeScale = value; } }
    public List<SpriteAnimation> animations { get { return _animations; } private set { _animations = value; } }

    private void Awake() {
        SetAnimation(_animations.Last());
    }

    public bool SetAnimation(string name, int frame = 0) {
        var animation = _animations.Find(anim => anim.name == name);
        if (animation != null) {
            SetAnimation(animation, frame);
            return true;
        }
        return false;
    }
    public void SetAnimation(SpriteAnimation animation, int frame = 0) {
        _lastAnimation = animation;
        SetFrame(frame);
    }
    public void SetFrame(int frame) {
        if (_lastAnimation != null) {
            _lastFrameIndex = frame > 0 ? frame % _lastAnimation.spriteFrames.Count : 0;
            SetFrame(_lastAnimation.spriteFrames[_lastFrameIndex]);
        }
    }
    public void SetFrame(Sprite sprite) {
        _renderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time >= _nextUpdTime) {
            if (_lastAnimation != null) {
                float localTimeScale = 0f;
                if (_isOwnTimeScale) {
                    localTimeScale = _timeScale;
                }
                else {
                    if (_isUnscaledDeltaTime) {
                        localTimeScale = Time.unscaledDeltaTime;
                    }
                    else {
                        localTimeScale = Time.timeScale;
                    }
                }
                float defaultTimeDelta = 1f / _lastAnimation.fps;
                float timeScaledDefaultAnimDeltaTime = defaultTimeDelta * localTimeScale;
                int framesToUpdate = (int)((Time.time - _nextUpdTime) / timeScaledDefaultAnimDeltaTime);
                if (framesToUpdate > 0) {
                    SetFrame(_lastFrameIndex + framesToUpdate);
                    float timeOffsetOfUpdate = framesToUpdate * timeScaledDefaultAnimDeltaTime;
                    _nextUpdTime += timeOffsetOfUpdate;
                }
            }
        }
	}
}
