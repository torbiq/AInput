using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BitcoinMiner {
    public class GameSettings : MonoBehaviour {
        [SerializeField]
        [Range(0f, 1f)]
        private float _soundVolume = 1f;
        [SerializeField]
        [Range(0f, 1f)]
        private float _musicVolume = 1f;

        [SerializeField]
        private bool _soundEnabled = true;
        [SerializeField]
        private bool _musicEnabled = true;

        [SerializeField]
        private Button _buttonMusicOn;
        [SerializeField]
        private Button _buttonMusicOff;

        [SerializeField]
        private Button _buttonSoundOn;
        [SerializeField]
        private Button _buttonSoundOff;

        private void Awake() {
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            _soundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);

            _musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
            _soundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1;

            UpdateButtons();
        }

        private void UpdateButtons() {
            _buttonMusicOn.gameObject.SetActive(!_musicEnabled);
            _buttonMusicOff.gameObject.SetActive(_musicEnabled);

            _buttonSoundOn.gameObject.SetActive(!_soundEnabled);
            _buttonSoundOff.gameObject.SetActive(_soundEnabled);
        }

        private static readonly string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
        private static readonly string SOUND_VOLUME_KEY = "SOUND_VOLUME";
        private static readonly string MUSIC_ENABLED_KEY = "MUSIC_ENABLED";
        private static readonly string SOUND_ENABLED_KEY = "SOUND_ENABLED";

        public bool musicEnabled { get { return _musicEnabled; } set { _musicEnabled = value; UpdateButtons(); } }
        public bool soundEnabled { get { return _soundEnabled; } set { _soundEnabled = value; UpdateButtons(); } }

        public float musicVolume { get { return _musicVolume; } set { _musicVolume = value; } }
        public float soundVolume { get { return _soundVolume; } set { _soundVolume = value; } }
        
        private void OnApplicationQuit() {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, _soundVolume);
            PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, _musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt(SOUND_ENABLED_KEY, _soundEnabled ? 1 : 0);

            PlayerPrefs.Save();
        }
    }
}
