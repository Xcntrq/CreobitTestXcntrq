namespace AdventureGame
{
    using System;
    using UnityEngine;

    [Serializable]
    public class LevelData
    {
        private float _timeElapsed = 0f;
        private float _timeOfVictory = 0f;
        private float _bestTime = 0f;
        private float _posX = 0f;
        private float _posY = 0f;
        private float _posZ = 0f;
        private float _rotX = 0f;
        private float _rotY = 0f;
        private float _rotZ = 0f;

        public float TimeElapsed
        {
            get { return _timeElapsed; }
            set { _timeElapsed = value; }
        }

        public float TimeOfVictory
        {
            get { return _timeOfVictory; }
            set { _timeOfVictory = value; }
        }

        public float BestTime
        {
            get { return _bestTime; }
            set { _bestTime = value; }
        }

        public Vector3 PlayerPos
        {
            get
            {
                return new(_posX, _posY, _posZ);
            }
            set
            {
                _posX = value.x;
                _posY = value.y;
                _posZ = value.z;
            }
        }

        public Quaternion PlayerRot
        {
            get
            {
                return Quaternion.Euler(_rotX, _rotY, _rotZ);
            }
            set
            {
                _rotX = value.eulerAngles.x;
                _rotY = value.eulerAngles.y;
                _rotZ = value.eulerAngles.z;
            }
        }
    }
}