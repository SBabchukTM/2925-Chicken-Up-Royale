using System.Threading;
using Cysharp.Threading.Tasks;

namespace Runtime.Game.DoodleJumpMiniGame
{
    public class DoodleGameSetupController
    {
        private const float TimeAmount = 30;
        
        private readonly DoodleGameData _data;
        private readonly DoodleGameTimer _timer;
        private readonly DoodleJumpGameEnabler _enabler;

        private CancellationTokenSource _cts;
        
        public DoodleGameSetupController(DoodleGameData data, DoodleGameTimer timer, DoodleJumpGameEnabler enabler)
        {
            _data = data;
            _timer = timer;
            _enabler = enabler;
        }

        public void SetupGame()
        {
            _cts = new ();
            ResetData();
            
            _timer.Start(TimeAmount, _cts.Token).Forget();
            _enabler.SpawnLevel();
        }

        public void EndGame()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            
            _enabler.DestroyLevel();
        }
        
        private void ResetData()
        {
            _data.Coins = 0;
            _data.Time = TimeAmount;
        }
    }
}