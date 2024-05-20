using Dogabeey.SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Dogabeey
{
    public class SettingsManager : SingletonComponent<SettingsManager>
    {
        [Header("References")]
        public VolumeProfile globalVolume;
        [Header("Default Values")]
        public float musicVolume = 1;
        public float sfxVolume = 1;
        public bool vibration = true;
        [Header("Settings UI")]
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;
        public Toggle vibrationToggle;

        public float MusicVolume
        {
            get => PlayerPrefs.GetFloat("MusicVolume", musicVolume);
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
            }
        }
        public float SfxVolume
        {
            get => PlayerPrefs.GetFloat("SfxVolume", sfxVolume);
            set
            {
                PlayerPrefs.SetFloat("SfxVolume", value);
            }
        }
        public bool Vibration
        {
            get => PlayerPrefs.GetInt("Vibration", vibration ? 1 : 0) == 1;
            set
            {
                PlayerPrefs.SetInt("Vibration", value ? 1 : 0);
            }
        }

        protected override void Awake()
        {
            base.Awake();

        }

        private void Start()
        {
            // Add default settings here. We are using Set methods because some of them may contain additional logic.
            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);
            SetVibration(vibration);

            // Set UI with the default settings.
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;
            vibrationToggle.isOn = vibration;
        }

        private void OnEnable()
        {
            // Add listeners to the UI elements.
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            vibrationToggle.onValueChanged.AddListener(SetVibration);
        }
        private void OnDisable()
        {
            // Remove listeners to avoid errors.
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
            vibrationToggle.onValueChanged.RemoveAllListeners();
        }

        #region Unity Editor Methods
        public void SetMusicVolume(float value)
        {
            MusicVolume = value;
            SoundManager.Instance.loopingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = musicVolume;
            });
        }
        public void SetSFXVolume(float value)
        {
            SfxVolume = value;
            SoundManager.Instance.playingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = sfxVolume;
            });
        }
        public void SetVibration(bool value)
        {
            Vibration = value;
        }
        #endregion
    }

}
