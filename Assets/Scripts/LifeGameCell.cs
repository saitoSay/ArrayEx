using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LifeGameCell : MonoBehaviour
{
    [SerializeField] private bool _isAlive;
    [SerializeField] private Image _image;
    public bool IsAlive
    {
        get { return _isAlive; }
        set 
        { 
            _isAlive = value;
            OnCellColorChanged();
        }
    }
    private void OnCellColorChanged()
    {
        if (_isAlive)
        {
            _image.color = Color.black;
        }
        else
        {
            _image.color = Color.white;
        }
    }
}
