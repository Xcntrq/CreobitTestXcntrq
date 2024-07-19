namespace Clicker
{
    using System;

    public interface IPlayer
    {
        public long Score { get; }

        public event Action DataChanged;
        public event Action PointAdded;

        public void AddOnePoint();

        public void RestartGame();
    }
}