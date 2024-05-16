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
    public class SettingsManager : SingletonComponent<SettingsManager>, ISaveable
    {
        public string SaveId => "Settings_Manager";

        [Header("References")]
        public VolumeProfile globalVolume;
        [Header("Default Values")]
        public float movementSensitivty = 0.5f;
        [Space]
        public bool isFullscreen = true;
        [Space]
        public float masterVolume = 1;
        public float musicVolume = 1;
        public float sfxVolume = 1;
        public float lightIntensity = 0;
        [Header("Settings UI")]
        public Slider movementSensitivitySlider;
        [Space]
        public Toggle fullscreenToggle;
        public Slider lightIntensitySlider;
        [Space]
        public Slider masterVolumeSlider;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;


        public Dictionary<string, object> Save()
        {
            Dictionary<string, object> saveData = new Dictionary<string, object>
            {
                { "movementSensitivty", movementSensitivty },
                { "isFullscreen", isFullscreen },
                { "lightIntensity", lightIntensity },
                { "masterVolume", masterVolume },
                { "musicVolume", musicVolume },
                { "sfxVolume", sfxVolume }
            };

            return saveData;
        }

        public bool Load()
        {
            JSONNode saveData = SaveManager.Instance.LoadSave(this);

            if (saveData == null)
            {
                return false;
            }

            movementSensitivty = (float) saveData["movementSensitivty"];
            isFullscreen = (bool)saveData["isFullscreen"];
            lightIntensity = (float)saveData["lightIntensity"];
            masterVolume = (float)saveData["masterVolume"];
            musicVolume = (float)saveData["musicVolume"];
            sfxVolume = (float)saveData["sfxVolume"];

            return true;
        }

        protected override void Awake()
        {
            base.Awake();
            SaveManager.Instance.Register(this);


        }

        private void Start()
        {
            if (!Load())
            {
                // Add default settings here. We are using Set methods because some of them may contain additional logic.
                SetMovementSensitivity(0.5f);

                SetFullScreen(true);
                SetLightIntensity(0);

                SetMasterVolume(1);
                SetMusicVolume(1);
                SetSFXVolume(1);
            }

            // Set UI with the default settings.
            movementSensitivitySlider.value = movementSensitivty;

            fullscreenToggle.isOn = isFullscreen;
            lightIntensitySlider.value = lightIntensity;

            masterVolumeSlider.value = masterVolume;
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;
        }

        private void OnEnable()
        {
            // Add listeners to the UI elements.
            movementSensitivitySlider.onValueChanged.AddListener(SetMovementSensitivity);

            fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
            lightIntensitySlider.onValueChanged.AddListener(SetLightIntensity);

            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        private void OnDisable()
        {
            // Remove listeners to avoid errors.
            movementSensitivitySlider.onValueChanged.RemoveAllListeners();

            fullscreenToggle.onValueChanged.RemoveAllListeners();
            lightIntensitySlider.onValueChanged.RemoveAllListeners();

            masterVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        }

        #region Unity Editor Methods
        public void SetMovementSensitivity(float value)
        {
            movementSensitivty = value;
        }
        public void SetFullScreen(bool value)
        {
            isFullscreen = value;
            Screen.fullScreen = isFullscreen;
        }
        public void SetMasterVolume(float value)
        {
            masterVolume = value;
            SoundManager.Instance.playingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = masterVolume;
            });
            SoundManager.Instance.loopingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = masterVolume;
            });
        }
        public void SetMusicVolume(float value)
        {
            musicVolume = value;
            SoundManager.Instance.loopingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = musicVolume;
            });
        }
        public void SetSFXVolume(float value)
        {
            sfxVolume = value;
            SoundManager.Instance.playingAudioSources.ForEach(p =>
            {
                p.audioSource.volume = sfxVolume;
            });
        }
        public void SetLightIntensity(float value)
        {
            lightIntensity = value;
            if(globalVolume.TryGet(out Bloom bloom))
            {
                bloom.intensity.value = 3 + lightIntensity;
            }
        }
        #endregion
    }

}
