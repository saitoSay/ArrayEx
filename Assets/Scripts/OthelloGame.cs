using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OthelloGame : MonoBehaviour
{
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] int _playerNum;
    [SerializeField] Text _text = null;
    [SerializeField] int _rows;
    [SerializeField] int _columns;
    [SerializeField] OthelloGameCell _cell = null;
    [SerializeField] GameObject[] _buttons;
    public static OthelloGame Instance { get; private set; }
    OthelloGameCell[,] _cells;
    int _passCheckCount = 0;
    bool _gameSetCheck = false;
    int _playerChangeCounter = 0;
    private bool _aiMode = false;

    private void Awake()
    {
        Instance = this;
    }
    public void SelectAI(bool aiMode)
    {
        if (aiMode)
        {
            _aiMode = true;
        }
    }
    public void SetBoard(int playerNum)
    {
        OthelloAI.Instance.SetPlayerNum(playerNum);
        _text.text = "黒のターン";
        if (playerNum == 2)
        {
            _columns = 8;
            _rows = 8;
            _gridLayoutGroup.constraintCount = 8;
        }
        else if (playerNum == 3)
        {
            _columns = 9;
            _rows = 9;
            _gridLayoutGroup.constraintCount = 9;
        }
        _cells = new OthelloGameCell[_columns, _rows];
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                var cell = Instantiate(_cell, transform);
                if (playerNum == 2)
                {
                    if (x == 4 && y == 3 || x == 3 && y == 4)
                    {
                        cell.CellState = OthelloCellState.Black;
                    }
                    else if (x == 3 && y == 3 || x == 4 && y == 4)
                    {
                        cell.CellState = OthelloCellState.White;
                    }
                    else
                    {
                        cell.CellState = OthelloCellState.None;
                    }
                }
                else if (playerNum == 3)
                {
                    if (x == 3 && y == 3 || x == 4 && y == 4 || x == 5 && y == 5)
                    {
                        cell.CellState = OthelloCellState.Black;
                    }
                    else if (x == 4 && y == 3 || x == 5 && y == 4 || x == 3 && y == 5)
                    {
                        cell.CellState = OthelloCellState.White;
                    }
                    else if (x == 5 && y == 3 || x == 3 && y == 4 || x == 4 && y == 5)
                    {
                        cell.CellState = OthelloCellState.Blue;
                    }
                    else
                    {
                        cell.CellState = OthelloCellState.None;
                    }

                }
                cell.Column = x;
                cell.Row = y;
                _cells[x, y] = cell;
            }
        }
        CheckAllCells();
        int count = 0;
        foreach (var item in _buttons)
        {
            item.SetActive(false);
            if (count == 3)
            {
                item.SetActive(true);
            }
            count++;
        }
    }
    public void ResetBoard()
    {
        foreach (Transform item in this.gameObject.transform)
        {
            Destroy(item.gameObject);
        }
        int count = 0;
        foreach (var item in _buttons)
        {
            item.SetActive(true);
            if (count == 3)
            {
                item.SetActive(false);
            }
            count++;
        }
        _playerChangeCounter = 0;
        _aiMode = false;
        _gameSetCheck = false;
        _text.text = "";
    }
    public void SetStone(int x, int y)
    {
        if (_playerChangeCounter % _playerNum == 0)
        {
            _cells[x, y].CellState = OthelloCellState.Black;
        }
        else if(_playerChangeCounter % _playerNum == 1)
        {
            _cells[x, y].CellState = OthelloCellState.White;
        }
        else
        {
            _cells[x, y].CellState = OthelloCellState.Blue;
        }
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (i == 0 && k == 0)
                {
                    continue;
                }
                ReverseStone(x, y, k, i);
            }
        }
        _playerChangeCounter++;
        CheckAllCells();
        if (_gameSetCheck)
        {
            return;
        }
        if (_playerChangeCounter % _playerNum == 0)
        {
            _text.text = "黒のターン";
            _text.color = Color.black;
        }
        else if (_playerChangeCounter % _playerNum == 1)
        {
            _text.text = "白のターン";
            _text.color = Color.white;
        }
        else
        {
            _text.text = "青のターン";
            _text.color = Color.blue;
        }
        if (_aiMode && _playerChangeCounter % _playerNum == 1)
        {
            var list = PutButtonDataPass();
            OthelloAI.Instance.SetStone(list, _cells);
        }
    }
    private void CheckAllCells()
    {
        bool isReverse = false;
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                _cells[x, y].IsSetStone = false;
                if (_cells[x, y].CellState == OthelloCellState.None)
                {
                    if (CanPutButton(x, y))
                    {
                        isReverse = true;
                    } 
                }
            }
        }
        if (!isReverse)
        {
            _playerChangeCounter++;
            _passCheckCount++;
            if (_passCheckCount == _playerNum)
            {
                GameSet();
                return;
            }
            CheckAllCells();
        }
        else
        {
            _passCheckCount = 0;
        }
    }
    public bool CanPutButton(int x, int y)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (i == 0 && k == 0)
                {
                    continue;
                }
                if (CanPutButton(x, y, k, i, false))
                {
                    _cells[x, y].IsSetStone = true;
                    return true;
                }

            }
        }
        return false;
    }
    private bool CanPutButton(int x, int y, int vectorX, int vectorY, bool firstStoneCheck)
    {
        int nextX = x + vectorX;
        int nextY = y + vectorY;
        if (nextX >= 0 && nextX < _cells.GetLength(0) && nextY >= 0 && nextY < _cells.GetLength(1))
        {
            if (_playerChangeCounter % _playerNum == 0)
            {
                if (_cells[nextX, nextY].CellState == OthelloCellState.White || _cells[nextX, nextY].CellState == OthelloCellState.Blue)
                {
                    firstStoneCheck = true;
                    return CanPutButton(nextX, nextY, vectorX, vectorY, firstStoneCheck);
                }
                else if (_cells[nextX, nextY].CellState == OthelloCellState.Black && firstStoneCheck)
                {
                    return true;
                }
            }
            if (_playerChangeCounter % _playerNum == 1)
            {
                if (_cells[nextX, nextY].CellState == OthelloCellState.Black || _cells[nextX, nextY].CellState == OthelloCellState.Blue)
                {
                    firstStoneCheck = true;
                    return CanPutButton(nextX, nextY, vectorX, vectorY, firstStoneCheck);
                }
                else if (_cells[nextX, nextY].CellState == OthelloCellState.White && firstStoneCheck)
                {
                    return true;
                }
            }
            if (_playerChangeCounter % _playerNum == 2)
            {
                if (_cells[nextX, nextY].CellState == OthelloCellState.Black || _cells[nextX, nextY].CellState == OthelloCellState.White)
                {
                    firstStoneCheck = true;
                    return CanPutButton(nextX, nextY, vectorX, vectorY, firstStoneCheck);
                }
                else if (_cells[nextX, nextY].CellState == OthelloCellState.Blue && firstStoneCheck)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ReverseStone(int x, int y, int vectorX, int vectorY)
    {
        if (CanPutButton(x, y, vectorX, vectorY, false))
        {
            int nextX = x + vectorX;
            int nextY = y + vectorY;

            if (nextX >= 0 && nextX < _cells.GetLength(0) && nextY >= 0 && nextY < _cells.GetLength(1))
            {
                if (_playerChangeCounter % _playerNum == 0)
                {
                    if (_cells[nextX, nextY].CellState == OthelloCellState.White || _cells[nextX, nextY].CellState == OthelloCellState.Blue)
                    {
                        _cells[nextX, nextY].CellState = OthelloCellState.Black;
                        ReverseStone(nextX, nextY, vectorX, vectorY);
                    }
                }
                else if(_playerChangeCounter % _playerNum == 1)
                {
                    if (_cells[nextX, nextY].CellState == OthelloCellState.Black || _cells[nextX, nextY].CellState == OthelloCellState.Blue)
                    {
                        _cells[nextX, nextY].CellState = OthelloCellState.White;
                        ReverseStone(nextX, nextY, vectorX, vectorY);
                    }
                }
                else if (_playerChangeCounter % _playerNum == 2)
                {
                    if (_cells[nextX, nextY].CellState == OthelloCellState.Black || _cells[nextX, nextY].CellState == OthelloCellState.White)
                    {
                        _cells[nextX, nextY].CellState = OthelloCellState.Blue;
                        ReverseStone(nextX, nextY, vectorX, vectorY);
                    }
                }
            }
        }
    }
    private void GameSet()
    {
        _gameSetCheck = true;
        int blackCount = 0;
        int whiteCount = 0;
        int blueCount = 0;
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                if (_cells[x, y].CellState == OthelloCellState.Black)
                {
                    blackCount++;
                }
                else if (_cells[x, y].CellState == OthelloCellState.White)
                {
                    whiteCount++;
                }
                else if (_cells[x, y].CellState == OthelloCellState.Blue)
                {
                    blueCount++;
                }
            }
        }
        if (_playerNum == 2)
        {
            if (blackCount == whiteCount)
            {
                _text.text = $"黒 : {blackCount}\n白 : {whiteCount}\n引き分け";
            }
            else
            {
                _text.text = $"黒 : {blackCount}\n白 : {whiteCount}\n{(blackCount > whiteCount ? "黒" : "白")}の勝ち";
            }
        }
        else if (_playerNum == 3)
        {
            if (blackCount == whiteCount && blackCount == blueCount)
            {
                _text.text = $"黒 : {blackCount}\n白 : {whiteCount}\n青 : {blueCount}\n引き分け";
            }
            else
            {
                _text.text = $"黒 : {blackCount}\n白 : {whiteCount}\n青 : {blueCount}\n" +
                    $"{(blackCount > whiteCount && blackCount > whiteCount ? "黒" : (whiteCount > blueCount ? "白" : "青"))}の勝ち";
                _text.color = blackCount > whiteCount && blackCount > whiteCount ? Color.black : (whiteCount > blueCount ? Color.white : Color.blue);
            }
        }
    }
    public List<OthelloGameCell> PutButtonDataPass()
    {
        List<OthelloGameCell> putList = new List<OthelloGameCell>();
        foreach (var item in _cells)
        {
            if (item.IsSetStone)
            {
                putList.Add(item);
            }
        }
        return putList;
    }
    public int GetPlayerChangeCounter()
    {
        return _playerChangeCounter;
    }
    public OthelloGameCell[,] GetOthelloGameCells()
    {
        return _cells;
    }
}
