using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game
{
    public class DoodleLosePopupState : StateController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        
        public DoodleLosePopupState(ILogger logger, IUiService uiService, UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            
            DoodleLosePopup popup = await _uiService.ShowPopup(ConstPopups.DoodleLosePopup) as DoodleLosePopup;

            popup.SetBestTime(_userDataService.GetUserData().UserDoodleClearTime.BestTime);
            
            popup.OnPressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();
                await GoTo<CareScreenStateController>();
            };
        }
    }
}