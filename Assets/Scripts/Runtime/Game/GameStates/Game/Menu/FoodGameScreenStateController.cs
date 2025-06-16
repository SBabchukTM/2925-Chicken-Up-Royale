using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Core.Infrastructure.Logger;
using Runtime.Game.Care;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class FoodGameScreenStateController : StateController
    {
        private const float HunderAdd = 0.33f;
        
        private readonly IUiService _uiService;
        private readonly HammerInputProvider _hammerInputProvider;
        private readonly ChickenCareService _chickenCareService;
        private readonly IAudioService _audioService;
        private readonly FoodEndPopupStateController _foodEndPopupStateController;

        private FoodGameScreen _screen;

        private int _progress = 0;
        private int _target = 5;
        
        private bool _hammerEquipped = false;

        public FoodGameScreenStateController(ILogger logger, IUiService uiService,
            HammerInputProvider hammerInputProvider, ChickenCareService chickenCareService, IAudioService audioService, FoodEndPopupStateController foodEndPopupStateController) : base(logger)
        {
            _uiService = uiService;
            _hammerInputProvider = hammerInputProvider;
            _chickenCareService = chickenCareService;
            _audioService = audioService;
            _foodEndPopupStateController = foodEndPopupStateController;
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
            _hammerInputProvider.Enable(false);
            await _uiService.HideScreen(ConstScreens.FoodGameScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<FoodGameScreen>(ConstScreens.FoodGameScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<CareScreenStateController>();
            _hammerInputProvider.OnWormHoleClicked += ProcessClick;
            _screen.OnHammerPressed += () => _hammerEquipped = true;
        }

        private void SetupGame()
        {
            _progress = 0;
            _hammerEquipped = false;
            UpdateProgressDisplay();
            _hammerInputProvider.Enable(true);
        }

        private void ProcessVictory()
        {
            _chickenCareService.AddHunger(HunderAdd);
            AchievementMediator.InvokeSnackTime();
            _foodEndPopupStateController.Enter().Forget();
        }

        private void ProcessClick(WormHole hole)
        {
            if(!_hammerEquipped)
                return;
            
            if (hole.InAnim)
            {
                _audioService.PlaySound(ConstAudio.HitSound);
                hole.Stop();
                _progress++;
                UpdateProgressDisplay();
                _hammerEquipped = false;
                if (_progress >= _target)
                    ProcessVictory();
                else
                    _screen.PlayHammerAnim(hole.transform.position);
            }
        }

        private void UpdateProgressDisplay()
        {
            _screen.UpdateProgress(_progress, _target);
        }
    }
}