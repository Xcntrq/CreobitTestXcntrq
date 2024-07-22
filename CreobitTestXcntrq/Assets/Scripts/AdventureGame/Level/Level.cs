namespace AdventureGame
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [DisallowMultipleComponent]
    public class Level : MonoBehaviour, ILevel
    {
        private readonly string _fileName = "adventure.bin";

        private LevelData _levelData;
        private Transform _player;
        private float _lastRecord;

        private string FilePath => string.Concat(Application.persistentDataPath, '/', _fileName);
        private float BestTime => _levelData.BestTime;
        private IGame Game => _game ??= (GetComponentInParent<IGame>() ?? _nullGame);
        private readonly IGame _nullGame = new NullGame();
        private IGame _game = null;

        public float TimeElapsed => _levelData.TimeElapsed;
        public float TimeOfVictory => _levelData.TimeOfVictory;
        public float LastRecord => _lastRecord;

        public event Action LevelStarted;
        public event Action TimeTicked;
        public event Action VictoryTriggered;

        public void RegisterPlayer(Transform transform)
        {
            _player = transform;
        }

        public void TriggerVictory()
        {
            _levelData.TimeOfVictory = TimeElapsed;
            _levelData.BestTime = (BestTime == 0) ? TimeOfVictory : BestTime;
            _levelData.BestTime = (TimeOfVictory < BestTime) ? TimeOfVictory : BestTime;
            VictoryTriggered?.Invoke();
            SaveToFile(_levelData);
        }

        public void ResetLevel()
        {
            // The timer is reset manually
            _levelData.TimeElapsed = 0f;
            SaveToFile(_levelData);
            Game.ReloadCurrentLevel();
        }

        public void Quit()
        {
            SaveToFile(_levelData);
            SceneManager.LoadScene("MainMenu");
        }

        private void Awake()
        {
            _levelData = File.Exists(FilePath) ? LoadFromFile() : new();
            _lastRecord = _levelData.BestTime;
        }

        private void Start()
        {
            // The timer is reset after victory along with the result
            if (_levelData.TimeOfVictory > 0)
            {
                _levelData.TimeOfVictory = 0;
                _levelData.TimeElapsed = 0;
            }

            // If the timer wasn't reset, we load the last session
            if (_levelData.TimeElapsed > 0)
            {
                _player.SetLocalPositionAndRotation(_levelData.PlayerPos, _levelData.PlayerRot);
            }

            LevelStarted?.Invoke();
            TimeTicked?.Invoke();
        }

        private void Update()
        {
            _levelData.TimeElapsed += Time.deltaTime;
            _levelData.PlayerPos = _player.position;
            _levelData.PlayerRot = _player.rotation;
            TimeTicked?.Invoke();
        }

        private void SaveToFile(LevelData levelData)
        {
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Create(FilePath);
            try
            {
                binaryFormatter.Serialize(fileStream, levelData);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            fileStream.Close();
        }

        private LevelData LoadFromFile()
        {
            LevelData result = null;
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Open(FilePath, FileMode.Open);
            try
            {
                if (binaryFormatter.Deserialize(fileStream) is LevelData levelData)
                {
                    result = levelData;
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