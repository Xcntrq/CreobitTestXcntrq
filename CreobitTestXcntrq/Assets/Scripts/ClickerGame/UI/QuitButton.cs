namespace Clicker
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class QuitButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        }
    }
}