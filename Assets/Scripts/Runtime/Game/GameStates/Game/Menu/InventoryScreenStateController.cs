using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Runtime.Core.Audio;
using Runtime.Core.GameStateMachine;
using Runtime.Game.Care;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UI;
using Runtime.Game.Services.UserData.Data;
using Runtime.Game.UI.Screen;
using UnityEngine;
using ILogger = Runtime.Core.Infrastructure.Logger.ILogger;

namespace Runtime.Game.GameStates.Game.Menu
{
    public class InventoryScreenStateController : StateController
    {
        private readonly IUiService _uiService;
        private readonly InventoryItemsFactory _inventoryItemsFactory;
        private readonly BoostersService _boostersService;
        private readonly ChickenCareService _chickenCareService;
        private readonly IAudioService _audioService;

        private InventoryScreen _screen;
        
        private List<BonusItemDisplay> _boostersList;
        private List<ChickenItemDisplay> _chickenList;

        public InventoryScreenStateController(ILogger logger, IUiService uiService,
            InventoryItemsFactory inventoryItemsFactory, BoostersService boostersService,
            ChickenCareService chickenCareService, IAudioService audioService) : base(logger)
        {
            _uiService = uiService;
            _inventoryItemsFactory = inventoryItemsFactory;
            _boostersService = boostersService;
            _chickenCareService = chickenCareService;
            _audioService = audioService;
        }

        public override UniTask Enter(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            
            CreateItems();

            return UniTask.CompletedTask;
        }

        private void CreateItems()
        {
            _boostersList = _inventoryItemsFactory.GetBonusItemsDisplay();
            _screen.SetBonusItems(_boostersList);

            var chickenItems = _inventoryItemsFactory.CreateChickensDisplay();
            _screen.SetChickenItems(chickenItems);
            
            var henItems = _inventoryItemsFactory.CreateHenDisplay();
            _screen.SetHenItems(henItems);
            
            _screen.SetEggItems(_inventoryItemsFactory.CreateEggsDisplay());

            foreach (var bonusItem in _boostersList)
                bonusItem.OnUsed += ProcessBonusUse;


            _chickenList = new();
            _chickenList.AddRange(chickenItems);
            _chickenList.AddRange(henItems);
            
            foreach (var chicken in _chickenList)
            {
                chicken.OnSelected += UpdateSelectedChicken;
            }
        }

        public override async UniTask Exit()
        {
            await _uiService.HideScreen(ConstScreens.InventoryScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<InventoryScreen>(ConstScreens.InventoryScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoTo<CareScreenStateController>();
        }

        private void ProcessBonusUse(int id)
        {
            switch (id)
            {
                case 0:
                    _boostersService.ApplyBooster(BoosterTypes.Grow);
                    break;
                case 1:
                    _boostersService.ApplyBooster(BoosterTypes.Happiness);
                    break;
                case 2:
                    _boostersService.ApplyBooster(BoosterTypes.Cleanliness);
                    break;
                case 3:
                    _boostersService.ApplyBooster(BoosterTypes.Hunger);
                    break;
                case 4:
                    _boostersService.ApplyBooster(BoosterTypes.Incubate);
                    break;
            }

            RemoveBoosterFromDisplay(id);
            _audioService.PlaySound(ConstAudio.SuccessSound);
        }

        private void RemoveBoosterFromDisplay(int id)
        {
            for (int i = 0; i < _boostersList.Count; i++)
            {
                var booster = _boostersList[i];

                if (booster.ItemID == id)
                {
                    booster.UpdateAmount(booster.Amount - 1);
                    if (booster.Amount <= 0)
                    {
                        Object.Destroy(booster.gameObject);
                        _boostersList.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private async void UpdateSelectedChicken(ItemData chick)
        {
            _chickenCareService.AddChickenToCare(chick.ItemType, chick.ItemId);
            _audioService.PlaySound(ConstAudio.ChickenSound);
            await GoTo<CareScreenStateController>();
        }
    }
}