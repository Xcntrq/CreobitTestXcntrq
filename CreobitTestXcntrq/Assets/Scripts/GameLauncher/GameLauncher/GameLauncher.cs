namespace GameLauncher
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;
    using System.Collections;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.AddressableAssets;
    using TMPro;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    [DisallowMultipleComponent]
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] private AssetReferenceScOb _gameReferences;
        [SerializeField] private TextMeshProUGUI _statusTMP;
        [SerializeField] private GameObject _inputBlocker;
        [SerializeField] private Transform _col1;
        [SerializeField] private Transform _col2;
        [SerializeField] private Transform _col3;
        [SerializeField] private Transform _col4;
        [SerializeField] private Button _buttonPf;
        [SerializeField] private TextMeshProUGUI _textPf;

        private AsyncOperationHandle<GameReferences> _gameReferencesHandle;
        private Dictionary<GameReference, long> _sizes;
        private List<GameEntry> _gameEntries;

        public event Action DataChanged;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_gameReferences.editorAsset is not GameReferences)
            {
                _gameReferences = null;
            }
        }
#endif

        public long GetGameSize(GameReference gameReference) => (_sizes ??= new()).ContainsKey(gameReference) ? _sizes[gameReference] : 1;

        public void PlayGame(GameReference gameReference)
        {
            BlockInput($"Launching {gameReference.Name}...");
            Addressables.LoadSceneAsync(gameReference.Scene).Completed += (AsyncOperationHandle<SceneInstance> handle) =>
            {
                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    UnblockInput($"Error launching {gameReference.Name}");
                    ReleaseHandle(handle);
                }
            };
        }

        public void LoadGame(GameReference gameReference)
        {
            BlockInput($"Downloading {gameReference.Name}...");
            Addressables.DownloadDependenciesAsync(gameReference.Scene, false).Completed += (AsyncOperationHandle handle) =>
            {
                string msg = (handle.Status != AsyncOperationStatus.Succeeded) ? $"Error downloading {gameReference.Name}" : string.Empty;
                float delay = (handle.Status != AsyncOperationStatus.Succeeded) ? 0.5f : 0f;
                ReleaseHandle(handle);
                UnblockInput(msg);
                StartCoroutine(UpdateSizes(delay));
            };
        }

        public void UnloadGame(GameReference gameReference)
        {
            BlockInput($"Unloading {gameReference.Name}...");
            Addressables.ClearDependencyCacheAsync(gameReference.Scene, false).Completed += (AsyncOperationHandle<bool> handle) =>
            {
                string msg = (handle.Status != AsyncOperationStatus.Succeeded) ? $"Error unloading {gameReference.Name}" : string.Empty;
                float delay = (handle.Status != AsyncOperationStatus.Succeeded) ? 0.5f : 0f;
                ReleaseHandle(handle);
                UnblockInput(msg);
                StartCoroutine(UpdateSizes(delay));
            };
        }

        public void Refresh() => StartCoroutine(ReloadAfterUpdate());

        private IEnumerator ReloadAfterUpdate()
        {
            BlockInput("Refreshing...");
            yield return Addressables.UpdateCatalogs(null, true);
            SceneManager.LoadScene("MainMenu");
        }

        private void Awake()
        {
            bool isReadyForWork = _gameReferences != null;
            isReadyForWork &= _inputBlocker != null;
            isReadyForWork &= _statusTMP != null;
            isReadyForWork &= _col1 != null;
            isReadyForWork &= _col2 != null;
            isReadyForWork &= _col3 != null;
            isReadyForWork &= _col4 != null;
            isReadyForWork &= _textPf != null;
            isReadyForWork &= _buttonPf != null;
            if (!isReadyForWork)
            {
                Debug.LogWarning($"Object not set up! {gameObject.name}");
                return;
            }

            StartCoroutine(LoadReferences());
        }

        private IEnumerator LoadReferences()
        {
            BlockInput("Checking for games...");
            ReleaseHandle(_gameReferencesHandle);

            _gameReferencesHandle = Addressables.LoadAssetAsync<GameReferences>(_gameReferences);
            yield return _gameReferencesHandle;

            if (_gameReferencesHandle.Status != AsyncOperationStatus.Succeeded)
            {
                UnblockInput("Connection error");
                yield break;
            }

            _gameEntries = new();
            foreach (GameReference gameReference in _gameReferencesHandle.Result.List)
            {
                TextMeshProUGUI tmp = Instantiate(_textPf, _col1);
                Button btn1 = Instantiate(_buttonPf, _col2);
                Button btn2 = Instantiate(_buttonPf, _col3);
                Button btn3 = Instantiate(_buttonPf, _col4);
                GameEntry gameEntry = new(this, gameReference, tmp, btn1, btn2, btn3);
                _gameEntries.Add(gameEntry);
            }

            UnblockInput(string.Empty);
            StartCoroutine(UpdateSizes());
        }

        private IEnumerator UpdateSizes(float delay = 0f)
        {
            BlockInput(null);
            yield return new WaitForSeconds(delay);
            BlockInput("Refreshing...");

            _sizes?.Clear();
            _sizes ??= new();
            foreach (GameReference gameReference in _gameReferencesHandle.Result.List)
            {
                AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(gameReference.Scene);
                yield return getDownloadSize;

                if (getDownloadSize.Status != AsyncOperationStatus.Succeeded)
                {
                    UnblockInput("Connection error");
                    ReleaseHandle(getDownloadSize);
                    yield break;
                }

                _sizes[gameReference] = getDownloadSize.Result;
                ReleaseHandle(getDownloadSize);
                DataChanged?.Invoke();
            }

            UnblockInput(string.Empty);
        }

        private void OnDestroy()
        {
            ReleaseHandle(_gameReferencesHandle);
        }

        private void BlockInput(string text)
        {
            _inputBlocker.SetActive(true);
            _statusTMP.text = string.IsNullOrEmpty(text) ? _statusTMP.text : text;
        }

        private void UnblockInput(string text)
        {
            _statusTMP.text = text;
            _inputBlocker.SetActive(false);
        }

        private void ReleaseHandle(AsyncOperationHandle<GameReferences> handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        private void ReleaseHandle(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        private void ReleaseHandle(AsyncOperationHandle<long> handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        private void ReleaseHandle(AsyncOperationHandle<bool> handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }

        private void ReleaseHandle(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }
}