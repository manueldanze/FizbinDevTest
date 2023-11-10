using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private InputAction _movePlayer;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private Camera _mainCamera;

    [Range(20f, 80f)]
    [SerializeField] 
    private float _moveSpeed;

    void Update()
    {
        MovePlayer();
        RotatePlayerTowardsMouse();
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
}
