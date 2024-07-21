namespace ClickerGame
{
    using System;

    [Serializable]
    public class PlayerData
    {
        private long _score = 0;

        public long Score => _score;

        public void AddOnePoint()
        {
            _score++;
        }
    }
}