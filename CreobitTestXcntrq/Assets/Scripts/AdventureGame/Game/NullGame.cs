namespace Adventure
{
    using System;
    using UnityEngine;

    public class NullGame : IGame
    {
        private readonly string _message = "Game not found!";

        public float TimeElapsed
        {
            get
            {
                Debug.LogWarning(_message);
                return 0f;
            }
        }

        public float TimeOfVictory
        {
            get
            {
                Debug.LogWarning(_message);
                return 0f;
            }
        }

        public float LastRecord
        {
            get
            {
                Debug.LogWarning(_message);
                return 0f;
            }
        }

        public event Action GameStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform) => Debug.LogWarning(_message);

        public void TriggerVictory() => Debug.LogWarning(_message);

        public void Restart() => Debug.LogWarning(_message);

        public void Quit() => Debug.LogWarning(_message);

        public void Bayda()
        {
            GameStarted?.Invoke();
            TimeTicked?.Invoke();
            VictoryTriggered?.Invoke();
        }
    }
}