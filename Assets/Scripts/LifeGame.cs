using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGame : MonoBehaviour
{
    LifeGameCell[,] _cells;
    [SerializeField] LifeGameCell _cell;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    /// <summary>x軸のマスの数</summary>
    [SerializeField] int _xLength;
    /// <summary>y軸のマスの数</summary>
    [SerializeField] int _yLength;
    bool[,] _nextIsAlive;
    float _time = 0;
    [SerializeField] float _stepTime;
    private void Start()
    {
        if (_stepTime <= 0)
        {
            _stepTime = 0.5f;
        }
        SetBoard();
        SetRandomCell();
    }
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _stepTime)
        {
            _time = 0;
            Step();
        }
    }
    private void SetBoard()
    {
        _cell.IsAlive = false;
        _gridLayoutGroup.constraintCount = _xLength;
        _cells = new LifeGameCell[_xLength, _yLength];
        _nextIsAlive = new bool[_xLength, _yLength];
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            for (int k = 0; k < _cells.GetLength(0); k++)
            {
                var cell = Instantiate(_cell, transform);
                _cells[k, i] = cell;
            }
        }
    }
    public void SetRandomCell()
    {
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            for (int k = 0; k < _cells.GetLength(0); k++)
            {
                int r = Random.Range(0, 2);
                if (r == 0)
                {
                    _cells[k, i].IsAlive = true;
                }
                else
                {
                    _cells[k, i].IsAlive = false;
                }
            }
        }
    }
    public int SearchAround(int x, int y)
    {
        int aliveCellCount = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (i == 0 && k == 0) continue;
                int xNum = x + k;
                int yNum = y + i;

                if (xNum == -1) xNum = _cells.GetLength(0) - 1;
                if (xNum == _cells.GetLength(0)) xNum = 0;

                if (yNum == -1) yNum = _cells.GetLength(1) - 1;
                if (yNum == _cells.GetLength(1)) yNum = 0;

                if (_cells[xNum, yNum].IsAlive)
                {
                    aliveCellCount++;
                }
            }
        }
        return aliveCellCount;
    }
    public void Step()
    {
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                int count = SearchAround(x, y);
                if (_cells[x, y].IsAlive && count <= 1)
                {
                    _nextIsAlive[x, y] = false;
                }
                else if (_cells[x, y].IsAlive && (count == 2 || count == 3))
                {
                    _nextIsAlive[x, y] = true;
                }
                else if (_cells[x, y].IsAlive && count >= 4)
                {
                    _nextIsAlive[x, y] = false;
                }
                else if (_cells[x, y].IsAlive == false && count == 3)
                {
                    _nextIsAlive[x, y] = true;
                }
                else
                {
                    _nextIsAlive[x, y] = false;
                }
            }
        }
        for (int i = 0; i < _cells.GetLength(1); i++)
        {
            for (int k = 0; k < _cells.GetLength(0); k++)
            {

                _cells[k, i].IsAlive = _nextIsAlive[k, i];
            }
        }
    }
}
