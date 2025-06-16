using Runtime.Game.Care;
using Runtime.Game.DailyRewards;
using Runtime.Game.DoodleJumpMiniGame;
using Runtime.Game.EggLaying;
using Runtime.Game.GameStates.Game.Menu;
using Runtime.Game.Incubation;
using Runtime.Game.Market;
using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using UnityEngine;
using Zenject;

namespace Runtime.Game.GameStates.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        [SerializeField] private DoodleJumpGameEnabler _doodleJumpGameEnabler;
        
        public override void InstallBindings()
        {
            Container.Bind<MenuStateController>().AsSingle();
            Container.Bind<AchievementsScreenStateController>().AsSingle();
            Container.Bind<AccountScreenStateController>().AsSingle();
            Container.Bind<BathGameScreenStateController>().AsSingle();
            Container.Bind<CareScreenStateController>().AsSingle();
            Container.Bind<DailyScreenStateController>().AsSingle();
            Container.Bind<DoodleJumpScreenStateController>().AsSingle();
            Container.Bind<FoodGameScreenStateController>().AsSingle();
            Container.Bind<IncubationScreenStateController>().AsSingle();
            Container.Bind<InventoryScreenStateController>().AsSingle();
            Container.Bind<MarketScreenStateController>().AsSingle();
            Container.Bind<SettingsScreenStateController>().AsSingle();
            Container.Bind<ShopScreenStateController>().AsSingle();
            Container.Bind<FoodEndPopupStateController>().AsSingle();
            Container.Bind<BathEndPopupStateController>().AsSingle();
            Container.Bind<UserInventoryService>().AsSingle();
            Container.Bind<UserLoginService>().AsSingle();
            Container.Bind<ModeSelectStateController>().AsSingle();
            Container.Bind<MarketService>().AsSingle();
            Container.Bind<MarketItemSelectController>().AsSingle();
            Container.BindInterfacesAndSelfTo<DailyRewardsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<MarketItemSelectionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<EggItemSelectFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoosterDisplayFactory>().AsSingle();
            Container.Bind<ItemDataService>().AsSingle();
            Container.Bind<EggItemSelectController>().AsSingle();
            Container.Bind<IncubatorService>().AsSingle();
            Container.Bind<ChickenCareService>().AsSingle();
            Container.Bind<ChickenGrowService>().AsSingle();
            Container.Bind<BoostersService>().AsSingle();
            Container.Bind<EggLayingService>().AsSingle();

            Container.BindInterfacesAndSelfTo<WashingInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryItemsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<HammerInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<AchievementDisplayFactory>().AsSingle();
            BindDoodleMiniGame();
        }

        private void BindDoodleMiniGame()
        {
            Container.BindInterfacesAndSelfTo<ChickenInputProvider>().AsSingle();
            Container.Bind<DoodleGameTimer>().AsSingle();
            Container.Bind<DoodleGameData>().AsSingle();
            Container.Bind<DoodleGameSetupController>().AsSingle();
            Container.Bind<DoodleLosePopupState>().AsSingle();
            Container.Bind<DoodleWinPopupState>().AsSingle();
            Container.Bind<DoodleJumpGameEnabler>().FromComponentInNewPrefab(_doodleJumpGameEnabler).AsSingle();
        }
    }
}