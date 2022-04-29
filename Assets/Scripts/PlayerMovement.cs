using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float forceMagnatude;
    [SerializeField] float maxVelocity;
    [SerializeField] float rotationSpeed;

    Rigidbody rb;
    Camera mainCamera;

    Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ProcessInput();

        KeepPlayerOnScreen();

        RotateToVelocity();
    }

    void FixedUpdate()
    {
        if (moveDirection == Vector3.zero) { return; }

        rb.AddForce(moveDirection * forceMagnatude * Time.deltaTime, ForceMode.Force);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);

            moveDirection = transform.position - worldPos;
            moveDirection.z = 0f;
            moveDirection.Normalize();
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void KeepPlayerOnScreen()
    {
        Vector3 newPos = transform.position;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPosition.x > 1f)
        {
            newPos.x = -newPos.x + 0.1f;
        }

        else if (viewportPosition.x < 0f)
        {
            newPos.x = -newPos.x - 0.1f;
        }

        if (viewportPosition.y > 1f)
        {
            newPos.y = -newPos.y + 0.1f;
        }

        else if (viewportPosition.y < 0f)
        {
            newPos.y = -newPos.y - 0.1f;
        }

        transform.position = newPos;
    }

    void RotateToVelocity()
    {
        if (rb.velocity == Vector3.zero) { return; }

        Quaternion targetRotation = Quaternion.LookRotation(rb.velocity, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}