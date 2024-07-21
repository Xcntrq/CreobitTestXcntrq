namespace AdventureGame
{
    using System;
    using UnityEngine;

    public interface ILevel
    {
        public float TimeElapsed { get; }
        public float TimeOfVictory { get; }
        public float LastRecord { get; }

        public event Action LevelStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform);

        public void TriggerVictory();

        public void ResetLevel();

        public void Quit();
    }
}