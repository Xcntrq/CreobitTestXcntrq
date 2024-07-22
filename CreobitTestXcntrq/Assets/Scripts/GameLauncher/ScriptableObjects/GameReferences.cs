namespace GameLauncher
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "GameReferences")]
    public class GameReferences : ScriptableObject
    {
        [SerializeField] private List<GameReference> _list;

        public IEnumerable<GameReference> List => _list;
    }
}