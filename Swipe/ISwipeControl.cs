using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts {
    public interface ISwipeControl : ITickable, IInitializable {
        void AddActions(Action<Vector2> startAction, Action<Vector2> endAction);
        void RemoveActions(Action<Vector2> startAction, Action<Vector2> endAction);
    }
}