using System;
using System.Collections.Generic;
using Runtime.Core.Audio;
using Runtime.Game.Services.Audio;
using Zenject;

public class WashingStateManager
{
    private readonly List<WashingState> _states;
    private int _currentIndex;

    private WashingState CurrentState => _states[_currentIndex];

    public event Action OnWashingComplete;
    public event Action OnStateChanged;
    public event Action<float> OnProgressChanged;
    

    public WashingStateManager()
    {
        _states = new List<WashingState>
        {
            new (WashingItemType.Water, 0.33f),
            new (WashingItemType.Soap, 0.5f),
            new (WashingItemType.Brush, 0.2f)
        };

        foreach (var state in _states)
        {
            state.OnCompleted += ChangeState;
            state.OnProgressChanged += InvokeProgressChanged;
        }

        _currentIndex = 0;
        CurrentState.EnterState();
    }

    private void InvokeProgressChanged(float progress) => OnProgressChanged?.Invoke(progress);

    public void UpdateProgress(WashingItemType itemType)
    {
        if(_currentIndex < _states.Count)
            CurrentState.ProcessUserActions(itemType);
    }

    private void ChangeState()
    {
        _currentIndex++;
        if (_currentIndex < _states.Count)
        {
            CurrentState.EnterState();
            OnStateChanged?.Invoke();
        }
        else
            OnWashingComplete?.Invoke();
    }
}
