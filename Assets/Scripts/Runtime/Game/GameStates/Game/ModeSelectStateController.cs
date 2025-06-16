using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.UI;

namespace Runtime.Game.GameStates.Game
{
    public class ModeSelectStateController : StateController
    {
        private readonly IUiService _uiService;
        
        public ModeSelectStateController(ILogger logger, IUiService uiService) : base(logger)
        {
            _uiService = uiService;
        }

        public override async UniTask Enter(CancellationToken cancellationToken = default)
        {
            ModeSelectionPopup popup = await _uiService.ShowPopup(ConstPopups.ModeSelectionPopup) as ModeSelectionPopup;

            popup.OnIncubatorButtonPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<IncubationScreenStateController>();
            };

            popup.OnVisitButtonPressed += async () =>
            {
                popup.DestroyPopup();
                await GoTo<CareScreenStateController>();
            };

            popup.OnCloseButtonPressed += popup.DestroyPopup;
        }
    }
}