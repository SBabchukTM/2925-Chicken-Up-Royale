using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DoodleGameTimer
{
    private DoodleGameData _data;
    private float _currentTime;
    private int _lastReportedSecond;

    [Inject]
    private void Construct(DoodleGameData data)
    {
        _data = data;
    }

    public async UniTask Start(float time, CancellationToken token)
    {
        _currentTime = time;
        _lastReportedSecond = Mathf.FloorToInt(_currentTime);
        _data.Time = _currentTime;

        while (!token.IsCancellationRequested && _currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;

            int currentSecond = Mathf.FloorToInt(_currentTime);
            if (currentSecond != _lastReportedSecond)
            {
                _lastReportedSecond = currentSecond;
                _data.Time = currentSecond;
            }

            await UniTask.NextFrame(cancellationToken: token);
        }

        _data.Time = 0;
    }
}
