namespace Namespace
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "GameReference")]
    public class GameReference : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private AssetReference _scene;

        public string ID => _id;
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