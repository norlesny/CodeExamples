using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Network.Models {
    [Serializable]
    public class UserGameData {
        public string first_name;
        public string last_name;
        public string nickname;
        public string email;
        public string user_photo;

        #region Stats

        public int level_points;
        public int prestige_points;
        
        public int UserLevel {
            get
            {
                int level = 1;
                var tresholds = LevelsTresholds.Instance.level_treshold;
                for (var i = 1; i < tresholds.Length; i++) {
                    if (level_points < tresholds[i]) {
                        break;
                    }
                    level++;
                }
                return level;
            }
        }

        public int UserPrestige {
            get
            {
                int prestige = 1;
                var tresholds = LevelsTresholds.Instance.prestige_treshold;
                for (var i = 1; i < tresholds.Length; i++) {
                    if (prestige_points < tresholds[i]) {
                        break;
                    }
                    prestige++;
                }
                return prestige;
            }
        }

        public bool IsMaxUserLevel {
            get { return UserLevel == LevelsTresholds.Instance.level_treshold.Length; }
        }

        public bool IsMaxPrestigeLevel {
            get { return UserPrestige == LevelsTresholds.Instance.prestige_treshold.Length; }
        }

        #endregion
    }

    [Serializable]
    public class UserDataChangeModel {
        public string first_name;
        public string last_name;
        public string nickname;
        public string email;
    }
}