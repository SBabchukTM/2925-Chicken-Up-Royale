using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.UI.Popup;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Services.UI;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

public class BathEndPopupStateController : StateController
{
    private readonly IUiService _uiService;
    
    public BathEndPopupStateController(ILogger logger, IUiService uiService) : base(logger)
    {
        _uiService = uiService;
    }
    
    public override async UniTask Enter(CancellationToken cancellationToken = default)
    {
        Time.timeScale = 0;
        BathEndPopup popup = await _uiService.ShowPopup(ConstPopups.BathEndPopup) as BathEndPopup;

        popup.OnClicked += async () =>
        {
            Time.timeScale = 1;
            popup.DestroyPopup();
            await GoTo<CareScreenStateController>();
        };
    }
}
