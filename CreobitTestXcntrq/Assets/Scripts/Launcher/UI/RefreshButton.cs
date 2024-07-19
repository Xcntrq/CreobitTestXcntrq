namespace Launcher
{
    using UnityEngine;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class RefreshButton : MonoBehaviour
    {
        [SerializeField] private GameLauncher _gameLauncher;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => _gameLauncher.Refresh());
        }
    }
}