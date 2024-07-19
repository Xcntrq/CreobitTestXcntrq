namespace Namespace
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
    using System.Linq;

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
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    UnblockInput($"Error launching {gameReference.Name}");
                    return;
                }
            };
        }

        public void LoadGame(GameReference gameReference)
        {
            BlockInput($"Downloading {gameReference.Name}...");

            Addressables.LoadAssetAsync<SceneInstance>(gameReference.Scene).Completed += (AsyncOperationHandle<SceneInstance> handle) =>
            {
                UnblockInput();
                ReleaseHandle(handle);
                StartCoroutine(UpdateSizes());
            };
        }

        public void UnloadGame(GameReference gameReference)
        {
            BlockInput($"Unloading {gameReference.Name}...");

            Addressables.ClearDependencyCacheAsync(gameReference.Scene, false).Completed += (AsyncOperationHandle<bool> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    ReleaseHandle(handle);
                    UnblockInput($"Error unloading {gameReference.Name}");
                    return;
                }

                UnblockInput();
                ReleaseHandle(handle);
                StartCoroutine(UpdateSizes());
            };
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
            BlockInput("Loading...");
            ReleaseHandle(_gameReferencesHandle);

            _gameReferencesHandle = Addressables.LoadAssetAsync<GameReferences>(_gameReferences);
            yield return _gameReferencesHandle;

            DestroyChildren();
            if (_gameReferencesHandle.Status == AsyncOperationStatus.Failed)
            {
                UnblockInput("Connection error");
                yield break;
            }

            foreach (GameEntry gameEntry in _gameEntries ?? Enumerable.Empty<GameEntry>())
            {
                gameEntry?.Unsub();
            }

            _gameEntries?.Clear();
            _gameEntries ??= new();
            foreach (GameReference gameReference in _gameReferencesHandle.Result.List)
            {
                TextMeshProUGUI tmp = Instantiate(_textPf, _col1);
                Button btn1 = Instantiate(_buttonPf, _col2);
                Button btn2 = Instantiate(_buttonPf, _col3);
                Button btn3 = Instantiate(_buttonPf, _col4);
                GameEntry gameEntry = new(this, gameReference, tmp, btn1, btn2, btn3);
                _gameEntries.Add(gameEntry);
            }

            UnblockInput();
            StartCoroutine(UpdateSizes());
        }

        private IEnumerator UpdateSizes()
        {
            BlockInput("Loading...");

            _sizes?.Clear();
            _sizes ??= new();
            foreach (GameReference gameReference in _gameReferencesHandle.Result.List)
            {
                AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(gameReference.Scene);
                yield return getDownloadSize;

                if (getDownloadSize.Status == AsyncOperationStatus.Failed)
                {
                    ReleaseHandle(getDownloadSize);
                    UnblockInput("Connection error");
                    yield break;
                }

                _sizes[gameReference] = getDownloadSize.Result;
                ReleaseHandle(getDownloadSize);
                DataChanged?.Invoke();
            }

            UnblockInput();
        }

        private void OnDestroy()
        {
            ReleaseHandle(_gameReferencesHandle);
        }

        private void DestroyChildren()
        {
            List<GameObject> children = new();
            foreach (Transform t in _col1.transform)
            {
                children.Add(t.gameObject);
            }

            foreach (Transform t in _col2.transform)
            {
                children.Add(t.gameObject);
            }

            foreach (Transform t in _col3.transform)
            {
                children.Add(t.gameObject);
            }

            foreach (Transform t in _col4.transform)
            {
                children.Add(t.gameObject);
            }

            for (int i = children.Count - 1; i >= 0; i--)
            {
                Destroy(children[i]);
            }
        }

        private void BlockInput(string text)
        {
            _inputBlocker.SetActive(true);
            _statusTMP.text = text;
            _statusTMP.gameObject.SetActive(true);
        }

        private void UnblockInput(string text)
        {
            _statusTMP.text = text;
            _statusTMP.gameObject.SetActive(true);
            _inputBlocker.SetActive(false);
        }

        private void UnblockInput()
        {
            _statusTMP.gameObject.SetActive(false);
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
    }
}