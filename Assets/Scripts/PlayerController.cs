using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    private InputAction _movePlayer;

    [SerializeField] 
    private CharacterController _playerController;

    [SerializeField] 
    private Camera _mainCamera;

    [Range(0f, 150f)]
    [SerializeField] 
    private float _moveSpeed;

    [Range(0f, 150f)]
    [SerializeField] 
    private float _followSpeed;

    [SerializeField] 
    private Vector3 _cameraOffset;

    private void Awake()
    {
        _moveSpeed = 50;
        _followSpeed = 50;
        _cameraOffset = new Vector3(0f, 100f, -150f);
    }

    void Update()
    {
        MovePlayer();
        RotatePlayerTowardsMouse();
        FollowPlayerWithCamera();
    }


    private void OnEnable()
    {
        _movePlayer.Enable();
    }
    private void OnDisable()
    {
        _movePlayer.Disable();
    }

    void MovePlayer()
    {
        // Move player according to input value
        _playerController.Move(_movePlayer.ReadValue<Vector3>() * _moveSpeed * Time.deltaTime);
    }

    void RotatePlayerTowardsMouse()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // Convert mouse position to a point in the world space
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            // Calculate the direction to the target position
            Vector3 direction = targetPosition - _playerController.transform.position;
            direction.y = 0f;

            // Rotate the player to face the mouse position
            if (direction.magnitude > 0.1f)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                _playerController.transform.rotation = Quaternion.RotateTowards(
                    _playerController.transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }
    }

    void FollowPlayerWithCamera()
    {
        Vector3 playerPosition = _playerController.transform.position;

        // Offset the camera position
        Vector3 targetPosition = playerPosition + _cameraOffset;

        // Adjust the camera's position smoothly to follow the player
        Vector3 newPosition = Vector3.Lerp(_mainCamera.transform.position, targetPosition, _followSpeed * Time.deltaTime);
        _mainCamera.transform.position = newPosition;

        // Make sure the camera is always looking at the player
        _mainCamera.transform.LookAt(playerPosition);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Player collided with: " + hit.gameObject.name);
    }

}
