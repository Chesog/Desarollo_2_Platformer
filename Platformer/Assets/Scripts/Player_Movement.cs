using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Vector2 _horDirection;
    [SerializeField] private float Speed;

    void Start()
    {
        
    }

   

    void Update()
    {
        _horDirection = ReadHorDirection();
        transform.position += new Vector3(_horDirection.x,0,_horDirection.y) * Speed * Time.deltaTime;
    }

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
