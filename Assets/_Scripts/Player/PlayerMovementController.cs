using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    Camera mainCamera;
    
    Vector2 inputDir;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
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

        transform.Translate(moveDir * speed * Time.deltaTime);
    }
}
