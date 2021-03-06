﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace BitcoinMiner {
    public class AudioController : MonoBehaviour {
        [SerializeField]
        private GameSettings _gameSettings;

        private List<AudioSource> _audioSourcesList;
        private List<AudioSource> _pausedSourcesList;
        private bool _isMusicPaused;
        
        private Dictionary<string, AudioClip> _clipsDict;
        
        public Dictionary<string, AudioClip> clipsDict {
            get { return _clipsDict; }
        }

        private GameObject _audioContainerGO;
        private GameObject _musicContainerGO;

        private AudioSource _musicSource;

        private float _musicVolume = 1;

        #region Singletone implementation
        void Init() {
            Instance = this;

            OnLevelLoaded(default(Scene), LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnLevelLoaded;

            _musicContainerGO = gameObject;
            _musicContainerGO.name = "AudioPlayer_Music";
            
            _clipsDict = new Dictionary<string, AudioClip>();
            LoadNonLangAudio();

            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicVolume = _gameSettings.musicVolume;
            _musicSource.volume = _musicVolume;
        }
        
        public static AudioController Instance { get; private set; }
        #endregion

        #region Mono Behaviour
        private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            var audioContainer = new GameObject();
            _audioContainerGO = audioContainer;
            _audioSourcesList = new List<AudioSource>();
            _pausedSourcesList = new List<AudioSource>();
            _audioContainerGO.name = "AudioPlayer_Sounds";
        }
        private void Awake() {
            if (Instance) {
                if (Instance != this) {
                    DestroyImmediate(gameObject);
                    return;
                }
            }
            DontDestroyOnLoad(gameObject);
            Init();
        }
        private void Update() {
            bool soundEnabled = true;
            bool musicEnabled = true;
            _musicVolume = 1f;

            if (_musicSource.mute == musicEnabled) {
                _musicSource.mute = !musicEnabled;
            }

            for (int i = 0; i < _audioSourcesList.Count; i++) {
                var source = _audioSourcesList[i];

                // If sound was completed and is not not paused.
                if ((source.time == 0f && !source.isPlaying) // Wasn't completed.
                    && !_pausedSourcesList.Contains(source)) // Isn't paused.
                {
                    RemoveSource(source);
                    continue;
                }

                // If sound is not muted when sound setting inactive.
                if (source.mute == soundEnabled) {
                    source.mute = !soundEnabled;
                }
            }
        }
        #endregion

        #region Incapsulated sources control methods
        private AudioSource CreateSource() {
            var audioSource = _audioContainerGO.AddComponent<AudioSource>();
            _audioSourcesList.Add(audioSource);
            return audioSource;
        }
        private bool RemoveSource(AudioSource audioSource) {
            _pausedSourcesList.Remove(audioSource);
            DestroyObject(audioSource);
            return _audioSourcesList.Remove(audioSource);
        }
        private AudioSource FindSource(string audioClipName) {
            int sourcesCount = _audioSourcesList.Count;
            for (int i = 0; i < sourcesCount; i++) {
                if (_audioSourcesList[i].clip) {
                    if (_audioSourcesList[i].clip.name == audioClipName) {
                        return _audioSourcesList[i];
                    }
                }
            }
            return null;
        }
        private AudioSource FindSource(AudioClip clip) {
            int sourcesCount = _audioSourcesList.Count;
            for (int i = 0; i < sourcesCount; i++) {
                if (_audioSourcesList[i].clip == clip) {
                    return _audioSourcesList[i];
                }
            }
            return null;
        }
        private Tween SetVolumeSource(AudioSource audioSource, float volume, float delayFade) {
            if (!audioSource) {
                Debug.LogError("No clip received");
                return null;
            }
            if (delayFade == 0f) {
                audioSource.volume = volume;
            }
            audioSource.DOKill(true);
            return audioSource.DOFade(volume, delayFade);
        }
        private float PlaySource(AudioClip audioClip, AudioSource audioSource, float delayFade, float volume, bool loop) {
            if (!audioClip) {
                Debug.LogError("No clip received");
                return 0;
            }
            audioSource.loop = loop;
            audioSource.clip = audioClip;
            audioSource.Play();
            SetVolumeSource(audioSource, volume, delayFade);
            return audioClip.length;
        }
        private void PauseSource(AudioSource audioSource, float delayFade) {
            _pausedSourcesList.Add(audioSource);
            SetVolumeSource(audioSource, 0f, delayFade).OnComplete(audioSource.Pause);
        }
        private void UnPauseSource(AudioSource audioSource, float delayFade, bool loop) {
            _pausedSourcesList.Remove(audioSource);
            audioSource.UnPause();
            SetVolumeSource(audioSource, _gameSettings.soundVolume, delayFade);
        }
        private void StopSource(AudioSource audioSource, float delayFade) {
            SetVolumeSource(audioSource, 0f, delayFade).OnComplete(delegate {
                RemoveSource(audioSource);
            });
        }
        #endregion

        #region Prototype load sounds replacement
        /// <summary>
        /// Gets clip from non-language dictionary.
        /// </summary>
        public AudioClip GetClip(string audioClipName) {
            AudioClip audioClip = null;
            _clipsDict.TryGetValue(audioClipName, out audioClip);
            return audioClip;
        }
        /// <summary>
        /// Loads non-language audio clips.
        /// </summary>
        private void LoadNonLangAudio() {
            _clipsDict = new Dictionary<string, AudioClip>();
            AudioClip[] nonLangClips = Resources.LoadAll<AudioClip>("Sounds");
            int length = nonLangClips.Length;
            for (int i = 0; i < length; i++) {
                try {
                    _clipsDict.Add(nonLangClips[i].name, nonLangClips[i]);
                }
                catch (System.Exception exception) {
                    Debug.LogError(exception);
                    Debug.LogError("Key [" + nonLangClips[i].name + "] is probaubly already contained by non-lang audio dictionary.");
                }
            }
        }
        #endregion

        #region Music control methods
        #region Play
        #region Play default
        /// <summary>
        /// Fades current music and plays given clip.
        /// </summary>
        public void PlayMusic(AudioClip audioClip, float delayFade = 0f) {
            _isMusicPaused = false;
            AudioSource currentMusicSource = _musicSource;
            StopMusic(delayFade);
            DOVirtual.DelayedCall(delayFade, delegate {
                Destroy(currentMusicSource);
            }, false);
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.volume = 0f;
            PlaySource(audioClip, _musicSource, delayFade, _musicVolume, true);
        }

        /// <summary>
        /// Fades current music and plays given clip by name.
        /// </summary>
        public void PlayMusic(string audioClipName, float delayFade = 0f) {
            _isMusicPaused = false;
            AudioSource currentMusicSource = _musicSource;
            StopMusic(delayFade);
            DOVirtual.DelayedCall(delayFade, delegate {
                Destroy(currentMusicSource);
            }, false);
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.volume = 0f;
            PlaySource(GetClip(audioClipName), _musicSource, delayFade, _musicVolume, true);
        }
        #endregion

        #region Play or unpause
        /// <summary>
        /// Play or resume given clip.
        /// </summary>
        public void PlayOrUnPauseMusic(AudioClip audioClip, float delayFade = 0f) {
            if (IsMusicClip(audioClip) && _isMusicPaused) {
                UnPauseMusic(delayFade);
            }
            else {
                PlayMusic(audioClip, delayFade);
            }
        }

        /// <summary>
        /// Play or resume given clip.
        /// </summary>
        public void PlayOrUnPauseMusic(string audioClipName, float delayFade = 0f) {
            if (IsMusicClip(audioClipName) && _isMusicPaused) {
                UnPauseMusic(delayFade);
            }
            else {
                PlayMusic(audioClipName, delayFade);
            }
        }
        #endregion

        #region Play if not playing
        /// <summary>
        /// Plays given clip if it's not playing now.
        /// </summary>
        public void PlayMusicIfNotPlaying(AudioClip audioClip, float delayFade = 0f) {
            if (!IsMusicPlaying(audioClip)) {
                PlayMusic(audioClip, delayFade);
            }
        }

        /// <summary>
        /// Plays given speech if it's not playing now.
        /// </summary>
        public float PlayMusicIfNotPlaying(string audioClipName, float delayFade = 0f) {
            if (!IsMusicPlaying(audioClipName)) {
                PlayMusic(audioClipName, delayFade);
            }
            return 0;
        }
        #endregion
        #endregion

        #region Pause
        /// <summary>
        /// Pauses music.
        /// </summary>
        public void PauseMusic(float delayFade = 0f) {
            _isMusicPaused = true;
            AudioSource currentMusicSource = _musicSource;
            SetVolumeSource(currentMusicSource, 0, delayFade).OnComplete(currentMusicSource.Pause);
        }
        #endregion

        #region Unpause
        /// <summary>
        ///Unpauses music.
        /// </summary>
        public void UnPauseMusic(float delayFade = 0f) {
            _isMusicPaused = false;
            _musicSource.UnPause();
            SetVolumeSource(_musicSource, _musicVolume, delayFade);
        }
        #endregion

        #region Stop
        /// <summary>
        /// Stops music.
        /// </summary>
        public void StopMusic(float delayFade = 0f) {
            AudioSource currentMusicSource = _musicSource;
            SetVolumeSource(currentMusicSource, 0f, delayFade).OnComplete(currentMusicSource.Stop);
        }
        #endregion

        #region Audio state
        /// <summary>
        /// Returns current music clip.
        /// </summary>
        public AudioClip GetMusicClip() {
            return _musicSource.clip;
        }

        /// <summary>
        /// Returns if current set music clip equals to given.
        /// </summary>
        public bool IsMusicClip(AudioClip audioClip) {
            return audioClip == GetMusicClip();
        }

        /// <summary>
        /// Returns if current set music clip name equals to given.
        /// </summary>
        public bool IsMusicClip(string audioClipName) {
            var currentMusicClip = GetMusicClip();
            if (currentMusicClip) {
                return currentMusicClip.name == audioClipName;
            }
            return false;
        }

        /// <summary>
        /// Is music paused?
        /// </summary>
        public bool IsMusicPaused() {
            return _isMusicPaused;
        }

        /// <summary>
        /// Is given music clip paused?
        /// </summary>
        public bool IsMusicPaused(AudioClip audioClip) {
            return IsMusicPaused() && IsMusicClip(audioClip);
        }

        /// <summary>
        /// Is given by name music clip paused?
        /// </summary>
        public bool IsMusicPaused(string audioClipName) {
            return IsMusicPaused() && IsMusicClip(audioClipName);
        }

        /// <summary>
        /// Is music playing?
        /// </summary>
        public bool IsMusicPlaying() {
            return _musicSource.isPlaying;
        }

        /// <summary>
        /// Is playing given clip?
        /// </summary>
        public bool IsMusicPlaying(AudioClip audioClip) {
            return _musicSource.isPlaying && IsMusicClip(audioClip);
        }

        /// <summary>
        /// Is playing given clip?
        /// </summary>
        public bool IsMusicPlaying(string audioClipName) {
            return _musicSource.isPlaying && IsMusicClip(audioClipName);
        }
        #endregion

        #region Volume control
        /// <summary>
        /// Set music volume.
        /// </summary>
        public Tween SetMusicVolume(float volume, float delayFade = 0f) {
            _musicVolume = volume;
            return SetVolumeSource(_musicSource, _musicVolume, delayFade);
        }

        /// <summary>
        /// Returns music volume.
        /// </summary>
        public float GetMusicVolume() {
            return _musicVolume;
        }

        /// <summary>
        /// Returns music source volume.
        /// </summary>
        public float GetMusicSourceVolume() {
            return _musicSource.volume;
        }
        #endregion
        #endregion
        
        #region Sound control methods
        #region Play
        #region Play default
        /// <summary>
        /// Returns true if received clip is valid and then plays audio.
        /// </summary>
        public float Play(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
            return PlaySource(audioClip, CreateSource(), delayFade, _gameSettings.soundVolume, loop);
        }

        /// <summary>
        /// Returns true if received clip name is valid and then plays audio.
        /// </summary>
        public float Play(string audioClipName, float delayFade = 0f, bool loop = false) {
            return PlaySource(GetClip(audioClipName), CreateSource(), delayFade, _gameSettings.soundVolume, loop);
        }
        #endregion

        #region Play or unpause
        /// <summary>
        /// Unpauses given clip if it's available. If not - plays new instance.
        /// </summary>
        public void PlayOrUnPause(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
            if (IsSoundPaused(audioClip)) {
                UnPause(audioClip, delayFade, loop);
            }
            else {
                Play(audioClip, delayFade, loop);
            }
        }

        /// <summary>
        /// Unpauses given clip if it's available. If not - plays new instance.
        /// </summary>
        public void PlayOrUnPause(string audioClipName, float delayFade = 0f, bool loop = false) {
            if (IsSoundPaused(audioClipName)) {
                UnPause(audioClipName, delayFade, loop);
            }
            else {
                Play(audioClipName, delayFade, loop);
            }
        }
        #endregion

        #region Play if not playing
        /// <summary>
        /// Plays given clip if it's not playing now.
        /// </summary>
        public void PlayIfNotPlaying(AudioClip audioClip, float delayFade = 0f, bool loop = false) {
            if (!IsSoundPlaying(audioClip)) {
                Play(audioClip, delayFade, loop);
            }
        }

        /// <summary>
        /// Plays given clip if it's not playing now.
        /// </summary>
        public void PlayIfNotPlaying(string audioClipName, float delayFade = 0f, bool loop = false) {
            if (!IsSoundPlaying(audioClipName)) {
                Play(audioClipName, delayFade, loop);
            }
        }
        #endregion
        #endregion

        #region Pause
        /// <summary>
        /// Pauses all sources.
        /// </summary>
        public void Pause(float delayFade = 0f) {
            int count = _audioSourcesList.Count;
            for (int i = 0; i < count; i++) {
                PauseSource(_audioSourcesList[i], delayFade);
            }
        }

        /// <summary>
        /// Returns true if source with given clip was found and then pauses it.
        /// </summary>
        public bool Pause(AudioClip audioClip, float delayFade = 0f) {
            var source = FindSource(audioClip);
            if (!source) {
                return false;
            }
            PauseSource(source, delayFade);
            return true;
        }

        /// <summary>
        /// Returns true if source with given clip name was found and then pauses it.
        /// </summary>
        public bool Pause(string audioClipName, float delayFade = 0f) {
            var source = FindSource(audioClipName);
            if (!source) {
                return false;
            }
            PauseSource(source, delayFade);
            return true;
        }
        #endregion

        #region Unpause
        /// <summary>
        /// Unpauses all sources.
        /// </summary>
        public void UnPause(float delayFade = 0f, bool loop = false) {
            int sourcesCount = _audioSourcesList.Count;
            for (int i = 0; i < sourcesCount; i++) {
                UnPauseSource(_audioSourcesList[i], delayFade, loop);
            }
        }

        /// <summary>
        /// Returns true if source with given clip was found and then unpauses it.
        /// </summary>
        public bool UnPause(AudioClip clip, float delayFade = 0f, bool loop = false) {
            var source = FindSource(clip);
            if (!source) {
                return false;
            }
            UnPauseSource(source, delayFade, loop);
            return true;
        }

        /// <summary>
        /// Returns true if source with given clip name was found and then unpauses it.
        /// </summary>
        public bool UnPause(string audioClipName, float delayFade = 0f, bool loop = false) {
            var source = FindSource(audioClipName);
            if (!source) {
                return false;
            }
            UnPauseSource(source, delayFade, loop);
            return true;
        }
        #endregion

        #region Stop
        /// <summary>
        /// Stops all sources.
        /// </summary>
        public void Stop(float delayFade = 0f) {
            int count = _audioSourcesList.Count;
            for (int i = 0; i < count; i++) {
                StopSource(_audioSourcesList[i], delayFade);
            }
        }

        /// <summary>
        /// Returns true if source with given clip was found and stopped.
        /// </summary>
        public bool Stop(AudioClip clip, float delayFade = 0f) {
            var source = FindSource(clip);
            if (!source) {
                return false;
            }
            StopSource(source, delayFade);
            return true;
        }

        /// <summary>
        /// Returns true if source with given clip name was found and stopped.
        /// </summary>
        public bool Stop(string audioClipName, float delayFade = 0f) {
            var source = FindSource(audioClipName);
            if (!source) {
                return false;
            }
            StopSource(source, delayFade);
            return true;
        }
        #endregion

        #region Audio state
        /// <summary>
        /// Returns true if sound is playing.
        /// </summary>
        public bool IsSoundPlaying(AudioClip audioClip) {
            var source = FindSource(audioClip);
            if (source) {
                return source.isPlaying;
            }
            return false;
        }

        /// <summary>
        /// Returns true if sound is playing.
        /// </summary>
        public bool IsSoundPlaying(string audioClipName) {
            var source = FindSource(audioClipName);
            if (source) {
                return source.isPlaying;
            }
            return false;
        }

        /// <summary>
        /// Returns true if any sound is playing.
        /// </summary>
        public bool IsSoundPlaying() {
            int sourcesCount = _audioSourcesList.Count;
            for (int i = 0; i < sourcesCount; i++) {
                if (_audioSourcesList[i].isPlaying) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if sound is paused.
        /// </summary>
        public bool IsSoundPaused(AudioClip audioClip) {
            var source = FindSource(audioClip);
            if (source) {
                return _pausedSourcesList.Contains(source);
            }
            return false;
        }

        /// <summary>
        /// Returns true if sound is paused.
        /// </summary>
        public bool IsSoundPaused(string audioClipName) {
            var source = FindSource(audioClipName);
            if (source) {
                return _pausedSourcesList.Contains(source);
            }
            return false;
        }
        #endregion

        #region Volume control
        /// <summary>
        /// Set volume for all sources.
        /// </summary>
        public void SetVolume(float volume, float delayFade = 0f) {
            int sourcesCount = _audioSourcesList.Count;
            for (int i = 0; i < sourcesCount; i++) {
                SetVolumeSource(_audioSourcesList[i], volume, delayFade);
            }
        }

        /// <summary>
        /// Sets volume for given clip.
        /// </summary>
        public Tween SetVolume(AudioClip audioClip, float volume, float delayFade = 0f) {
            return SetVolumeSource(FindSource(audioClip), volume, delayFade);
        }

        /// <summary>
        /// Sets volume for given clip.
        /// </summary>
        public Tween SetVolume(string audioClipName, float volume, float delayFade = 0f) {
            return SetVolumeSource(FindSource(audioClipName), volume, delayFade);

        }

        /// <summary>
        /// Returns sound source volume.
        /// </summary>
        public float GetSourceVolume(AudioClip audioClip) {
            var source = FindSource(audioClip);
            if (source) {
                return source.volume;
            }
            return 0f;
        }

        /// <summary>
        /// Returns sound source volume.
        /// </summary>
        public float GetSourceVolume(string audioClipName) {
            var source = FindSource(audioClipName);
            if (source) {
                return source.volume;
            }
            return 0f;
        }
        #endregion
        #endregion
    }
}