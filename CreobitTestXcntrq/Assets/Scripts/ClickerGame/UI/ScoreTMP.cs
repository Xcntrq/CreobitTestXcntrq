namespace ClickerGame
{
    using TMPro;
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreTMP : MonoBehaviour
    {
        [SerializeField] private GameObject[] _effects;

        private IPlayer Player => _player ??= (GetComponentInParent<IPlayer>() ?? _nullPlayer);
        private readonly IPlayer _nullPlayer = new NullPlayer();
        private IPlayer _player = null;

        private TextMeshProUGUI _tmp;
        private RectTransform _rt;

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _tmp = GetComponent<TextMeshProUGUI>();
            Player.DataChanged += Player_DataChanged;
            Player.PointAdded += Player_PointAdded;
        }

        private void Player_DataChanged()
        {
            _tmp.text = Player.Score.ToString();
        }

        private void Player_PointAdded()
        {
            if (_effects.Length == 0)
            {
                return;
            }

            Vector3[] corners = new Vector3[4];
            _rt.GetWorldCorners(corners);
            float rot = Random.Range(0f, 360f);
            float x = Random.Range(corners[0].x, corners[2].x);
            float y = Random.Range(corners[0].y, corners[2].y);
            int i = Random.Range(0, _effects.Length);
            Instantiate(_effects[i], new(x, y), Quaternion.Euler(0, 0, rot));
        }

        private void OnDestroy()
        {
            Player.DataChanged -= Player_DataChanged;
            Player.PointAdded -= Player_PointAdded;
        }
    }
}