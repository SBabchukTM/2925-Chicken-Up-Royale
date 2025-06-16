using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using Runtime.Game.UserAccountSystem;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class AccountScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserAccountService _userAccountService;
        private readonly AvatarSelectionService _avatarSelectionService;

        private AccountScreen _screen;

        private UserAccountData _modifiedData;

        public AccountScreenStateController(ILogger logger, IUiService uiService,
            UserAccountService userAccountService,
            AvatarSelectionService avatarSelectionService) : base(logger)
        {
            _uiService = uiService;
            _userAccountService = userAccountService;
            _avatarSelectionService = avatarSelectionService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CopyData();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.AccountScreen);
        }

        private void CopyData() => _modifiedData = _userAccountService.GetAccountDataCopy();
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<AccountScreen>(ConstScreens.AccountScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.SetData(_modifiedData);
            _screen.SetAvatar(_userAccountService.GetUsedAvatarSprite());
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += GoToMenu;
            _screen.OnSavePressed += SaveAndLeave;
            _screen.OnChangeAvatarPressed += SelectNewAvatar;
            _screen.OnNameChanged += ValidateName;
        }

        private async void GoToMenu() => await GoTo<MenuStateController>();

        private void SaveAndLeave()
        {
            _userAccountService.SaveAccountData(_modifiedData);
            GoToMenu();
        }

        private async void SelectNewAvatar()
        {
            Sprite newAvatar = await _avatarSelectionService.PickImage(512, CancellationToken.None);

            if (newAvatar != null)
            {
                _modifiedData.AvatarBase64 = _userAccountService.ConvertToBase64(newAvatar);
                _screen.SetAvatar(newAvatar);
            }
        }

        private void ValidateName(string value)
        {
            if (!IsNameValid(value))
                _screen.SetData(_modifiedData);
            else
                _modifiedData.Username = value;
        }

        private bool IsNameValid(string value)
        {
            return value.Length > 2 || Char.IsLetter(value[0]);
        }
    }
}