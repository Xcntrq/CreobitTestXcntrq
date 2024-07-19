namespace Launcher
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class ReturnToMainMenuButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MainMenu");
            });
        }
    }
}