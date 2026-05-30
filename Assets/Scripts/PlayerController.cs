using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private float _verticalVelocity;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private bool _isOnLadder = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        HandleMovement();
    }

    void HandleMovement()
    {
        if (_isOnLadder)
        {
            Vector3 climbMove = new Vector3(0f, _moveInput.y * climbSpeed, 0f);
            _characterController.Move(climbMove * Time.deltaTime);
            _verticalVelocity = 0f;
            
            if (_characterController.isGrounded && _moveInput.y <= 0)
                _isOnLadder = false;
            
            if ((_characterController.collisionFlags & CollisionFlags.Above) != 0)
                _isOnLadder = false;
        }
        else
        {
            Vector3 move = transform.TransformDirection(new Vector3(_moveInput.x, 0f, _moveInput.y)) * walkSpeed;


            if (_characterController.isGrounded)
                _verticalVelocity = -2f;
            else
                _verticalVelocity += gravity * Time.deltaTime;

            move.y = _verticalVelocity;
            _characterController.Move(move * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
            _isOnLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
            _isOnLadder = false;
    }
}