using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Camera mainCamera;

    Rigidbody rb;
    
    Vector2 inputDir;
    public float movementSpeed;
    private Vector3 velocity;
    
    public bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    public void OnMovement(InputValue input)
    {
        inputDir = input.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0.0f;
        forward.Normalize();
        right.y = 0.0f;
        right.Normalize();

        Vector3 moveDir = forward * inputDir.y + right * inputDir.x;
        velocity = moveDir * movementSpeed * Time.deltaTime;

        rb.velocity = velocity;
    }
}
