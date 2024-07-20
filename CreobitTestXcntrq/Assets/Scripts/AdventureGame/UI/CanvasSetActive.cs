namespace Adventure
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

        private IGame Game => _game ??= (GetComponentInParent<IGame>() ?? _nullGame);
        private readonly IGame _nullGame = new NullGame();
        private IGame _game = null;

        private void Awake()
        {
            Game.GameStarted += Game_GameStarted;
            Game.VictoryTriggered += Game_VictoryTriggered;
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
            Game.GameStarted -= Game_GameStarted;
            Game.VictoryTriggered -= Game_VictoryTriggered;
        }
    }
}