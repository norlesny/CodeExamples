using System;
using Assets.Scripts.Network.Models;

namespace Assets.Scripts.Network.DataAccess {
    public interface IUserDataAccess {
        void GetUserData(Action<UserGameData> resultCallback, bool forceOverwrite = false);

        void GetLevelsTresholds(Action<LevelsTresholds> resultCallback);

        void ChangeUserData(UserDataChangeModel model, Action<UserGameData> resultCallback);
    }
}