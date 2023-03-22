using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector2 horDirection;
    private bool isJumpinginput;
    [Range(0, 500)][SerializeField] private float speed = 10;
    [Range(0, 100)][SerializeField] private float jumpForce = 10;
    Rigidbody rigidbody;


    private void Awake()
    {
            rigidbody ??= GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
        }
    }

    void Update()
    {
        horDirection = ReadHorDirection();
        isJumpinginput = ReadJumpInput();
    }

    private void FixedUpdate()
    {
        if (rigidbody)
        {
            rigidbody.velocity = GetVelocity();
        }

        if (isJumpinginput && rigidbody) 
        {
            rigidbody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            isJumpinginput = false;
        }
    }

    /// <summary>
    /// Return the value for the jump input
    /// </summary>
    /// <returns>True if the button for jump is presed</returns>
    private bool ReadJumpInput() 
    {
        return Input.GetButtonUp("Jump");
    }

    /// <summary>
    /// Calculate The RigidBody Velocity
    /// </summary>
    /// <returns>A Vector3</returns>
    private Vector3 GetVelocity()
    {
        return new Vector3(horDirection.x, 0, horDirection.y) * speed + Vector3.up * rigidbody.velocity.y;
    }

    /// <summary>
    /// Return the value for the Horizontal && vertical axis inputs
    /// </summary>
    /// <returns> A Vector 2</returns>
    private Vector2 ReadHorDirection()
    {
        var result = new Vector2
        {
            x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical")
        };

        return result;
    }
}
