//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;

//public class AudioController : MonoBehaviour {
//    private List<AudioSource> _audioSourcesList;
//    private List<AudioSource> _pausedSourcesList;
//    private bool _isMusicPaused;
//    private bool _isSpeechPaused;

//    private Dictionary<string, AudioClip> _langClipsDict;
//    private Dictionary<string, AudioClip> _nonLangClipsDict;

//    public Dictionary<string, AudioClip> langClipsDict {
//        get { return _langClipsDict; }
//    }
//    public Dictionary<string, AudioClip> nonLangClipsDict {
//        get { return _nonLangClipsDict; }
//    }

//    private GameObject _audioContainerGO;
//    private GameObject _musicContainerGO;

//    private AudioSource _mainSpeech;
//    private AudioSource _musicSource;

//    private float _musicVolume = 1;
//    private float _speechVolume = 1;

//    #region Singletone implementation
//    static AudioController() {
//        OnLevelLoaded();

//        UnityEngine.SceneManagement.SceneManager.sceneLoaded += delegate { OnLevelLoaded(); };

//        Instance._musicContainerGO = Instance.gameObject;
//        Instance._musicContainerGO.name = "AudioPlayer_Music";

//        Instance._langClipsDict = new Dictionary<string, AudioClip>();
//        Instance._nonLangClipsDict = new Dictionary<string, AudioClip>();

//        Instance._musicSource = Instance.gameObject.AddComponent<AudioSource>();
//        Instance._musicSource.loop = true;
//        Instance._musicVolume = 1f;
//        Instance._musicSource.volume = Instance._musicVolume;

//        Instance._mainSpeech = Instance.gameObject.AddComponent<AudioSource>();
//        Instance._mainSpeech.volume = Instance._speechVolume;

//        UnityEngine.SceneManagement.SceneManager.sceneLoaded += delegate { Instance.StopSpeech(0); };
//    }

//    private AudioController() {
//    }

//    public static AudioController Instance { get; private set; }
//    #endregion

//    #region Mono Behaviour
//    private static void OnLevelLoaded() {
//        var audioContainer = new GameObject();
//        Instance._audioContainerGO = audioContainer;
//        Instance._audioSourcesList = new List<AudioSource>();
//        Instance._pausedSourcesList = new List<AudioSource>();
//        Instance._audioContainerGO.name = "AudioPlayer_Sounds";
//    }
//    private void Awake() {
//        if (Instance && Instance != this) {
//            DestroyImmediate(gameObject);
//            return;
//        }
//        DontDestroyOnLoad(gameObject);
//    }
//    private void Update() {
//        bool soundEnabled = true;
//        bool musicEnabled = true;
//        _speechVolume = 1f;
//        _musicVolume = 1f;
//        _mainSpeech.volume = _speechVolume;

//        if (_musicSource.mute == musicEnabled) {
//            _musicSource.mute = !musicEnabled;
//        }

//        for (int i = 0; i < _audioSourcesList.Count; i++) {
//            var source = _audioSourcesList[i];

//            // If sound was completed and is not not paused.
//            if ((source.time == 0f && !source.isPlaying) // Wasn't completed.
//                && !_pausedSourcesList.Contains(source)) // Isn't paused.
//            {
//                RemoveSource(source);
//                continue;
//            }

//            // If sound is not muted when sound setting inactive.
//            if (source.mute == soundEnabled) {
//                source.mute = !soundEnabled;
//            }
//        }
//    }
//    #endregion

