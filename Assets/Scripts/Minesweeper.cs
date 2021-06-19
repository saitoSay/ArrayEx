using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Minesweeper : MonoBehaviour
{
    MineSweeperCell[,] _cells;
    [SerializeField] MineSweeperCell _cell = null;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    /// <summary>x軸のマスの数</summary>
    [SerializeField] int _xLength;
    /// <summary>y軸のマスの数</summary>
    [SerializeField] int _yLength;
    /// <summary>地雷の数</summary>
    [SerializeField] int _mineCount;
    /// <summary>開いたマスの数</summary>
    int _openCellCount = 0;
    [SerializeField] Text _clearText;
    [SerializeField] Text _remainingNumberOfMinesText;
    public bool _firstClickCheck = false;
    private void Start()
    {
        SetBoard();
    }
    private void Update()
    {
        if (!_firstClickCheck && Input.GetMouseButtonUp(0))
        {
            _firstClickCheck = true;
            MineCraft(_mineCount);
            CheckCellOpen();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CheckCellOpen();
        }
        if (_xLength * _yLength - _mineCount == _openCellCount)
        {
            _clearText.text = "クリア！";
        }
        _remainingNumberOfMinesText.text = "開ける残りマス数 : " +(_xLength * _yLength - _mineCount - _openCellCount);
    }
    /// <summary>
    /// ステータスがNullのセルを並べるメソッド
    /// 並べると同時に各セルにx座標とy座標の情報を持たせる
    /// </summary>
    private void SetBoard()
    {
        if (_xLength * _yLength <= _mineCount)
        {
            _mineCount = _xLength * _yLength - 1;
        }
        _cell.CellState = CellState.None;
        _gridLayoutGroup.constraintCount = _xLength;
        _cells = new MineSweeperCell[_xLength, _yLength];
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            for (int k = 0; k < _cells.GetLength(0); k++)
            {
                var cell = Instantiate(_cell, transform);
                cell._xNum = k;
                cell._yNum = i;
                _cells[k, i] = cell;
            }
        }
    }
    /// <summary>
    /// 地雷を設置するメソッド
    /// </summary>
    /// <param name="mineCount">設置する地雷の数</param>
    public void MineCraft(int mineCount)
    {

        while (0 < mineCount)
        {
            int x = Random.Range(0, _xLength);
            int y = Random.Range(0, _yLength);
            if (_cells[x, y].CellState != CellState.Mine && !_cells[x, y]._isClick)
            {
                _cells[x, y].CellState = CellState.Mine;
                AddCellNumber(x, y);
                mineCount--;
            }
        }
    }
    /// <summary>
    /// 地雷の周りのマスの数字を1増やすメソッド
    /// </summary>
    /// <param name="x">地雷の設置されたマスのx座標</param>
    /// <param name="y">地雷の設置されたマスのy座標</param>
    public void AddCellNumber(int x, int y)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (x + i >= 0 && x + i < _cells.GetLength(0) && y + k >= 0 && y + k < _cells.GetLength(1) && _cells[x + i, y + k].CellState != CellState.Mine)
                {
                    if (i == 0 && k == 0)
                    {
                        continue;
                    }
                    _cells[x + i, y + k].CellState += (int)CellState.One;
                }
            }
        }
    }
    /// <summary>
    /// セルを開くメソッド
    /// セルが空白の場合は八方向のセルで自分自身を呼び出す
    /// </summary>
    /// <param name="x">開くセルのx座標</param>
    /// <param name="y">開くセルのy座標</param>
    public void OpenCell(int x, int y)
    {
        if (_cells[x, y].CellState == CellState.Mine)
        {
            _cells[x, y].OpenCell();
            _clearText.text = "爆発！";
            return;
        }
        if (_cells[x, y].CellState != CellState.None)
        {
            _openCellCount++;
            _cells[x, y].OpenCell();
            return;
        }
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (x + k >= 0 && x + k < _cells.GetLength(0) && y + i >= 0 && y + i < _cells.GetLength(1))
                {
                    if (i == 0 && k == 0) continue;
                    if (_cells[x + k, y + i].CellState == CellState.None && !_cells[x + k, y + i]._Open)
                    {
                        _openCellCount++;
                        _cells[x + k, y + i].OpenCell();
                        OpenCell(x + k, y + i);
                    }
                    else if(!_cells[x + k, y + i]._Open)
                    {
                        _openCellCount++;
                        _cells[x + k, y + i].OpenCell();
                        continue;
                    }
                }
            }
        }
    }
    /// <summary>
    /// どのセルをクリックしたか確認し、セルを開くメソッド
    /// </summary>
    public void CheckCellOpen()
    {
        foreach (var item in _cells)
        {
            if (item._isClick)
            {
                OpenCell(item._xNum, item._yNum);
            }
        }
    }
}
