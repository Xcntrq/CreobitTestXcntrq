namespace Adventure
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class YourTimeTMP : MonoBehaviour
    {
        private IGame Game => _game ??= (GetComponentInParent<IGame>() ?? _nullGame);
        private readonly IGame _nullGame = new NullGame();
        private IGame _game = null;

        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            Game.VictoryTriggered += Game_VictoryTriggered;
        }

        private void Game_VictoryTriggered()
        {
            int m = (int)(Game.TimeOfVictory / 60f);
            int s = (int)Game.TimeOfVictory - m * 60;
            _tmp.text = $"Your time is {m:D2}:{s:D2}";
        }

        private void OnDestroy()
        {
            Game.VictoryTriggered -= Game_VictoryTriggered;
        }
    }
}