using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;

namespace Assets.Scripts {
    public class SwipeControlImpl : ISwipeControl {
        private List<Action<Vector2>> startActions;
        private List<Action<Vector2>> endActions;

        public void Initialize() {
            startActions = new List<Action<Vector2>>();
            endActions = new List<Action<Vector2>>();
        }

        public void Tick() {
            if (startActions.IsEmpty() && endActions.IsEmpty()) return;

#if UNITY_EDITOR
            MouseSwipe();
#else
            SwipeCheck();
#endif
        }

        public void AddActions(Action<Vector2> startAction, Action<Vector2> endAction) {
            startActions.Add(startAction);
            endActions.Add(endAction);
        }

        public void RemoveActions(Action<Vector2> startAction, Action<Vector2> endAction) {
            startActions.Remove(startAction);
            endActions.Remove(endAction);
        }

        private void SwipeCheck() {
            if (Input.touchCount > 0) {
                var t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began) {
                    LaunchAllStartActions(new Vector2(t.position.x, t.position.y));
                }
                if (t.phase == TouchPhase.Ended) {
                    LaunchAllEndActions(new Vector2(t.position.x, t.position.y));
                }
            }
        }

        private void MouseSwipe() {
            if (Input.GetMouseButtonDown(0)) {
                LaunchAllStartActions(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
            if (Input.GetMouseButtonUp(0)) {
                LaunchAllEndActions(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            }
        }

        private void LaunchAllStartActions(Vector2 param) {
            LaunchAllActions(startActions, param);
        }

        private void LaunchAllEndActions(Vector2 param) {
            LaunchAllActions(endActions, param);
        }

        private void LaunchAllActions(List<Action<Vector2>> actionsList, Vector2 param) {
            for (var i = 0; i < actionsList.Count; i++) {
                actionsList[i](param);
            }
        }
    }
}