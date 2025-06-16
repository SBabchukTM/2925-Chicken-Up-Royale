using System;
using DG.Tweening;
using Runtime.Core.Audio;
using Runtime.Core.Factory;
using Runtime.Core.Infrastructure.AssetProvider;
using Runtime.Core.Infrastructure.SettingsProvider;
using Runtime.Game.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ShopModeButton : MonoBehaviour
{
    [SerializeField] protected RectTransform Content;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;

    protected GameObjectFactory GameObjectFactory;
    protected ISettingProvider SettingProvider;
    protected IAssetProvider AssetProvider;
    protected UserInventoryService UserInventoryService;
    protected IAudioService AudioService;
    
    public event Action<ShopModeButton> OnClicked;

    [Inject]
    private void Construct(ISettingProvider settingProvider, GameObjectFactory gameObjectFactory, 
        IAssetProvider assetProvider, UserInventoryService userInventoryService, IAudioService audioService)
    {
        GameObjectFactory = gameObjectFactory;
        SettingProvider = settingProvider;
        AssetProvider = assetProvider;
        UserInventoryService = userInventoryService;
        AudioService = audioService;
    }

    private void Awake()
    {
        _button.onClick.AddListener(() =>
        {
            OnClicked?.Invoke(this);
        });
    }

    public void EnableView(bool enable)
    {
        Content.gameObject.SetActive(enable);
        if (enable)
        {
            SetAlpha(1);
            _scrollRect.content = Content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.transform as RectTransform);
        }
        else
        {
            SetAlpha(0);
        }
    }

    private void SetAlpha(float alpha)
    {
        _image.DOFade(alpha, 0.5f);
    }
}
