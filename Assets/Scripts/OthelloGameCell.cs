using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OthelloCellState
{
    None = 0,
    Black = 1,
    White = 2,
    Blue = 3
}

public class OthelloGameCell : MonoBehaviour
{
    [SerializeField] private OthelloCellState _cellState = OthelloCellState.None;
    [SerializeField] private Image _stoneImage = null;
    [SerializeField] private GameObject _isSetImage = null;
    [SerializeField] private Button _button = null;
    private bool _isSetStone = false;
    public bool IsSetStone 
    { 
        get => _isSetStone;
        set
        {
            _isSetStone = value;
            OnSetGuideChanged();
        }
    }
    public int Row { get; set; }
    public int Column { get; set; }
    public OthelloCellState CellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged();
            if (_cellState != OthelloCellState.None)
            {
                _button.enabled = false;
            }
        }
    }
    private void OnValidate()
    {
        OnCellStateChanged();
    }
    private void OnCellStateChanged()
    {
        if (_cellState == OthelloCellState.None)
        {
            _stoneImage.color = Color.clear;
        }
        else if (_cellState == OthelloCellState.Black)
        {
            _stoneImage.color = Color.black;
        }
        else if(_cellState == OthelloCellState.White)
        {
            _stoneImage.color = Color.white;
        }
        else if(_cellState == OthelloCellState.Blue)
        {
            _stoneImage.color = Color.blue;
        }
    }
    private void OnSetGuideChanged()
    {
        if (_isSetStone)
        {
            _isSetImage.SetActive(true);
        }
        else
        {
            _isSetImage.SetActive(false);
        }
    }
    public void ClickBoard()
    {
        if (OthelloGame.Instance.CanPutButton(Column, Row))
        {
            OthelloGame.Instance.SetStone(Column, Row);
        }
    }
}