//    #region Incapsulated sources control methods
//    private AudioSource CreateSource() {
//        var audioSource = _audioContainerGO.AddComponent<AudioSource>();
//        _audioSourcesList.Add(audioSource);
//        return audioSource;
//    }
//    private bool RemoveSource(AudioSource audioSource) {
//        _pausedSourcesList.Remove(audioSource);
//        DestroyObject(audioSource);
//        return _audioSourcesList.Remove(audioSource);
//    }
//    private AudioSource FindSource(string audioClipName) {
//        int sourcesCount = _audioSourcesList.Count;
//        for (int i = 0; i < sourcesCount; i++) {
//            if (_audioSourcesList[i].clip) {
//                if (_audioSourcesList[i].clip.name == audioClipName) {
//                    return _audioSourcesList[i];
//                }
//            }
//        }
//        return null;
//    }
//    private AudioSource FindSource(AudioClip clip) {
//        int sourcesCount = _audioSourcesList.Count;
//        for (int i = 0; i < sourcesCount; i++) {
//            if (_audioSourcesList[i].clip == clip) {
//                return _audioSourcesList[i];
//            }
//        }
//        return null;
//    }
//    private Tween SetVolumeSource(AudioSource audioSource, float volume, float delayFade) {
//        if (!audioSource) {
//            Log.Error("No clip received");
//            return null;
//        }
//        if (delayFade == 0f) {
//            audioSource.volume = volume;
//        }
//        audioSource.DOKill(true);
//        return audioSource.DOFade(volume, delayFade);
//    }
//    private float PlaySource(AudioClip audioClip, AudioSource audioSource, float delayFade, float volume, bool loop) {
//        if (!audioClip) {
//            Log.Error("No clip received");
//            return 0;
//        }
//        audioSource.loop = loop;
//        audioSource.clip = audioClip;
//        audioSource.Play();
//        SetVolumeSource(audioSource, volume, delayFade);
//        return audioClip.length;
//    }
//    private void PauseSource(AudioSource audioSource, float delayFade) {
//        _pausedSourcesList.Add(audioSource);
//        SetVolumeSource(audioSource, 0f, delayFade).OnComplete(audioSource.Pause);
//    }
//    private void UnPauseSource(AudioSource audioSource, float delayFade, bool loop) {
//        _pausedSourcesList.Remove(audioSource);
//        audioSource.UnPause();
//        SetVolumeSource(audioSource, PrototypeHelpers.SoundVolume(), delayFade);
//    }
//    private void StopSource(AudioSource audioSource, float delayFade) {
//        SetVolumeSource(audioSource, 0f, delayFade).OnComplete(delegate {
//            RemoveSource(audioSource);
//        });
//    }
//    #endregion

//    #region Prototype load sounds replacement
//    /// <summary>
//    /// Gets clip from non-language dictionary.
//    /// </summary>
//    public AudioClip GetNonLangClip(string audioClipName) {
//        AudioClip audioClip = null;
//        _nonLangClipsDict.TryGetValue(audioClipName, out audioClip);
//        return audioClip;
//    }
//    /// <summary>
//    /// Gets clip from language dictionary.
//    /// </summary>
//    public AudioClip GetLangClip(string audioClipName) {
//        AudioClip audioClip = null;
//        _langClipsDict.TryGetValue(audioClipName, out audioClip);
//        return audioClip;
//    }
//    /// <summary>
//    /// Gets clip from language and non-language audio dictionaries.
//    /// </summary>
//    public AudioClip GetClip(string audioClipName) {
//        AudioClip audioClipReturned = GetLangClip(audioClipName);
//        if (!audioClipReturned) {
//            audioClipReturned = GetNonLangClip(audioClipName);
//        }
//        return audioClipReturned;
//    }

//    /// <summary>
//    /// Called when language is changed in prototype.
//    /// </summary>
//    private void OnLanguageChangedEventHandler(Languages.Language language) {
//        LoadLanguageAudio(language);
//    }
//    /// <summary>
//    /// Loads language audio clips.
//    /// </summary>
//    private void LoadLanguageAudio(Languages.Language language) {
//        _langClipsDict = new Dictionary<string, AudioClip>();
//        AudioClip[] langClips = Resources.LoadAll<AudioClip>("Sounds/Multilanguage/" + language.ToString());
//        int length = langClips.Length;
//        for (int i = 0; i < length; i++) {
//            try {
//                _langClipsDict.Add(langClips[i].name, langClips[i]);
//            }
//            catch (System.Exception exception) {
//                Log.Error(exception);
//                Log.Error("Key [" + langClips[i].name + "] is probaubly already contained by lang audio dictionary.");
//            }
//        }
//    }
//    /// <summary>
//    /// Loads non-language audio clips.
//    /// </summary>
//    private void LoadNonLangAudio() {
//        _nonLangClipsDict = new Dictionary<string, AudioClip>();
//        AudioClip[] nonLangClips = Resources.LoadAll<AudioClip>("Sounds/NonLanguage");
//        int length = nonLangClips.Length;
//        for (int i = 0; i < length; i++) {
//            try {
//                _nonLangClipsDict.Add(nonLangClips[i].name, nonLangClips[i]);
//            }
//            catch (System.Exception exception) {
//                Log.Error(exception);
//                Log.Error("Key [" + nonLangClips[i].name + "] is probaubly already contained by non-lang audio dictionary.");
//            }
//        }
//    }
//    #endregion

