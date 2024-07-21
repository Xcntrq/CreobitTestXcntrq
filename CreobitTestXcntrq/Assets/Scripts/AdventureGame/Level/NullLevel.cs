namespace AdventureGame
{
    using System;
    using UnityEngine;

    public class NullLevel : ILevel
    {
        private readonly string _message = "Level not found!";

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

        public event Action LevelStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform) => Debug.LogWarning(_message);

        public void TriggerVictory() => Debug.LogWarning(_message);

        public void ResetLevel() => Debug.LogWarning(_message);

        public void Quit() => Debug.LogWarning(_message);

        public void Bayda()
        {
            LevelStarted?.Invoke();
            TimeTicked?.Invoke();
            VictoryTriggered?.Invoke();
        }
    }
}