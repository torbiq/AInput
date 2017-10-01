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
        private Image _buttonMusicImage;
        [SerializeField]
        private Image _buttonSoundImage;

        [SerializeField]
        private Sprite _musicOnSprite;
        [SerializeField]
        private Sprite _musicOffSprite;

        [SerializeField]
        private Sprite _soundOnSprite;
        [SerializeField]
        private Sprite _soundOffSprite;

        private void Awake() {
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            _soundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, 1f);

            _musicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1;
            _soundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1;

            UpdateButtons();
        }

        private void UpdateButtons() {
            UpdateSoundButton();
            UpdateMusicButton();
        }
        private void UpdateSoundButton() {
            _buttonSoundImage.sprite = _soundEnabled ? _soundOnSprite : _soundOffSprite;
        }
        private void UpdateMusicButton() {
            _buttonMusicImage.sprite = _musicEnabled ? _musicOnSprite : _musicOffSprite;
        }

        private static readonly string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
        private static readonly string SOUND_VOLUME_KEY = "SOUND_VOLUME";
        private static readonly string MUSIC_ENABLED_KEY = "MUSIC_ENABLED";
        private static readonly string SOUND_ENABLED_KEY = "SOUND_ENABLED";

        public bool musicEnabled { get { return _musicEnabled; } set { _musicEnabled = value; UpdateMusicButton(); } }
        public bool soundEnabled { get { return _soundEnabled; } set { _soundEnabled = value; UpdateSoundButton(); } }

        public float musicVolume { get { return _musicEnabled ? _musicVolume : 0f; } set { _musicVolume = value; } }
        public float soundVolume { get { return _soundEnabled ? _soundVolume : 0f; } set { _soundVolume = value; } }

        public void SwitchSoundEnabled() {
            soundEnabled = !_soundEnabled;
        }
        public void SwitchMusicEnabled() {
            musicEnabled = !_musicEnabled;
        }

        private void OnApplicationQuit() {
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME_KEY, _soundVolume);
            PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, _musicEnabled ? 1 : 0);
            PlayerPrefs.SetInt(SOUND_ENABLED_KEY, _soundEnabled ? 1 : 0);

            PlayerPrefs.Save();
        }
    }
}