//    #region Music control methods
//    #region Play
//    #region Play default
//    /// <summary>
//    /// Fades current music and plays given clip.
//    /// </summary>
//    public void PlayMusic(AudioClip audioClip, float delayFade = 0f) {
//        _isMusicPaused = false;
//        AudioSource currentMusicSource = _musicSource;
//        StopMusic(delayFade);
//        DOVirtual.DelayedCall(delayFade, delegate {
//            Destroy(currentMusicSource);
//        }, false);
//        _musicSource = Instance.gameObject.AddComponent<AudioSource>();
//        _musicSource.loop = true;
//        _musicSource.volume = 0f;
//        PlaySource(audioClip, _musicSource, delayFade, _musicVolume, true);
//    }

//    /// <summary>
//    /// Fades current music and plays given clip by name.
//    /// </summary>
//    public void PlayMusic(string audioClipName, float delayFade = 0f) {
//        _isMusicPaused = false;
//        AudioSource currentMusicSource = _musicSource;
//        StopMusic(delayFade);
//        DOVirtual.DelayedCall(delayFade, delegate {
//            Destroy(currentMusicSource);
//        }, false);
//        _musicSource = Instance.gameObject.AddComponent<AudioSource>();
//        _musicSource.loop = true;
//        _musicSource.volume = 0f;
//        PlaySource(GetClip(audioClipName), _musicSource, delayFade, _musicVolume, true);
//    }
//    #endregion

//    #region Play or unpause
//    /// <summary>
//    /// Play or resume given clip.
//    /// </summary>
//    public void PlayOrUnPauseMusic(AudioClip audioClip, float delayFade = 0f) {
//        if (IsMusicClip(audioClip) && _isMusicPaused) {
//            UnPauseMusic(delayFade);
//        }
//        else {
//            PlayMusic(audioClip, delayFade);
//        }
//    }

//    /// <summary>
//    /// Play or resume given clip.
//    /// </summary>
//    public void PlayOrUnPauseMusic(string audioClipName, float delayFade = 0f) {
//        if (IsMusicClip(audioClipName) && _isMusicPaused) {
//            UnPauseMusic(delayFade);
//        }
//        else {
//            PlayMusic(audioClipName, delayFade);
//        }
//    }
//    #endregion

//    #region Play if not playing
//    /// <summary>
//    /// Plays given clip if it's not playing now.
//    /// </summary>
//    public void PlayMusicIfNotPlaying(AudioClip audioClip, float delayFade = 0f) {
//        if (!IsMusicPlaying(audioClip)) {
//            PlayMusic(audioClip, delayFade);
//        }
//    }

//    /// <summary>
//    /// Plays given speech if it's not playing now.
//    /// </summary>
//    public float PlayMusicIfNotPlaying(string audioClipName, float delayFade = 0f) {
//        if (!IsMusicPlaying(audioClipName)) {
//            PlayMusic(audioClipName, delayFade);
//        }
//        return 0;
//    }
//    #endregion
//    #endregion

//    #region Pause
//    /// <summary>
//    /// Pauses music.
//    /// </summary>
//    public void PauseMusic(float delayFade = 0f) {
//        _isMusicPaused = true;
//        AudioSource currentMusicSource = _musicSource;
//        SetVolumeSource(currentMusicSource, 0, delayFade).OnComplete(currentMusicSource.Pause);
//    }
//    #endregion

//    #region Unpause
//    /// <summary>
//    ///Unpauses music.
//    /// </summary>
//    public void UnPauseMusic(float delayFade = 0f) {
//        _isMusicPaused = false;
//        _musicSource.UnPause();
//        SetVolumeSource(_musicSource, _musicVolume, delayFade);
//    }
//    #endregion

//    #region Stop
//    /// <summary>
//    /// Stops music.
//    /// </summary>
//    public void StopMusic(float delayFade = 0f) {
//        AudioSource currentMusicSource = _musicSource;
//        SetVolumeSource(currentMusicSource, 0f, delayFade).OnComplete(currentMusicSource.Stop);
//    }
//    #endregion

//    #region Audio state
//    /// <summary>
//    /// Returns current music clip.
//    /// </summary>
//    public AudioClip GetMusicClip() {
//        return _musicSource.clip;
//    }

