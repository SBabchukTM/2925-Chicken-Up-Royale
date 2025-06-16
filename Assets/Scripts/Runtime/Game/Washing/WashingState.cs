using System;
using UnityEngine;

public class WashingState
{
    private float _progressSpeed = 1;
    private readonly WashingItemType _requiredItem;
    private float _progress;

    public float Progress => _progress;

    public event Action OnCompleted;
    public event Action<float> OnProgressChanged;

    public WashingState(WashingItemType requiredItem, float progressSpeed)
    {
        _requiredItem = requiredItem;
        _progressSpeed = progressSpeed;
    }

    public void EnterState()
    {
        UpdateProgress(0);
    }

    public void ProcessUserActions(WashingItemType selectedItem)
    {
        if (selectedItem != _requiredItem)
            return;

        UpdateProgress(_progress + _progressSpeed * Time.deltaTime);
        OnProgressChanged?.Invoke(_progress);

        if (_progress >= 1f)
            OnCompleted?.Invoke();
    }

    private void UpdateProgress(float progress)
    {
        _progress = Mathf.Clamp01(progress);
    }
}