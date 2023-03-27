using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEngine;

public class Character_Movements : MonoBehaviour
{

    [Header("SetUp")]
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] GameObject feet_Pivot;

    [Header("Movement")]
    [SerializeField] Vector3 _CurrentMovement;
    [Range (0,100)] [SerializeField] float speed = 20.0f;
    [Range (0,100)] [SerializeField] float jumpForce = 20.0f;
    [SerializeField] bool canJunp;

    [Header("SetUp")]
    [SerializeField] const float maxDistance = 0f;
    [SerializeField] const float minJumpDistance = 0.5f;

    private void Awake()
    {
        rigidbody ??= GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(rigidbody)} is null");
        }

        feet_Pivot ??= GetComponent<GameObject>();
        if (!feet_Pivot)
        {
            Debug.LogError(message: $"{name}: (logError){nameof(feet_Pivot)} is null");
        }

        canJunp = true;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * _CurrentMovement);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (canJunp && Physics.Raycast(feet_Pivot.transform.position,Vector3.down,out hit,maxDistance) && hit.distance <= minJumpDistance) 
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJunp = false;
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
        canJunp = true;   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
    }
}
