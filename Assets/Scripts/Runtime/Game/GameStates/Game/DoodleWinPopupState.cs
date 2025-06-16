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
    public class DoodleWinPopupState : StateController
    {
        private readonly IUiService _uiService;
        private readonly DoodleGameData _doodleGameData;
        private readonly UserDataService _userDataService;
        
        public DoodleWinPopupState(ILogger logger, IUiService uiService, DoodleGameData data, UserDataService userDataService) : base(logger)
        {
            _uiService = uiService;
            _doodleGameData = data;
            _userDataService = userDataService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            
            DoodleWinPopup popup = await _uiService.ShowPopup(ConstPopups.DoodleWinPopup) as DoodleWinPopup;

            popup.SetData(_userDataService.GetUserData().UserDoodleClearTime.BestTime, _doodleGameData.Coins);
            
            popup.OnPressed += async () =>
            {
                Time.timeScale = 1;
                popup.DestroyPopup();
                await GoTo<CareScreenStateController>();
            };
        }
    }
}