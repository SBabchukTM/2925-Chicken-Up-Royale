using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class ShopScreenStateController : StateController
    {
        private readonly IUiService _uiService;

        private ShopScreen _screen;

        public PrevScreen _prevScreen;

        public ShopScreenStateController(ILogger logger, IUiService uiService) : base(logger)
        {
            _uiService = uiService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.ShopScreen);
        }

        public void SetPreviousScreen(PrevScreen previousScreen) => _prevScreen = previousScreen;
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<ShopScreen>(ConstScreens.ShopScreen);
            _screen.Initialize();
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
    
    public enum PrevScreen
    {
        Menu,
        Care
    }
}