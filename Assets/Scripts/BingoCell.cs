using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoCell : MonoBehaviour
{
    [SerializeField] private int _bingoNum = 0;
    [SerializeField] private Text _view = null;
    [SerializeField] private Image _image;
    private bool _openFrag = false;
    public bool OpenFrag
    {
        get { return _openFrag; }
        set
        {
            _openFrag = value;
            OnCellColorChanged();
        }
    }
    public int BingoNum
    {
        get => _bingoNum;
        set
        {
            _bingoNum = value;
            OnCellStateChanged();
        }
    }

    private void OnValidate()
    {
        OnCellStateChanged();
        OnCellColorChanged();
    }
    private void OnCellColorChanged()
    {
        if (_openFrag)
        {
            _image.color = Color.gray;
        }
        else
        {
            _image.color = Color.white;
        }
    }
    private void OnCellStateChanged()
    {
        if (_view == null) { return; }

        if (_bingoNum == 0)
        {
            _view.text = "FREE";
        }
        else
        {
            _view.text = _bingoNum.ToString();
            _view.color = Color.blue;
        }
    }
}
