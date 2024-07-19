namespace Clicker
{
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        private IPlayer Player => _player ??= (GetComponentInParent<IPlayer>() ?? _nullPlayer);
        private readonly IPlayer _nullPlayer = new NullPlayer();
        private IPlayer _player = null;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Player.RestartGame());
        }
    }
}