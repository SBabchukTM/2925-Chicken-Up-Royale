using System;

namespace Runtime.Game.UserAccountSystem
{
    [Serializable]
    public class UserAccountData
    {
        public string Username = "Player";
        public int Age = 18;
        public string Gender = "Male";
        public string AvatarBase64 = String.Empty;

        public UserAccountData Copy()
        {
            return (UserAccountData)MemberwiseClone();
        }
    }
}