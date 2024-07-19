namespace Clicker
{
    using System;

    [Serializable]
    public class PlayerData
    {
        private long _score;

        public long Score => _score;

        public PlayerData()
        {
            _score = 0;
        }

        public void AddOnePoint()
        {
            _score++;
        }
    }
}