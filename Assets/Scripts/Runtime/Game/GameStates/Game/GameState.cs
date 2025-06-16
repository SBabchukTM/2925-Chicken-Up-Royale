using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.GameStateMachine;
using Runtime.Game.GameStates.Game.Controllers;
using Runtime.Game.GameStates.Game.Menu;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game
{
    public class GameState : StateController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateController _menuStateController;
        private readonly AccountScreenStateController _accountScreenStateController;
        private readonly UserDataStateChangeController _userDataStateChangeController;
        private readonly AchievementsScreenStateController _achievementsScreenStateController;
        private readonly BathGameScreenStateController _bathGameScreenStateController;
        private readonly CareScreenStateController _careScreenStateController;
        private readonly DailyScreenStateController _dailyScreenStateController;
        private readonly DoodleJumpScreenStateController _doodleJumpScreenStateController;
        private readonly FoodGameScreenStateController _foodGameScreenStateController;
        private readonly IncubationScreenStateController _incubationScreenStateController;
        private readonly InventoryScreenStateController _inventoryScreenStateController;
        private readonly MarketScreenStateController _marketScreenStateController;
        private readonly SettingsScreenStateController _settingsScreenStateController;
        private readonly ShopScreenStateController _shopScreenStateController;
        private readonly DoodleLosePopupState _doodleLosePopupState;
        private readonly DoodleWinPopupState _doodleWinPopupState;
        private readonly ModeSelectStateController _modeSelectStateController;
        private readonly FoodEndPopupStateController _foodEndPopupStateController;
        private readonly BathEndPopupStateController _bathEndPopupStateController;

        public GameState(ILogger logger,
            MenuStateController menuStateController,
            AccountScreenStateController accountScreenStateController,
            StateMachine stateMachine,
            UserDataStateChangeController userDataStateChangeController,
            AchievementsScreenStateController achievementsScreenStateController,
            BathGameScreenStateController bathGameScreenStateController,
            CareScreenStateController careScreenStateController,
            DailyScreenStateController dailyScreenStateController,
            DoodleJumpScreenStateController doodleJumpScreenStateController,
            FoodGameScreenStateController foodGameScreenStateController,
            IncubationScreenStateController incubationScreenStateController,
            InventoryScreenStateController inventoryScreenStateController,
            MarketScreenStateController marketScreenStateController,
            SettingsScreenStateController settingsScreenStateController,
            ShopScreenStateController shopScreenStateController,
            DoodleLosePopupState doodleLosePopupState,
            DoodleWinPopupState doodleWinPopupState,
            ModeSelectStateController modeSelectStateController,
            FoodEndPopupStateController foodEndPopupStateController,
            BathEndPopupStateController bathEndPopupStateController) : base(logger)
        {
            _stateMachine = stateMachine;
            _menuStateController = menuStateController;
            _accountScreenStateController = accountScreenStateController;
            _userDataStateChangeController = userDataStateChangeController;
            _achievementsScreenStateController = achievementsScreenStateController;
            _bathGameScreenStateController = bathGameScreenStateController;
            _careScreenStateController = careScreenStateController;
            _dailyScreenStateController = dailyScreenStateController;
            _doodleJumpScreenStateController = doodleJumpScreenStateController;
            _foodGameScreenStateController = foodGameScreenStateController;
            _incubationScreenStateController = incubationScreenStateController;
            _inventoryScreenStateController = inventoryScreenStateController;
            _marketScreenStateController = marketScreenStateController;
            _settingsScreenStateController = settingsScreenStateController;
            _shopScreenStateController = shopScreenStateController;
            _doodleLosePopupState = doodleLosePopupState;
            _doodleWinPopupState = doodleWinPopupState;
            _modeSelectStateController = modeSelectStateController;
            _foodEndPopupStateController = foodEndPopupStateController;
            _bathEndPopupStateController = bathEndPopupStateController;
        }

        public override async UniTask Enter(CancellationToken cancellationToken)
        {
            await _userDataStateChangeController.Run(default);

            _stateMachine.Initialize(_menuStateController, _accountScreenStateController, _achievementsScreenStateController, 
                _bathGameScreenStateController, _careScreenStateController, _dailyScreenStateController, 
                _doodleJumpScreenStateController, _foodGameScreenStateController, _incubationScreenStateController, 
                _inventoryScreenStateController, _marketScreenStateController, _settingsScreenStateController, 
                _shopScreenStateController, _doodleLosePopupState, _doodleWinPopupState, _modeSelectStateController,
                _foodEndPopupStateController, _bathEndPopupStateController);
            
            _stateMachine.GoTo<MenuStateController>().Forget();
        }
    }
}