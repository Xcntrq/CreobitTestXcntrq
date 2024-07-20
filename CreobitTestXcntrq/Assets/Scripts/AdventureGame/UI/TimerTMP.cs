namespace Adventure
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimerTMP : MonoBehaviour
    {
        private IGame Game => _game ??= (GetComponentInParent<IGame>() ?? _nullGame);
        private readonly IGame _nullGame = new NullGame();
        private IGame _game = null;

        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            Game.TimeTicked += Game_TimeTicked;
        }

        private void Game_TimeTicked()
        {
            int m = (int)(Game.TimeElapsed / 60f);
            int s = (int)Game.TimeElapsed - m * 60;
            _tmp.text = $"{m:D2}:{s:D2}";
        }

        private void OnDestroy()
        {
            Game.TimeTicked -= Game_TimeTicked;
        }
    }
}