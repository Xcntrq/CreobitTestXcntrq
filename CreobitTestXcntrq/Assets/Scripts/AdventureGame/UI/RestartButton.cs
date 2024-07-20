namespace Adventure
{
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        private IGame Game => _game ??= (GetComponentInParent<IGame>() ?? _nullGame);
        private readonly IGame _nullGame = new NullGame();
        private IGame _game = null;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Game.Restart());
        }
    }
}