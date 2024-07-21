namespace AdventureGame
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;

        private ILevel Level => _level ??= (GetComponentInParent<ILevel>() ?? _nullLevel);
        private readonly ILevel _nullLevel = new NullLevel();
        private ILevel _level = null;

        private Rigidbody _rb;
        private Animator _animator;
        private bool _isStopped;

        public void Stop()
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _animator.SetFloat("VelX", 0f);
            _animator.SetFloat("VelZ", 0f);
            _isStopped = true;
        }

        private void Awake()
        {
            _isStopped = false;
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            Level.RegisterPlayer(transform);
        }

        private void FixedUpdate()
        {
            if (_isStopped) return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            _animator.SetFloat("VelX", h);
            _animator.SetFloat("VelZ", v);

            _rb.velocity = transform.rotation * new Vector3(0, _rb.velocity.y, v * _moveSpeed);
            transform.Rotate(0, h * _turnSpeed * Time.deltaTime, 0);
        }
    }
}