using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class AchievementsScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly AchievementDisplayFactory _achievementDisplayFactory;

        private AchievementsScreen _screen;

        public AchievementsScreenStateController(ILogger logger, IUiService uiService, AchievementDisplayFactory factory) : base(logger)
        {
            _uiService = uiService;
            _achievementDisplayFactory = factory;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.AchievementsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<AchievementsScreen>(ConstScreens.AchievementsScreen);
            _screen.Initialize(_achievementDisplayFactory.CreateAchievementDisplayList());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<MenuStateController>();
        }
    }
}