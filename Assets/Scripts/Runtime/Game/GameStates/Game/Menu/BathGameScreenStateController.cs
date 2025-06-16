using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Care;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class BathGameScreenStateController : StateController
    {
        private const float CleanlinessAdd = 0.5f;
        
        private readonly IUiService _uiService;
        private readonly WashingInputProvider _washingInputProvider;
        private readonly ChickenCareService _chickenCareService;
        private readonly IAudioService _audioService;
        private readonly BathEndPopupStateController _bathEndPopupStateController;

        private BathGameScreen _screen;

        private WashingStateManager _washingStateManager;

        private WashingItemType _currentItem;
        
        public BathGameScreenStateController(ILogger logger, IUiService uiService,
            WashingInputProvider washingInputProvider, ChickenCareService chickenCareService, IAudioService audioService,
            BathEndPopupStateController bathEndPopupStateController) : base(logger)
        {
            _uiService = uiService;
            _washingInputProvider = washingInputProvider;
            _chickenCareService = chickenCareService;
            _audioService = audioService;
            _bathEndPopupStateController = bathEndPopupStateController;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            SetupGame();
            return UniTask.CompletedTask;
        }

        public override async UniTask Exit()
        {
            _washingInputProvider.SetEnabled(false);
            _washingInputProvider.OnInput -= ProcessWashingInput;
            _washingStateManager.OnWashingComplete -= ProcessWashingComplete;
            _washingStateManager.OnProgressChanged -= UpdateWashingProgress;
            _washingStateManager.OnStateChanged -= PlaySuccessSound;

            await _uiService.HideScreen(ConstScreens.BathGameScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<BathGameScreen>(ConstScreens.BathGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<CareScreenStateController>();
            _screen.OnBrushPressed += () => _currentItem = WashingItemType.Brush;
            _screen.OnSoapPressed += () => _currentItem = WashingItemType.Soap;
            _screen.OnWaterPressed += () => _currentItem = WashingItemType.Water;
        }

        private void SetupGame()
        {
            _currentItem = WashingItemType.None;
            
            _washingStateManager = new();
            _washingStateManager.OnWashingComplete += ProcessWashingComplete;
            _washingStateManager.OnProgressChanged += UpdateWashingProgress;
            _washingStateManager.OnStateChanged += PlaySuccessSound;
            
            _washingInputProvider.SetEnabled(true);
            _washingInputProvider.OnInput += ProcessWashingInput;
        }

        private void PlaySuccessSound()
        {
            _audioService.PlaySound(ConstAudio.SuccessSound);
        }

        private void ProcessWashingInput()
        {
            _washingStateManager.UpdateProgress(_currentItem);
        }

        private void UpdateWashingProgress(float progress)
        {
            _screen.UpdateProgress(_currentItem, progress);
        }

        private void ProcessWashingComplete()
        {
            _chickenCareService.AddCleanliness(CleanlinessAdd);
            _audioService.PlaySound(ConstAudio.SuccessSound);
            AchievementMediator.InvokeBathTime();
            _bathEndPopupStateController.Enter().Forget();
        }
    }
}