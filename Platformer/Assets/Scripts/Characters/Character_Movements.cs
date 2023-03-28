using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;
using System;

public class Character_Movements : MonoBehaviour
{

    [Header("SetUp")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform feet_Pivot;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferTimeCounter;
    [Header("Movement")]
    [SerializeField] Vector3 _CurrentMovement;
    [Range (0,500)] [SerializeField] private float speed = 20.0f;
    [SerializeField] float initialSpeed;
    [Range (0,500)] [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isSprinting;
    [SerializeField] const float maxDistance = 10f;
    [SerializeField] const float minJumpDistance = 0.5f;
    [Header("Coyote Time Setup")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimerCounter;

    private void Awake()
    {
        rigidbody ??= GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
        }

        feet_Pivot ??= GetComponent<Transform>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(feet_Pivot)} is null");
        }

        playerCamera ??= GetComponent<Transform>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(playerCamera)} is null");
        }

        isJumping = false;
        isSprinting = false;
        initialSpeed = speed;
    }

    private void FixedUpdate()
    {
        //Debug.Log(isGrounded());

        if (coyoteTimerCounter > 0f && jumpBufferTimeCounter > 0f && !isJumping) 
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            //Debug.Log("Jump");
        }

        if (isGrounded())
        {
            coyoteTimerCounter = coyoteTime;
        }
        else
        {
            coyoteTimerCounter -= Time.deltaTime;
        }

        rigidbody.velocity = _CurrentMovement.normalized * speed + Vector3.up * rigidbody.velocity.y;

        if (isSprinting)
        {
            speed = initialSpeed * 2;
        }
        else 
        {
            speed = initialSpeed;
        }
    }

    public void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();

        float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        _CurrentMovement = Quaternion.Euler(0f, targetAngle,0f) * Vector3.forward;
    }

    public void OnJump(InputValue input)
    {
        isJumping = true;
        if (input.isPressed)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferTimeCounter -= Time.deltaTime;
            coyoteTimerCounter = 0.0f;
        }

        if (input.isPressed && rigidbody.velocity.y > 0f)
        {
            rigidbody.velocity = _CurrentMovement * speed + Vector3.up * rigidbody.velocity.y * 0.5f;
            coyoteTimerCounter = 0f;
        }

        CancelInvoke(nameof(CancelJump));
        Invoke(nameof(CancelJump), jumpBufferTime);
    }

    private bool isGrounded() 
    {
        RaycastHit hit;
        return Physics.Raycast(feet_Pivot.position, Vector3.down, out hit, maxDistance) && hit.distance <= minJumpDistance;
    }

    private void CancelJump() 
    {
        isJumping = false;
    }

    public void OnSprint(InputValue input) 
    {
        Debug.Log(input.isPressed);
        isSprinting = input.isPressed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(feet_Pivot.position,feet_Pivot.position + Vector3.down * minJumpDistance);
    }
}
