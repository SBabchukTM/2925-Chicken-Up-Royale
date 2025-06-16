using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class MenuStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly ShopScreenStateController _shopScreen;
        private readonly DailyScreenStateController _dailyScreen;
        private readonly ModeSelectStateController _modeSelect;

        private MenuScreen _screen;

        public MenuStateController(ILogger logger, IUiService uiService, ShopScreenStateController screenStateController,
            DailyScreenStateController dailyScreenStateController, ModeSelectStateController modeSelectStateController) : base(logger)
        {
            _uiService = uiService;
            _shopScreen = screenStateController;
            _dailyScreen = dailyScreenStateController;
            _modeSelect = modeSelectStateController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnDailyPressed += async () =>
            {
                _dailyScreen.SetPreviousScreen(PrevScreen.Menu);
                await GoTo<DailyScreenStateController>();
            };
            _screen.OnProfilePressed += async () => await GoTo<AccountScreenStateController>();
            _screen.OnSettingsPressed += async () => await GoTo<SettingsScreenStateController>();
            _screen.OnShopPressed += async () =>
            {
                _shopScreen.SetPreviousScreen(PrevScreen.Menu);
                await GoTo<ShopScreenStateController>();
            };
            _screen.OnHowToPlayPressed += async () => await _uiService.ShowPopup(ConstPopups.HowToPlayPopup);
            _screen.OnPlayPressed += async () =>
            {
                _modeSelect.Enter().Forget();
            };
            _screen.OnAchievementsPressed += async () => await GoTo<AchievementsScreenStateController>();
            _screen.OnMarketPressed += async () => await GoTo<MarketScreenStateController>();
        }
    }
}