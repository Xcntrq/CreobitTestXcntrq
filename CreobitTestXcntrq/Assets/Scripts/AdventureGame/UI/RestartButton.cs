namespace AdventureGame
{
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        private ILevel Level => _level ??= (GetComponentInParent<ILevel>() ?? _nullLevel);
        private readonly ILevel _nullLevel = new NullLevel();
        private ILevel _level = null;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Level.ResetLevel());
        }
    }
}