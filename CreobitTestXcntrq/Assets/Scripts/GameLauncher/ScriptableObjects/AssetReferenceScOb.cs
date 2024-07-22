namespace GameLauncher
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class AssetReferenceScOb : AssetReferenceT<ScriptableObject>
    {
        public AssetReferenceScOb(string guid) : base(guid) { }
    }
}