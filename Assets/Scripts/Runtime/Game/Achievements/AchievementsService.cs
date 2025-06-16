using Runtime.Game.Services;
using Runtime.Game.Services.UserData;
using Runtime.Game.Services.UserData.Data;

namespace Runtime.Game.Achievements
{
    public class AchievementsService
    {
        private readonly UserDataService _userDataService;
        
        public AchievementsService(UserDataService userDataService)
        {
            _userDataService = userDataService;
            
            AchievementMediator.OnFirstHatch += ProcessFirstHatch;
            AchievementMediator.OnNewCaretaker += ProcessNewCaretaker;
            AchievementMediator.OnBathTime += ProcessBathTime;
            AchievementMediator.OnSnackTime += ProcessSnackTime;
            AchievementMediator.OnPlayTime += ProcessPlayTime;
            AchievementMediator.OnBoosterShopper += ProcessBoosterShopper;
            AchievementMediator.OnCheater += OnCheater;
            AchievementMediator.OnStylist += ProcessStylist;
            AchievementMediator.OnGrowTime += ProcessGrowTime;
            AchievementMediator.OnSeller += ProcessSeller;
            AchievementMediator.OnNewEnvironment += ProcessNewEnvironment;
        }

        private void ProcessFirstHatch()
        {
            ProcessAchievement(ref GetAchievementsData().FirstHatch);
        }

        private void ProcessNewCaretaker()
        {
            ProcessAchievement(ref GetAchievementsData().NewCaretaker);
        }

        private void ProcessBathTime()
        {
            ProcessAchievement(ref GetAchievementsData().BathTime);
        }

        private void ProcessSnackTime()
        {
            ProcessAchievement(ref GetAchievementsData().SnackTime);
        }

        private void ProcessPlayTime()
        {
            ProcessAchievement(ref GetAchievementsData().PlayTime);
        }

        private void ProcessBoosterShopper()
        {
            ProcessAchievement(ref GetAchievementsData().BoosterShopper);
        }

        private void OnCheater()
        {
            ProcessAchievement(ref GetAchievementsData().Cheater);
        }

        private void ProcessStylist()
        {
            ProcessAchievement(ref GetAchievementsData().Stylist);
        }

        private void ProcessGrowTime()
        {
            ProcessAchievement(ref GetAchievementsData().GrowTime);
        }

        private void ProcessSeller()
        {
            ProcessAchievement(ref GetAchievementsData().Seller);
        }

        private void ProcessNewEnvironment()
        {
            ProcessAchievement(ref GetAchievementsData().NewEnvironment);
        }

        private UserAchievementsData GetAchievementsData() => _userDataService.GetUserData().UserAchievementsData;

        private void ProcessAchievement(ref AchievementData achievement)
        {
            if (achievement.Unlocked)
                return;

            achievement.Unlocked = true;
        }
    }
}