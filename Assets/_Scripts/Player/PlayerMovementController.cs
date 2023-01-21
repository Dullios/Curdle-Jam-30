using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody rb;

    [Header("Game Objects")]
    public Transform followTransform;
    public GameObject body;
    public GameObject armor;
    public GameObject shell;

    public bool isGrounded;
    
    [Header("Ground Check Ray")]
    public LayerMask layerMask;
    Vector3 origin;
    RaycastHit hit;
    public float distance;

    Vector2 inputDir;
    [Header("Movement")]
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
        Cursor.lockState = CursorLockMode.Confined;

        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        shell.SetActive(false);

        isGrounded = true;
    }

    private void RollUp(bool hasRolled)
    {
        if(hasRolled)
        {
            body.SetActive(false);
            armor.SetActive(false);
            shell.SetActive(true);
        }
        else
        {
            body.SetActive(true);
            armor.SetActive(true);
            shell.SetActive(false);
        }
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
            RollUp(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CameraMovement();

        origin = transform.position;
        origin.y += 0.299f;
        // Ground Check Raycast
        if(velocity.y < 0 && Physics.SphereCast(origin, 0.01f, Vector3.down, out hit, distance, layerMask))
        {
            Debug.DrawRay(origin, Vector3.down * distance, Color.cyan);
            isGrounded = true;
            RollUp(false);
        }

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0.0f;
        forward.Normalize();
        right.y = 0.0f;
        right.Normalize();

        Vector3 moveDir = forward * inputDir.y + right * inputDir.x;
        velocity = moveDir * movementSpeed * Time.deltaTime;
        
        if (!isGrounded)
            velocity += jumpVelocity;

        rb.velocity = velocity;
        
        ApplyGravity();
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

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //        //RollUp(false);
    //    }
    //}
}
