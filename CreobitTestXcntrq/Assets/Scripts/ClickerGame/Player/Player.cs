namespace ClickerGame
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IPlayer
    {
        private readonly string _fileName = "clicker.bin";

        private string _filePath;

        private PlayerData _playerData;

        public long Score => _playerData.Score;

        public event Action DataChanged;
        public event Action PointAdded;

        public void AddOnePoint()
        {
            _playerData.AddOnePoint();
            SaveToFile(_playerData);
            DataChanged?.Invoke();
            PointAdded?.Invoke();
        }

        public void RestartGame()
        {
            _playerData = new();
            SaveToFile(_playerData);
            DataChanged?.Invoke();
        }

        private void Awake()
        {
            _filePath = string.Concat(Application.persistentDataPath, '/', _fileName);
            _playerData = File.Exists(_filePath) ? LoadFromFile() : new();
        }

        private void Start()
        {
            DataChanged?.Invoke();
        }

        private void SaveToFile(PlayerData playerData)
        {
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Create(_filePath);
            try
            {
                binaryFormatter.Serialize(fileStream, playerData);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }

            fileStream.Close();
        }

        private PlayerData LoadFromFile()
        {
            PlayerData result = null;
            BinaryFormatter binaryFormatter = new();
            FileStream fileStream = File.Open(_filePath, FileMode.Open);
            try
            {
                if (binaryFormatter.Deserialize(fileStream) is PlayerData playerData)
                {
                    result = playerData;
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