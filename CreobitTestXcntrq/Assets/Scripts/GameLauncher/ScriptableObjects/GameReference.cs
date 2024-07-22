namespace GameLauncher
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(fileName = "GameReference")]
    public class GameReference : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private AssetReference _scene;

        public string Name => _name;
        public AssetReference Scene => _scene;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_scene.editorAsset is not SceneAsset)
            {
                _scene = null;
            }
        }
#endif
    }
}