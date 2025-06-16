using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Care;
using Runtime.Game.DoodleJumpMiniGame;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class DoodleJumpScreenStateController : StateController
    {
        private const float HappinessAdd = 0.33f;
        
        private readonly IUiService _uiService;
        private readonly DoodleGameSetupController _doodleGameSetupController;
        private readonly DoodleGameData _doodleGameData;
        private readonly UserInventoryService _userInventoryService;
        private readonly UserDataService _userDataService;
        private readonly DoodleWinPopupState _doodleWinPopupState;
        private readonly DoodleLosePopupState _doodleLosePopupState;
        private readonly ChickenCareService _chickenCareService;
        private readonly IAudioService _audioService;

        private DoodleJumpGameScreen _screen;

        public DoodleJumpScreenStateController(ILogger logger, IUiService uiService, 
            DoodleGameSetupController doodleGameSetupController, DoodleGameData doodleGameData, UserInventoryService userInventoryService
            ,UserDataService userDataService, DoodleWinPopupState doodleWinPopupState, DoodleLosePopupState doodleLosePopupState,
            ChickenCareService chickenCareService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _doodleGameSetupController = doodleGameSetupController;
            _doodleGameData = doodleGameData;
            _userInventoryService = userInventoryService;
            _userDataService = userDataService;
            _doodleWinPopupState = doodleWinPopupState;
            _doodleLosePopupState = doodleLosePopupState;
            _chickenCareService = chickenCareService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            _doodleGameSetupController.SetupGame();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _doodleGameSetupController.EndGame();
            global::Finish.OnPlayerFinished -= ProcessVictory;
            _doodleGameData.OnTimeChanged -= ProcessTime;
            
            await _uiService.HideScreen(ConstScreens.DoodleJumpGameScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<DoodleJumpGameScreen>(ConstScreens.DoodleJumpGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<CareScreenStateController>();
            global::Finish.OnPlayerFinished += ProcessVictory;
            _doodleGameData.OnTimeChanged += ProcessTime;
        }

        private void ProcessVictory()
        {
            _chickenCareService.AddHappiness(HappinessAdd);
            RecordCoins();
            RecordClearTime();
            IncreaseHappiness();
            _doodleWinPopupState.Enter().Forget();
            _audioService.PlaySound(ConstAudio.VictorySound);
            AchievementMediator.InvokePlayTime();
        }

        private void ProcessTime(float time)
        {
            if(time <= 0)
                ProcessLose();
        }

        private void ProcessLose()
        {
            _doodleLosePopupState.Enter().Forget();
            _audioService.PlaySound(ConstAudio.LoseSound);
        }

        private void RecordCoins()
        {
            _userInventoryService.AddBalance(_doodleGameData.Coins);
        }

        private void RecordClearTime()
        {
            float lastBestTime = _userDataService.GetUserData().UserDoodleClearTime.BestTime;

            float time = _doodleGameData.Time;
            if(time > lastBestTime)
                _userDataService.GetUserData().UserDoodleClearTime.BestTime = time;
        }

        private void IncreaseHappiness()
        {
            
        }
    }
}