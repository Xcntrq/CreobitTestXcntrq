namespace Adventure
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [DisallowMultipleComponent]
    public class Game : MonoBehaviour, IGame
    {
        private readonly string _fileName = "adventure.bin";

        private GameData _gameData;
        private Transform _player;
        private float _lastRecord;

        private string FilePath => string.Concat(Application.persistentDataPath, '/', _fileName);
        private float BestTime => _gameData.BestTime;

        public float TimeElapsed => _gameData.TimeElapsed;
        public float TimeOfVictory => _gameData.TimeOfVictory;
        public float LastRecord => _lastRecord;

        public event Action GameStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform)
        {
            _player = transform;
        }

        public void TriggerVictory()
        {
            _gameData.TimeOfVictory = TimeElapsed;
            _gameData.BestTime = (BestTime == 0) ? TimeOfVictory : BestTime;
            _gameData.BestTime = (TimeOfVictory < BestTime) ? TimeOfVictory : BestTime;
            VictoryTriggered?.Invoke();
            SaveToFile(_gameData);
        }

        public void Restart()
        {
            _gameData.TimeElapsed = 0f;
            SaveToFile(_gameData);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void Quit()
        {
            SaveToFile(_gameData);
            SceneManager.LoadScene("MainMenu");
        }

        private void Awake()
        {
            _gameData = File.Exists(FilePath) ? LoadFromFile() : new();
            _lastRecord = _gameData.BestTime;
        }

        private void Start()
        {
            // Last game was a win, gotta reset the timer automatically
            if (_gameData.TimeOfVictory > 0)
            {
                _gameData.TimeElapsed = 0;
            }

            // Otherwise not a win and not a manual reset
            if (_gameData.TimeElapsed > 0)
            {
                _player.SetLocalPositionAndRotation(_gameData.PlayerPos, _gameData.PlayerRot);
            }

            GameStarted?.Invoke();
            TimeTicked?.Invoke();
        }

        private void Update()
        {
            _gameData.TimeElapsed += Time.deltaTime;
            _gameData.PlayerPos = _player.position;
            _gameData.PlayerRot = _player.rotation;
            TimeTicked?.Invoke();
        }

        private void SaveToFile(GameData gameData)
        {
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Create(FilePath);
            try
            {
                binaryFormatter.Serialize(fileStream, gameData);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            fileStream.Close();
        }

        private GameData LoadFromFile()
        {
            GameData result = null;
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Open(FilePath, FileMode.Open);
            try
            {
                if (binaryFormatter.Deserialize(fileStream) is GameData gameData)
                {
                    result = gameData;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                result = new();
            }

            fileStream.Close();
            return result;
        }
    }
}