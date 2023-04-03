using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefab_Intantiator : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    
    void Start()
    {
        var newChild = Instantiate(prefab,transform);
    }
}
