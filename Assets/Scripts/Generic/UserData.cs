using System;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    [Serializable]
    public class UserData
    {
        public string name;
        public int starCount;
        public int money;
        public string playTime;

        public Dictionary<string, int> clearedStages;
        public List<string> unLockStages;

        public static UserData CreateNewUserData()
        {
            UserData userData = new UserData();
        
            userData.name = SystemInfo.deviceName;
            userData.starCount = 0;
            userData.money = 1000;
            userData.playTime = TimeSpan.Zero.ToString();
            userData.clearedStages = new Dictionary<string, int>();
            userData.unLockStages = new List<string>();
        
            return userData;
        }
    }
}
