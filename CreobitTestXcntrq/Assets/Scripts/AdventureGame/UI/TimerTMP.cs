namespace AdventureGame
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TimerTMP : MonoBehaviour
    {
        private ILevel Level => _level ??= (GetComponentInParent<ILevel>() ?? _nullLevel);
        private readonly ILevel _nullLevel = new NullLevel();
        private ILevel _level = null;

        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            Level.TimeTicked += Game_TimeTicked;
        }

        private void Game_TimeTicked()
        {
            int m = (int)(Level.TimeElapsed / 60f);
            int s = (int)Level.TimeElapsed - m * 60;
            _tmp.text = $"{m:D2}:{s:D2}";
        }

        private void OnDestroy()
        {
            Level.TimeTicked -= Game_TimeTicked;
        }
    }
}