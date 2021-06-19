using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    Vector3 _mousePos;
    Vector3 _mousePos2;
    int count = 0;
    private void Start()
    {
        _mousePos = new Vector3(0, 0, 0);
        _mousePos2 = new Vector3(1, 1, 0);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && count % 2 == 0)
        {
            _mousePos = Input.mousePosition;
            Debug.Log(_mousePos.x + " " + _mousePos.y + " " + _mousePos.z);
            count++;
        }
        else if (Input.GetMouseButtonDown(0) && count % 2 == 1)
        {
            _mousePos2 = Input.mousePosition;
            Debug.Log(_mousePos2.x + " " + _mousePos2.y + " " + _mousePos2.z);
            count++;
        }
    }

}