//    /// <summary>
//    /// Returns if current set music clip equals to given.
//    /// </summary>
//    public bool IsMusicClip(AudioClip audioClip) {
//        return audioClip == GetMusicClip();
//    }

//    /// <summary>
//    /// Returns if current set music clip name equals to given.
//    /// </summary>
//    public bool IsMusicClip(string audioClipName) {
//        var currentMusicClip = GetMusicClip();
//        if (currentMusicClip) {
//            return currentMusicClip.name == audioClipName;
//        }
//        return false;
//    }

//    /// <summary>
//    /// Is music paused?
//    /// </summary>
//    public bool IsMusicPaused() {
//        return _isMusicPaused;
//    }

//    /// <summary>
//    /// Is given music clip paused?
//    /// </summary>
//    public bool IsMusicPaused(AudioClip audioClip) {
//        return IsMusicPaused() && IsMusicClip(audioClip);
//    }

//    /// <summary>
//    /// Is given by name music clip paused?
//    /// </summary>
//    public bool IsMusicPaused(string audioClipName) {
//        return IsMusicPaused() && IsMusicClip(audioClipName);
//    }

//    /// <summary>
//    /// Is music playing?
//    /// </summary>
//    public bool IsMusicPlaying() {
//        return _musicSource.isPlaying;
//    }

//    /// <summary>
//    /// Is playing given clip?
//    /// </summary>
//    public bool IsMusicPlaying(AudioClip audioClip) {
//        return _musicSource.isPlaying && IsMusicClip(audioClip);
//    }

//    /// <summary>
//    /// Is playing given clip?
//    /// </summary>
//    public bool IsMusicPlaying(string audioClipName) {
//        return _musicSource.isPlaying && IsMusicClip(audioClipName);
//    }
//    #endregion

//    #region Volume control
//    /// <summary>
//    /// Set music volume.
//    /// </summary>
//    public Tween SetMusicVolume(float volume, float delayFade = 0f) {
//        _musicVolume = volume;
//        return SetVolumeSource(_musicSource, _musicVolume, delayFade);
//    }

//    /// <summary>
//    /// Returns music volume.
//    /// </summary>
//    public float GetMusicVolume() {
//        return _musicVolume;
//    }

//    /// <summary>
//    /// Returns music source volume.
//    /// </summary>
//    public float GetMusicSourceVolume() {
//        return _musicSource.volume;
//    }
//    #endregion
//    #endregion

//    #region Speech control methods
//    #region Play
//    #region Play default
//    /// <summary>
//    /// Fades current speaker and plays given clip.
//    /// </summary>
//    public float PlaySpeech(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        AudioSource currentSpeechSource = _mainSpeech;
//        StopSpeech(delayFade);
//        DOVirtual.DelayedCall(delayFade, delegate {
//            Destroy(currentSpeechSource);
//        }, false);
//        _mainSpeech = Instance.gameObject.AddComponent<AudioSource>();
//        _mainSpeech.volume = 0f;
//        return PlaySource(audioClip, _mainSpeech, delayFade, _speechVolume, loop);
//    }

//    /// <summary>
//    /// Fades current speaker and plays given clip by name.
//    /// </summary>
//    public float PlaySpeech(string audioClipName, float delayFade = 0f, bool loop = false) {
//        AudioSource currentSpeechSource = _mainSpeech;
//        StopSpeech(delayFade);
//        DOVirtual.DelayedCall(delayFade, delegate {
//            Destroy(currentSpeechSource);
//        }, false);
//        _mainSpeech = Instance.gameObject.AddComponent<AudioSource>();
//        _mainSpeech.volume = 0f;
//        return PlaySource(GetClip(audioClipName), _mainSpeech, delayFade, _speechVolume, loop);
//    }
//    #endregion

//    #region Play or unpause
//    /// <summary>
//    /// Play or resume given clip.
//    /// </summary>
//    public float PlayOrUnPauseSpeech(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        if (IsSpeechPaused(audioClip)) {
//            UnPauseSpeech(delayFade, loop);
//            return 0f;
//        }
//        else {
//            return PlaySpeech(audioClip, delayFade, loop);
//        }
//    }

//    /// <summary>
//    /// Play or resume given clip.
//    /// </summary>
//    public float PlayOrUnPauseSpeech(string audioClipName, float delayFade = 0f, bool loop = false) {
//        if (IsSpeechPaused(audioClipName)) {
//            UnPauseSpeech(delayFade, loop);
//            return 0f;
//        }
//        else {
//            return PlaySpeech(audioClipName, delayFade, loop);
//        }
//    }
//    #endregion

