using Surfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float moveDamping = 10f;
        [SerializeField] private float jumpForce = 4f;

        #region Dependencies
        private PlatformerInput _input;
        #endregion

        #region State
        private Rigidbody2D _selfBody;
        private Transform _selfTransform;
        private Surface _currentSurface;
        private Vector2 _inputDirection;
        private Vector2 _gravityDirection;
        private float _gravityForce;
        private bool _isGrounded;
        #endregion

        private void Awake()
        {
            DIContainer.Instance.Resolve(out _input);
            _gravityForce = Mathf.Abs(Physics2D.gravity.y);
            _gravityDirection = Physics2D.gravity.normalized;
            _selfTransform = transform;
            _selfBody = GetComponent<Rigidbody2D>();
            _selfBody.gravityScale = 0f; // Disable built-in gravity
        }

        private void OnEnable()
        {
            if (_input == null)
                return;
            
            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled += OnMove;
            _input.Player.Jump.performed += OnJump;
        }

        private void OnDisable()
        {
            if (_input == null)
                return;
            
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled -= OnMove;
            _input.Player.Jump.performed -= OnJump;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _inputDirection = ctx.ReadValue<Vector2>();
        }

        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (_isGrounded)
            {
                _selfBody.linearVelocity += (Vector2)_selfTransform.up * jumpForce;
                _isGrounded = false;
            }
        }

        private void FixedUpdate()
        {
            if (_currentSurface)
                _currentSurface.GetGravityModifier(_selfTransform.position, ref _gravityForce, ref _gravityDirection);

            ApplyGravity();
            ApplyMovement();
            AlignToGravity();
        }

        private void ApplyMovement()
        {
            var targetSpeed = _inputDirection.x * moveSpeed;
            Vector2 tangent = _selfTransform.right;
            var currentSpeed = Vector2.Dot(_selfBody.linearVelocity, tangent);
            var newSpeed = Mathf.Lerp(currentSpeed, targetSpeed, moveDamping * Time.fixedDeltaTime);
            var gravityVel = _selfBody.linearVelocity - currentSpeed * tangent;
            var moveVel = tangent * newSpeed;
            _selfBody.linearVelocity = moveVel + gravityVel;
        }

        private void ApplyGravity()
        {
            _selfBody.linearVelocity += _gravityDirection * (_gravityForce * Time.fixedDeltaTime);
        }
        
        private void AlignToGravity()
        {
            if (!_currentSurface)
                return;

            var up = -_gravityDirection;
            var targetAngle = Mathf.Atan2(up.y, up.x) * Mathf.Rad2Deg - 90f;
            var targetRot = Quaternion.AngleAxis(targetAngle, Vector3.forward);
            _selfTransform.rotation = Quaternion.Lerp(_selfTransform.rotation, targetRot, 15f * Time.fixedDeltaTime);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out Surface surface))
            {
                _currentSurface = surface;
                _isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (_currentSurface && other.gameObject == _currentSurface.Container)
                _isGrounded = false;
        }
    }
}
