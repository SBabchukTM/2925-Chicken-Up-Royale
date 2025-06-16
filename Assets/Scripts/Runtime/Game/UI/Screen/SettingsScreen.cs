using System;
using Runtime.Game.Services.UserData.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Game.UI.Screen
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Button _privacyButton;
        [SerializeField] private Button _touButton;
        
        public event Action OnBackPressed;
        public event Action OnSavePressed;
        public event Action<float> OnSoundVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        
        public event Action OnPrivacyPressed;
        public event Action OnTouPressed;

        public void Initialize(SettingsData settings)
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _saveButton.onClick.AddListener(() => OnSavePressed?.Invoke());

            _soundSlider.value = settings.SoundVolume;
            _musicSlider.value = settings.MusicVolume;
            
            _soundSlider.onValueChanged.AddListener((value) => OnSoundVolumeChanged?.Invoke(value));
            _musicSlider.onValueChanged.AddListener((value) => OnMusicVolumeChanged?.Invoke(value));
            
            _privacyButton.onClick.AddListener(() => OnPrivacyPressed?.Invoke());
            _touButton.onClick.AddListener(() => OnTouPressed?.Invoke());
        }
    }
}