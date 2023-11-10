using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private InputAction wasd;
    [SerializeField] private CharacterController player;


    private void OnEnable()
    {
        wasd.Enable();
    }
    private void OnDisable()
    {
        wasd.Disable();
    }

    void Start()
    {

    }


    void Update()
    {
        // for 3D movement
        player.transform.position += wasd.ReadValue<Vector3>() * 20 * Time.deltaTime;

        Debug.Log(wasd.ReadValue<Vector3>().ToString());
    }


}
