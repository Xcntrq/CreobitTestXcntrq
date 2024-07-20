namespace Adventure
{
    using System;
    using UnityEngine;

    public interface IGame
    {
        public float TimeElapsed { get; }
        public float TimeOfVictory { get; }
        public float LastRecord { get; }

        public event Action GameStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform);

        public void TriggerVictory();

        public void Restart();

        public void Quit();
    }
}