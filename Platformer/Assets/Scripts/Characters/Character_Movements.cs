using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;
using System;

public class Character_Movements : MonoBehaviour
{

    [Header("SetUp")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Transform feet_Pivot;
    [SerializeField] float jumpBufferTime;
    [Header("Movement")]
    [SerializeField] Vector3 _CurrentMovement;
    [Range (0,500)] [SerializeField] float speed = 20.0f;
    [SerializeField] float initialSpeed;
    [Range (0,500)] [SerializeField] float jumpForce = 20.0f;
    [SerializeField] bool canJump;
    [SerializeField] bool isSprinting;
    [SerializeField] const float maxDistance = 10f;
    [SerializeField] const float minJumpDistance = 0.5f;

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

        canJump = false;
        isSprinting = false;
        initialSpeed = speed;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (canJump && Physics.Raycast(feet_Pivot.position,Vector3.down,out hit,maxDistance) && hit.distance <= minJumpDistance) 
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            Debug.Log("Jump");
        }

        rigidbody.velocity = _CurrentMovement * speed + Vector3.up * rigidbody.velocity.y;

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
        _CurrentMovement.x = movement.x;
        _CurrentMovement.z = movement.y;
    }

    public void OnJump()
    {
        canJump = true;
        CancelInvoke(nameof(CancelJump));
        Invoke(nameof(CancelJump), jumpBufferTime);
    }

    private void CancelJump() 
    {
        canJump = false;
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