//    #region Play if not playing
//    /// <summary>
//    /// Plays given clip if it's not playing now.
//    /// </summary>
//    public float PlaySpeechIfNotPlaying(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        if (!IsSpeechPlaying(audioClip)) {
//            return PlaySpeech(audioClip, delayFade, loop);
//        }
//        return 0;
//    }

//    /// <summary>
//    /// Plays given speech if it's not playing now.
//    /// </summary>
//    public float PlaySpeechIfNotPlaying(string audioClipName, float delayFade = 0f, bool loop = false) {
//        if (!IsSpeechPlaying(audioClipName)) {
//            return PlaySpeech(audioClipName, delayFade, loop);
//        }
//        return 0;
//    }
//    #endregion
//    #endregion

//    #region Pause
//    /// <summary>
//    /// Pauses speech.
//    /// </summary>
//    public void PauseSpeech(float delayFade = 0f) {
//        _isSpeechPaused = true;
//        AudioSource currentSpeechSource = _mainSpeech;
//        SetVolumeSource(currentSpeechSource, 0, delayFade).OnComplete(currentSpeechSource.Pause);
//    }
//    #endregion

//    #region Unpause
//    /// <summary>
//    ///Unpauses speech.
//    /// </summary>
//    public void UnPauseSpeech(float delayFade = 0f, bool loop = false) {
//        _isSpeechPaused = false;
//        _mainSpeech.UnPause();
//        _mainSpeech.loop = loop;
//        SetVolumeSource(_mainSpeech, _speechVolume, delayFade);
//    }
//    #endregion

//    #region Stop
//    /// <summary>
//    /// Stops speech.
//    /// </summary>
//    public void StopSpeech(float delayFade = 0f) {
//        AudioSource currentSpeechSource = _mainSpeech;
//        SetVolumeSource(currentSpeechSource, 0f, delayFade).OnComplete(currentSpeechSource.Stop);
//    }
//    #endregion

//    #region Audio state
//    /// <summary>
//    /// Returns current speech clip.
//    /// </summary>
//    public AudioClip GetSpeechClip() {
//        return _mainSpeech.clip;
//    }

//    /// <summary>
//    /// Returns if current set speech clip equals to given.
//    /// </summary>
//    public bool IsSpeechClip(AudioClip audioClip) {
//        return audioClip == GetSpeechClip();
//    }

//    /// <summary>
//    /// Returns if current set speech clip name equals to given.
//    /// </summary>
//    public bool IsSpeechClip(string audioClipName) {
//        var currentSpeechClip = GetSpeechClip();
//        if (currentSpeechClip) {
//            return currentSpeechClip.name == audioClipName;
//        }
//        return false;
//    }

//    /// <summary>
//    /// Is speech paused?
//    /// </summary>
//    public bool IsSpeechPaused() {
//        return _isSpeechPaused;
//    }

//    /// <summary>
//    /// Is given speech clip paused?
//    /// </summary>
//    public bool IsSpeechPaused(AudioClip audioClip) {
//        return IsSpeechPaused() && IsSpeechClip(audioClip);
//    }

//    /// <summary>
//    /// Is given by name speech clip paused?
//    /// </summary>
//    public bool IsSpeechPaused(string audioClipName) {
//        return IsSpeechPaused() && IsSpeechClip(audioClipName);
//    }

//    /// <summary>
//    /// Is speech playing?
//    /// </summary>
//    public bool IsSpeechPlaying() {
//        return _mainSpeech.isPlaying;
//    }

//    /// <summary>
//    /// Is playing given clip?
//    /// </summary>
//    public bool IsSpeechPlaying(AudioClip audioClip) {
//        return _mainSpeech.isPlaying && IsSpeechClip(audioClip);
//    }

//    /// <summary>
//    /// Is playing given clip?
//    /// </summary>
//    public bool IsSpeechPlaying(string audioClipName) {
//        return _mainSpeech.isPlaying && IsSpeechClip(audioClipName);
//    }
//    #endregion

//    #region Volume control
//    /// <summary>
//    /// Set speech volume.
//    /// </summary>
//    public Tween SetSpeechVolume(float volume, float delayFade = 0f) {
//        _speechVolume = volume;
//        return SetVolumeSource(_mainSpeech, _speechVolume, delayFade);
//    }

