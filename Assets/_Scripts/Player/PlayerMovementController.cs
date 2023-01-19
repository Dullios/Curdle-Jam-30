using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody rb;

    public Transform followTransform;

    public bool isGrounded;

    Vector2 inputDir;
    public float movementSpeed;
    private Vector3 velocity;

    Vector2 mouseDir;
    public float lookSpeed;

    Vector3 jumpVelocity;
    [Header("Jump Equation")]
    public float jumpForce;
    public float gravityMultiplier;

    public float apexHeight;
    public float distToHeight;
    public float jumpSpeed;

    public float initialVelocity;
    public float gravity;

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

    public void OnLook(InputValue input)
    {
        mouseDir = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        if(input.isPressed && isGrounded)
        {
            initialVelocity = 2.0f * apexHeight * jumpSpeed / distToHeight;
            gravity = -2 * apexHeight * (jumpSpeed * jumpSpeed) / (distToHeight * distToHeight);

            jumpVelocity.y = initialVelocity;

            isGrounded = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        CameraMovement();

        if (!isGrounded)
            velocity += jumpVelocity;

        ApplyGravity();
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

    void CameraMovement()
    {
        // Horizontal rotation
        followTransform.rotation *= Quaternion.AngleAxis(mouseDir.x * lookSpeed, Vector3.up);

        // Vertical rotation
        followTransform.rotation *= Quaternion.AngleAxis(mouseDir.y * lookSpeed, Vector3.right);

        Vector3 angles = followTransform.localEulerAngles;
        angles.z = 0;

        float angle = followTransform.localEulerAngles.x;

        // Clamp vertical rotation
        if (angle > 180 && angle < 340)
            angles.x = 340;
        else if (angle < 180 && angle > 40)
            angles.x = 40;

        followTransform.localEulerAngles = angles;

        if (inputDir.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, followTransform.rotation.eulerAngles.y, 0);
            followTransform.localEulerAngles = new Vector3(angles.x, 0, 0);
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            float prevVelY = jumpVelocity.y;
            float nextVelY = jumpVelocity.y + ((jumpVelocity.y <= 0 || !isGrounded ? gravity * gravityMultiplier : gravity) * Time.deltaTime);
            float avgVelY = (prevVelY + nextVelY) * 0.5f;

            jumpVelocity.y = avgVelY;
        }
        else
        {
            jumpVelocity = Vector3.zero;
        }
    }
}
