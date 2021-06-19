using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CellState
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,

    Mine = -1
}
public class MineSweeperCell : MonoBehaviour//, IPointerClickHandler
{
    [SerializeField] private Text _view = null;
    [SerializeField] private CellState _cellState = CellState.None;
    [SerializeField] GameObject _button = null;
    public bool _isClick = false;
    public bool _Open = false;
    public int _xNum;
    public int _yNum;
    public CellState CellState
    {
        get => _cellState;
        set
        {
            _cellState = value;
            OnCellStateChanged();
        }
    }
    
    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (_view == null) { return; }

        if (_cellState == CellState.None)
        {
            _view.text = "";
        }
        else if (_cellState == CellState.Mine)
        {
            _view.text = "X";
            _view.color = Color.red;
        }
        else
        {
            _view.text = ((int)_cellState).ToString();
            _view.color = Color.blue;
        }
    }
    public void ClickCheck()
    {
        _isClick = true;
        _button.SetActive(false);
    }
    public void OpenCell()
    {
        _Open = true;
        _isClick = false;
        _button.SetActive(false);
    }
}