//    /// <summary>
//    /// Returns speech volume.
//    /// </summary>
//    public float GetSpeechVolume() {
//        return _speechVolume;
//    }

//    /// <summary>
//    /// Returns speech source volume.
//    /// </summary>
//    public float GetSpeechSourceVolume() {
//        return _mainSpeech.volume;
//    }
//    #endregion
//    #endregion

//    #region Sound control methods
//    #region Play
//    #region Play default
//    /// <summary>
//    /// Returns true if received clip is valid and then plays audio.
//    /// </summary>
//    public float Play(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        return PlaySource(audioClip, CreateSource(), delayFade, PrototypeHelpers.SoundVolume(), loop);
//    }

//    /// <summary>
//    /// Returns true if received clip name is valid and then plays audio.
//    /// </summary>
//    public float Play(string audioClipName, float delayFade = 0f, bool loop = false) {
//        return PlaySource(GetClip(audioClipName), CreateSource(), delayFade, PrototypeHelpers.SoundVolume(), loop);
//    }
//    #endregion

//    #region Play or unpause
//    /// <summary>
//    /// Unpauses given clip if it's available. If not - plays new instance.
//    /// </summary>
//    public void PlayOrUnPause(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        if (IsSoundPaused(audioClip)) {
//            UnPause(audioClip, delayFade, loop);
//        }
//        else {
//            Play(audioClip, delayFade, loop);
//        }
//    }

//    /// <summary>
//    /// Unpauses given clip if it's available. If not - plays new instance.
//    /// </summary>
//    public void PlayOrUnPause(string audioClipName, float delayFade = 0f, bool loop = false) {
//        if (IsSoundPaused(audioClipName)) {
//            UnPause(audioClipName, delayFade, loop);
//        }
//        else {
//            Play(audioClipName, delayFade, loop);
//        }
//    }
//    #endregion

//    #region Play if not playing
//    /// <summary>
//    /// Plays given clip if it's not playing now.
//    /// </summary>
//    public void PlayIfNotPlaying(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
//        if (!IsSoundPlaying(audioClip)) {
//            Play(audioClip, delayFade, loop);
//        }
//    }

//    /// <summary>
//    /// Plays given clip if it's not playing now.
//    /// </summary>
//    public void PlayIfNotPlaying(string audioClipName, float delayFade = 0f, bool loop = false) {
//        if (!IsSoundPlaying(audioClipName)) {
//            Play(audioClipName, delayFade, loop);
//        }
//    }
//    #endregion
//    #endregion

//    #region Pause
//    /// <summary>
//    /// Pauses all sources.
//    /// </summary>
//    public void Pause(float delayFade = 0f) {
//        int count = _audioSourcesList.Count;
//        for (int i = 0; i < count; i++) {
//            PauseSource(_audioSourcesList[i], delayFade);
//        }
//    }

//    /// <summary>
//    /// Returns true if source with given clip was found and then pauses it.
//    /// </summary>
//    public bool Pause(AudioClip audioClip, float delayFade = 0f) {
//        var source = FindSource(audioClip);
//        if (!source) {
//            return false;
//        }
//        PauseSource(source, delayFade);
//        return true;
//    }

//    /// <summary>
//    /// Returns true if source with given clip name was found and then pauses it.
//    /// </summary>
//    public bool Pause(string audioClipName, float delayFade = 0f) {
//        var source = FindSource(audioClipName);
//        if (!source) {
//            return false;
//        }
//        PauseSource(source, delayFade);
//        return true;
//    }
//    #endregion

//    #region Unpause
//    /// <summary>
//    /// Unpauses all sources.
//    /// </summary>
//    public void UnPause(float delayFade = 0f, bool loop = false) {
//        int sourcesCount = _audioSourcesList.Count;
//        for (int i = 0; i < sourcesCount; i++) {
//            UnPauseSource(_audioSourcesList[i], delayFade, loop);
//        }
//    }

//    /// <summary>
//    /// Returns true if source with given clip was found and then unpauses it.
//    /// </summary>
//    public bool UnPause(AudioClip clip, float delayFade = 0f, bool loop = false) {
//        var source = FindSource(clip);
//        if (!source) {
//            return false;
//        }
//        UnPauseSource(source, delayFade, loop);
//        return true;
//    }

