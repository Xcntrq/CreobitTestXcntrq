namespace Clicker
{
    using System;
    using UnityEngine;

    public class NullPlayer : IPlayer
    {
        private readonly string _message = "Player not found!";

        public long Score
        {
            get
            {
                Debug.LogWarning(_message);
                return 0;
            }
        }

        public event Action DataChanged;
        public event Action PointAdded;

        public void AddOnePoint() => Debug.LogWarning(_message);

        public void RestartGame() => Debug.LogWarning(_message);

        public void Bayda()
        {
            DataChanged?.Invoke();
            PointAdded?.Invoke();
        }
    }
}