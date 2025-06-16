using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class DailyScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly DailyRewardsFactory _dailyRewardsFactory;

        private DailyBonusScreen _screen;

        private PrevScreen _prevScreen;
        
        public DailyScreenStateController(ILogger logger, IUiService uiService, DailyRewardsFactory dailyRewardsFactory) : base(logger)
        {
            _uiService = uiService;
            _dailyRewardsFactory = dailyRewardsFactory;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            return UniTask.CompletedTask;
        }

        public void SetPreviousScreen(PrevScreen previousScreen) => _prevScreen = previousScreen;
        
        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.DailyBonusScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<DailyBonusScreen>(ConstScreens.DailyBonusScreen);
            _screen.Initialize(_dailyRewardsFactory.CreateDailyRewards());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () =>
            {
                if(_prevScreen == PrevScreen.Menu)
                    await GoTo<MenuStateController>();
                else
                    await GoTo<CareScreenStateController>();
            };
        }
    }
}