//    /// <summary>
//    /// Returns true if source with given clip name was found and then unpauses it.
//    /// </summary>
//    public bool UnPause(string audioClipName, float delayFade = 0f, bool loop = false) {
//        var source = FindSource(audioClipName);
//        if (!source) {
//            return false;
//        }
//        UnPauseSource(source, delayFade, loop);
//        return true;
//    }
//    #endregion

//    #region Stop
//    /// <summary>
//    /// Stops all sources.
//    /// </summary>
//    public void Stop(float delayFade = 0f) {
//        int count = _audioSourcesList.Count;
//        for (int i = 0; i < count; i++) {
//            StopSource(_audioSourcesList[i], delayFade);
//        }
//    }

//    /// <summary>
//    /// Returns true if source with given clip was found and stopped.
//    /// </summary>
//    public bool Stop(AudioClip clip, float delayFade = 0f) {
//        var source = FindSource(clip);
//        if (!source) {
//            return false;
//        }
//        StopSource(source, delayFade);
//        return true;
//    }

//    /// <summary>
//    /// Returns true if source with given clip name was found and stopped.
//    /// </summary>
//    public bool Stop(string audioClipName, float delayFade = 0f) {
//        var source = FindSource(audioClipName);
//        if (!source) {
//            return false;
//        }
//        StopSource(source, delayFade);
//        return true;
//    }
//    #endregion

//    #region Audio state
//    /// <summary>
//    /// Returns true if sound is playing.
//    /// </summary>
//    public bool IsSoundPlaying(AudioClip audioClip) {
//        var source = FindSource(audioClip);
//        if (source) {
//            return source.isPlaying;
//        }
//        return false;
//    }

//    /// <summary>
//    /// Returns true if sound is playing.
//    /// </summary>
//    public bool IsSoundPlaying(string audioClipName) {
//        var source = FindSource(audioClipName);
//        if (source) {
//            return source.isPlaying;
//        }
//        return false;
//    }

//    /// <summary>
//    /// Returns true if any sound is playing.
//    /// </summary>
//    public bool IsSoundPlaying() {
//        int sourcesCount = _audioSourcesList.Count;
//        for (int i = 0; i < sourcesCount; i++) {
//            if (_audioSourcesList[i].isPlaying) {
//                return true;
//            }
//        }
//        return false;
//    }

//    /// <summary>
//    /// Returns true if sound is paused.
//    /// </summary>
//    public bool IsSoundPaused(AudioClip audioClip) {
//        var source = FindSource(audioClip);
//        if (source) {
//            return _pausedSourcesList.Contains(source);
//        }
//        return false;
//    }

//    /// <summary>
//    /// Returns true if sound is paused.
//    /// </summary>
//    public bool IsSoundPaused(string audioClipName) {
//        var source = FindSource(audioClipName);
//        if (source) {
//            return _pausedSourcesList.Contains(source);
//        }
//        return false;
//    }
//    #endregion

//    #region Volume control
//    /// <summary>
//    /// Set volume for all sources.
//    /// </summary>
//    public void SetVolume(float volume, float delayFade = 0f) {
//        int sourcesCount = _audioSourcesList.Count;
//        for (int i = 0; i < sourcesCount; i++) {
//            SetVolumeSource(_audioSourcesList[i], volume, delayFade);
//        }
//    }

//    /// <summary>
//    /// Sets volume for given clip.
//    /// </summary>
//    public Tween SetVolume(AudioClip audioClip, float volume, float delayFade = 0f) {
//        return SetVolumeSource(FindSource(audioClip), volume, delayFade);
//    }

//    /// <summary>
//    /// Sets volume for given clip.
//    /// </summary>
//    public Tween SetVolume(string audioClipName, float volume, float delayFade = 0f) {
//        return SetVolumeSource(FindSource(audioClipName), volume, delayFade);

//    }

//    /// <summary>
//    /// Returns sound source volume.
//    /// </summary>
//    public float GetSourceVolume(AudioClip audioClip) {
//        var source = FindSource(audioClip);
//        if (source) {
//            return source.volume;
//        }
//        return 0f;
//    }

//    /// <summary>
//    /// Returns sound source volume.
//    /// </summary>
//    public float GetSourceVolume(string audioClipName) {
//        var source = FindSource(audioClipName);
//        if (source) {
//            return source.volume;
//        }
//        return 0f;
//    }
//    #endregion
//    #endregion
//}