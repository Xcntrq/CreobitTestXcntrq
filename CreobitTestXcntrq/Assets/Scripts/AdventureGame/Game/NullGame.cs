namespace AdventureGame
{
    using UnityEngine;

    public class NullGame : IGame
    {
        private readonly string _message = "Game not found!";

        public void ReloadCurrentLevel() => Debug.LogWarning(_message);
    }
}