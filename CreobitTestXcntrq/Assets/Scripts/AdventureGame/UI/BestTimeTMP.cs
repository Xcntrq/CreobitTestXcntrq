namespace AdventureGame
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BestTimeTMP : MonoBehaviour
    {
        private ILevel Level => _level ??= (GetComponentInParent<ILevel>() ?? _nullLevel);
        private readonly ILevel _nullLevel = new NullLevel();
        private ILevel _level = null;

        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            Level.VictoryTriggered += Game_VictoryTriggered;
        }

        private void Game_VictoryTriggered()
        {
            int m = (int)(Level.LastRecord / 60f);
            int s = (int)Level.LastRecord - m * 60;
            _tmp.text = Level.LastRecord > 0 ? $"Previously recorded<br>best time is {m:D2}:{s:D2}" : "Previously recorded<br>best time was not found";
        }

        private void OnDestroy()
        {
            Level.VictoryTriggered -= Game_VictoryTriggered;
        }
    }
}