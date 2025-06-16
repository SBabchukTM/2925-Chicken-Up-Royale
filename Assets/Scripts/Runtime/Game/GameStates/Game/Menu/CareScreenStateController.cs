using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class CareScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly ShopScreenStateController _shopScreen;
        private readonly DailyScreenStateController _dailyScreen;

        private CareScreen _screen;

        public CareScreenStateController(ILogger logger, IUiService uiService, ShopScreenStateController screenStateController, DailyScreenStateController dailyScreenStateController) : base(logger)
        {
            _uiService = uiService;
            _shopScreen = screenStateController;
            _dailyScreen = dailyScreenStateController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            AchievementMediator.InvokeNewCaretaker();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.CareScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<CareScreen>(ConstScreens.CareScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
            _screen.OnDailyPressed += async () =>
            {
                _dailyScreen.SetPreviousScreen(PrevScreen.Care);
                await GoTo<DailyScreenStateController>();
            };
            _screen.OnShopPressed += async () =>
            {
                _shopScreen.SetPreviousScreen(PrevScreen.Care);
                await GoTo<ShopScreenStateController>();
            };
            _screen.OnInventoryPressed += async () => await GoTo<InventoryScreenStateController>();
            _screen.OnFoodPressed += async () => await GoTo<FoodGameScreenStateController>();
            _screen.OnBathPressed += async () => await GoTo<BathGameScreenStateController>();
            _screen.OnHappyPressed += async () => await GoTo<DoodleJumpScreenStateController>();
        }
    }
}