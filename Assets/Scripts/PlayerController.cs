using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float gravity = 10.0f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownLimit = 65f;


    [Header("Spawn Settings")]
    [SerializeField] private Vector3 spawnPoint = new Vector3(18.0f, 1.5f, 4.5f);

    private static float originalHeight = 1.8f;

    private static float currentHeight = originalHeight;

    float currentSpeed;

    private float verticalRotation;

    [SerializeField] private Camera playerCamera;

    private Vector3 currentMovement = Vector3.zero;

    private CharacterController characterController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = this.GetComponent<CharacterController>();

        playerCamera = GetComponentInChildren<Camera>();
    }


    void Update()
    {
        characterController.height = currentHeight;

        if (characterController.isGrounded && Input.GetKey(KeyCode.Space))
        {
            currentMovement.y = jumpSpeed;
        }
        if (!characterController.isGrounded)
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }

        characterController.Move(currentMovement * Time.deltaTime);

        HandleMovement();

        HandleLook();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Killbox")
        {
            currentHeight = originalHeight;
            jumpSpeed = 5;
            characterController.enabled = false;
            gameObject.transform.position = spawnPoint;
            characterController.enabled = true;
        }

        if (other.gameObject.tag == "CheckPoint")
        {
            spawnPoint = gameObject.transform.position;
        }
    }


    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
        Vector3 horizontalMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        horizontalMovement = transform.rotation * horizontalMovement;

        currentMovement.x = horizontalMovement.x * currentSpeed;
        currentMovement.z = horizontalMovement.z * currentSpeed;

        characterController.Move(currentMovement * Time.deltaTime);
    }


    void HandleLook()
    {
        float mouseXrotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        this.transform.Rotate(0, mouseXrotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
