namespace AdventureGame
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public class CanvasSetActive : MonoBehaviour
    {
        private enum ActivityType
        {
            Gameplay,
            Victory,
        }

        [SerializeField] private ActivityType _activityType;

        private ILevel Level => _level ??= (GetComponentInParent<ILevel>() ?? _nullLevel);
        private readonly ILevel _nullLevel = new NullLevel();
        private ILevel _level = null;

        private void Awake()
        {
            Level.LevelStarted += Game_GameStarted;
            Level.VictoryTriggered += Game_VictoryTriggered;
        }

        private void Game_GameStarted()
        {
            gameObject.SetActive(_activityType == ActivityType.Gameplay);
        }

        private void Game_VictoryTriggered()
        {
            gameObject.SetActive(_activityType == ActivityType.Victory);
        }

        private void OnDestroy()
        {
            Level.LevelStarted -= Game_GameStarted;
            Level.VictoryTriggered -= Game_VictoryTriggered;
        }
    }
}