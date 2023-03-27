using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;

public class Character_Movements : MonoBehaviour
{
    [Range (0,100)] [SerializeField] float speed = 20.0f;
    [Range (0,100)] [SerializeField] float jumpForce = 20.0f;
    [SerializeField] Vector3 _CurrentMovement;
    [SerializeField] Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * _CurrentMovement);
    }

    public void OnMove(InputValue input)
    {
        var movement = input.Get<Vector2>();
        _CurrentMovement.x = movement.x;
        _CurrentMovement.z = movement.y;
    }

    public void OnJump()
    {
        Debug.Log("Jump");
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
