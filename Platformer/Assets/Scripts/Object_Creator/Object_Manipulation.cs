using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Manipulation : MonoBehaviour
{
    private GameObject _newChild;

    [ContextMenu(itemName: "Create Object")]
    private void CreateObject()
    {
        _newChild = new GameObject(name: "MyChild");
        _newChild.transform.SetParent(transform);
    }

    [ContextMenu(itemName: "Destroy Object")]
    private void DestroyObject()
    {
        Destroy(_newChild);
    }

    [ContextMenu(itemName: "Destroy Object", isValidateFunction: true)]
    private bool ValidateDestroyObject()
    {
        return _newChild != null;
    }
}
