using System;

namespace Runtime.Game.Services.UserData.Data
{
    [Serializable]
    public class UserAchievementsData
    {
        public AchievementData FirstHatch = new ()
        {
            Name = "FirstHatch",
            Description = "Hatch your first egg.",
            Reward = 150,
        };
        
        public AchievementData NewCaretaker = new ()
        {
            Name = "New Caretaker",
            Description = "Open the care screen for the first time.",
            Reward = 65,
        };
        
        public AchievementData BathTime = new ()
        {
            Name = "Bath Time",
            Description = "Clean a chicken for the first time.",
            Reward = 70,
        };
        
        public AchievementData SnackTime = new ()
        {
            Name = "Snack Time",
            Description = "Feed a chicken for the first time.",
            Reward = 70,
        };
        
        public AchievementData PlayTime = new ()
        {
            Name = "Play Time",
            Description = "Play with a chicken for the first time.",
            Reward = 70,
        };
        
        public AchievementData BoosterShopper = new ()
        {
            Name = "Booster Shopper",
            Description = "Purchase your first booster.",
            Reward = 200,
        };
        
        public AchievementData Cheater = new ()
        {
            Name = "Cheater!",
            Description = "Use any booster for the first time.",
            Reward = 50,
        };
        
        public AchievementData Stylist = new ()
        {
            Name = "Stylist",
            Description = "Purchase your first background area.",
            Reward = 200,
        };
        
        public AchievementData GrowTime = new ()
        {
            Name = "Grown Up!",
            Description = "Grow a chicken into a hen for the first time.",
            Reward = 250,
        };
        
        public AchievementData Seller = new ()
        {
            Name = "Business boom!",
            Description = "Sell any item on the market.",
            Reward = 50,
        };

        public AchievementData NewEnvironment = new()
        {
            Name = "New Environment",
            Description = "Purchase any new background.",
            Reward = 250,
        };
    }

    [Serializable]
    public class AchievementData
    {
        public string Name;
        public string Description;
        public bool Unlocked = false;
        public bool Claimed = false;
        public int Reward = 0;
    }
}