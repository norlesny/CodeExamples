using System;
using Assets.Scripts.Network.DataAccess;
using Assets.Scripts.Network.Models;

namespace Assets.Scripts.Network.Mock {
    public class MockUserDataAccess : IUserDataAccess {
        private UserGameData userGameData;
        private LevelsTresholds levelsTresholds;

        public MockUserDataAccess() {
            userGameData = new UserGameData {
                first_name = "John",
                last_name = "Doe",
                nickname = "heresjohny",
                email = "temptest@gmail.com",
                user_photo = "http://lorempixel.com/500/500/",
                level_points = 250,
                prestige_points = 75,
            };

            levelsTresholds = new LevelsTresholds {
                level_treshold = new[] {0, 100, 500, 1000, 5000},
                prestige_treshold = new[] {0, 100, 500, 1000, 5000},
            };
        }

        public void GetUserData(Action<UserGameData> resultCallback, bool forceOverwrite = false) {
            resultCallback(userGameData);
        }

        public void GetLevelsTresholds(Action<LevelsTresholds> resultCallback) {
            resultCallback(levelsTresholds);
        }

        public void ChangeUserData(UserDataChangeModel model, Action<UserGameData> resultCallback) {
            userGameData.first_name = model.first_name;
            userGameData.last_name = model.last_name;
            userGameData.nickname = model.nickname;
            userGameData.email = model.email;

            resultCallback(userGameData);
        }
    }
}