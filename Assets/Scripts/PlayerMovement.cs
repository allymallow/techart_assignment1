using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float gravity = -9.8f;
    

    private InputAction _inputAction;
    private Vector2 _moveInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _moveDirection;
    private Quaternion _targetRotation;
    private Vector3 _velocity;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

// Update is called once per frame
    void Update()
    {
        CalculateMovement();
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void CalculateMovement()
    {
        _camForward = playerCamera.transform.forward;
        _camRight = playerCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }

        //Calculate gravity
        _velocity = Vector3.up * _velocity.y + _moveDirection * moveSpeed;
        _velocity.y += gravity * Time.deltaTime;
    }
    
    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

}

