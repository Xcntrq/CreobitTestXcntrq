namespace GameLauncher
{
    using TMPro;
    using UnityEngine.UI;

    public class GameEntry
    {
        private readonly GameLauncher _gameLauncher;
        private readonly GameReference _gameReference;
        private readonly TextMeshProUGUI _tmp;
        private readonly Button _playBtn;
        private readonly Button _loadBtn;
        private readonly Button _unloadBtn;

        public GameEntry(GameLauncher gameLauncher, GameReference gameReference, TextMeshProUGUI tmp, Button playBtn, Button loadBtn, Button unloadBtn)
        {
            _gameLauncher = gameLauncher;
            _gameReference = gameReference;
            _tmp = tmp;
            _playBtn = playBtn;
            _loadBtn = loadBtn;
            _unloadBtn = unloadBtn;

            _tmp.gameObject.SetActive(false);
            _playBtn.gameObject.SetActive(false);
            _loadBtn.gameObject.SetActive(false);
            _unloadBtn.gameObject.SetActive(false);

            _tmp.text = gameReference.Name;
            _playBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
            _playBtn.onClick.AddListener(() => _gameLauncher.PlayGame(_gameReference));

            _loadBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Load";
            _loadBtn.onClick.AddListener(() => _gameLauncher.LoadGame(_gameReference));

            _unloadBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Unload";
            _unloadBtn.onClick.AddListener(() => _gameLauncher.UnloadGame(_gameReference));

            _gameLauncher.DataChanged += LauncherContent_DataChanged;
        }

        public void Unsub()
        {
            _gameLauncher.DataChanged -= LauncherContent_DataChanged;
        }

        private void LauncherContent_DataChanged()
        {
            _tmp.gameObject.SetActive(true);
            _playBtn.gameObject.SetActive(true);
            _loadBtn.gameObject.SetActive(true);
            _unloadBtn.gameObject.SetActive(true);

            bool hasSize = _gameLauncher.GetGameSize(_gameReference) > 0;

            _playBtn.interactable = !hasSize;
            _loadBtn.interactable = hasSize;
            _unloadBtn.interactable = !hasSize;
        }
    }
}