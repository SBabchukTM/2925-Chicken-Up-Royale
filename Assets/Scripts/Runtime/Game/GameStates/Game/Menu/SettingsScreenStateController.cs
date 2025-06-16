using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class SettingsScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        private SettingsScreen _screen;
        
        private float _soundVolume;
        private float _musicVolume;

        public SettingsScreenStateController(ILogger logger, IUiService uiService, UserDataService userDataService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();

            _soundVolume = _userDataService.GetUserData().SettingsData.SoundVolume;
            _musicVolume = _userDataService.GetUserData().SettingsData.MusicVolume;
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.SettingsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SettingsScreen>(ConstScreens.SettingsScreen);
            _screen.Initialize(_userDataService.GetUserData().SettingsData);
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () =>
            {
                ResetSettings();
                await GoTo<MenuStateController>();
            };
            
            
            _screen.OnSavePressed += async () => await GoTo<MenuStateController>();
            
            _screen.OnMusicVolumeChanged += UpdateMusicVolume;
            _screen.OnSoundVolumeChanged += UpdateSoundVolume;
            _screen.OnPrivacyPressed += async () => await _uiService.ShowPopup(ConstPopups.PrivacyPolicyPopup);
            _screen.OnTouPressed += async () => await _uiService.ShowPopup(ConstPopups.TermsOfUsePopup);
        }

        private void UpdateSoundVolume(float volume)
        {
            _audioService.SetVolume(AudioType.Sound, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.SoundVolume = volume;
        }

        private void UpdateMusicVolume(float volume)
        {
            _audioService.SetVolume(AudioType.Music, volume);
            var userData = _userDataService.GetUserData();
            userData.SettingsData.MusicVolume = volume;
        }

        private void ResetSettings()
        {
            _audioService.SetVolume(AudioType.Music, _musicVolume);
            _audioService.SetVolume(AudioType.Sound, _soundVolume);
            
            var userData = _userDataService.GetUserData();
            userData.SettingsData.MusicVolume = _musicVolume;
            userData.SettingsData.SoundVolume = _soundVolume;
        }
    }
}