using System;
using Runtime.Core.Audio;
using Runtime.Game.Services;
using Runtime.Game.Services.Audio;
using Runtime.Game.Services.UserData;

namespace Runtime.Game.DailyRewards
{
    public class UserLoginService
    {
        private readonly UserDataService _userDataService;
        private readonly UserInventoryService _userInventoryService;
        private readonly IAudioService _audioService;

        public UserLoginService(UserDataService userDataService, UserInventoryService userInventoryService, IAudioService audioService)
        {
            _userDataService = userDataService;
            _userInventoryService = userInventoryService;
            _audioService = audioService;
        }

        public int GetLoginStreak()
        {
            return _userDataService.GetUserData().UserLoginData.LoginStreak;
        }

        public bool ShowReward(int configRewards)
        {
            var showReward = false;

            var firstLogin = _userDataService.GetUserData().UserLoginData.LastDailyRewardLoginTimeString == string.Empty;

            if (firstLogin)
            {
                showReward = true;
            }
            else
            {
                var lastLoginTime =
                    Convert.ToDateTime(_userDataService.GetUserData().UserLoginData.LastDailyRewardLoginTimeString);
                showReward = DateTime.Now.Date > lastLoginTime.Date;
            }

            return showReward && GetLoginStreak() < configRewards;
        }

        public void UpdateLoginStreak(DailyRewardDisplay display, int amount)
        {
            _audioService.PlaySound(ConstAudio.CoinSound);
            display.ClaimReward();

            _userInventoryService.AddBalance(amount);

            var loginData = _userDataService.GetUserData().UserLoginData;
            loginData.LoginStreak++;
            loginData.LastDailyRewardLoginTimeString = DateTime.Now.ToString();
        }

        public void RecordChickenVisitTime()
        {
            var loginData = _userDataService.GetUserData().UserLoginData;
            loginData.LastChickenVisitTimeString = DateTime.Now.ToString();
        }

        public void RecordIncubatorVisitTime()
        {
            var loginData = _userDataService.GetUserData().UserLoginData;
            loginData.LastIncubatorVisitTimeString = DateTime.Now.ToString();
        }
    }
}