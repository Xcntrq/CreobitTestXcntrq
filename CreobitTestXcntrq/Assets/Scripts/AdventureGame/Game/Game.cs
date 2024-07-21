namespace AdventureGame
{
    using UnityEngine;

    public class Game : MonoBehaviour, IGame
    {
        [SerializeField] private GameObject[] _levels;
        [SerializeField] private GameObject _currentLevel;

        public void ReloadCurrentLevel()
        {
            Destroy(_currentLevel);
            _currentLevel = Instantiate(_levels[0], transform);
        }
    }
}