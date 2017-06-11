using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Network.Models;
using UnityEngine;

namespace Assets.Scripts.Network.DataAccess {
    public class UserDataAccessImpl : IUserDataAccess {
        private readonly Networking networking;

        private UserGameData userGameData;

        public UserDataAccessImpl(Networking networking) {
            this.networking = networking;
            LoadDataAtStart();
        }

        private void LoadDataAtStart() {
            GetUserData(data => { });
            GetLevelsTresholds(tresholds => { });
        }

        public void GetUserData(Action<UserGameData> resultCallback, bool forceOverwrite = false) {
            if (userGameData != null && !forceOverwrite) {
                resultCallback(userGameData);
            }
            else {
                var request = new GetRequest<UserGameData>()
                    .SelectMethod(NetworkConstants.Methods.USER_DATA)
                    .AddSuccesCallback(data => {
                        userGameData = data;
                        resultCallback(data);
                    })
                    .Build();
                networking.SendRequest(request);
            }
        }

        public void GetLevelsTresholds(Action<LevelsTresholds> resultCallback) {
            if (LevelsTresholds.Instance != null) {
                resultCallback(LevelsTresholds.Instance);
            }
            else {
                var request = new GetRequest<LevelsTresholds>()
                    .SelectMethod(NetworkConstants.Methods.TRESHOLDS)
                    .AddSuccesCallback(data => {
                        LevelsTresholds.Instance = data;
                        resultCallback(data);
                    })
                    .Build();
                networking.SendRequest(request);
            }
        }

        public void ChangeUserData(UserDataChangeModel model, Action<UserGameData> resultCallback) {
            var request = new PostRequest<UserGameData>()
                .SelectMethod(NetworkConstants.Methods.USER_DATA_EDIT)
                .AddBodyParameters(model)
                .AddSuccesCallback(data => {
                    userGameData = data;
                    resultCallback(data);
                })
                .Build();
            networking.SendRequest(request);
        }
    }
}