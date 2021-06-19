using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class BingoGame : MonoBehaviour
{
    [SerializeField] BingoCell _cell = null;
    BingoCell[,] _cells;
    int[] _naturalNumberArray = new int[75];
    int[] _randomArray = new int[75];
    int _count = 0;
    [SerializeField] GridLayoutGroup _gridLayoutGroup = null;
    [SerializeField] Text _riichiText = null;
    [SerializeField] Text _numText = null;
    bool _bingoCheck = false;
    private void Start()
    {
        SetBoard();
        SetRandomCell();
        _randomArray = _naturalNumberArray.OrderBy(i => Guid.NewGuid()).ToArray();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_bingoCheck)
        {
            Step();
        }
    }
    
    private void Step()
    {
        if (_count >= 75) return;

        foreach (var item in _cells)
        {
            if (item.BingoNum == _randomArray[_count])
            {
                item.OpenFrag = true;
            }
        }
        _numText.text = "前の数字\n　 " + _randomArray[_count]; 
        _count++;
        BingoCheck();
    }

    private void SetBoard()
    {
        _riichiText.text = "";
        _numText.text = "前の数字\n 　";
        _cell.OpenFrag = false;
        _gridLayoutGroup.constraintCount = 5;
        _cells = new BingoCell[5, 5];
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                var cell = Instantiate(_cell, transform);
                _cells[x, y] = cell;
            }
        }
    }
    public void SetRandomCell()
    {
        for (int i = 0; i < _naturalNumberArray.Length; i++)
        {
            _naturalNumberArray[i] = i + 1;
        }
        // ランダムな順にソートされた配列
        _randomArray = _naturalNumberArray.OrderBy(i => Guid.NewGuid()).ToArray();

        int count = 0;
        foreach (var item in _cells)
        {
            ///真ん中のマスの時は0(Freeマス)を入れる
            if (count == 12)
            {
                item.BingoNum = 0;
                item.OpenFrag = true;
                count++;
                continue;
            }
            item.BingoNum = _randomArray[count];
            count++;
        }
    }
    public void Bingo()
    {
        _riichiText.text = "ビンゴ！";
    }
    public void BingoCheck()
    {
        int riichiCount = 0;
        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            int openCount = 0;
            for (int x = 0; x < _cells.GetLength(0); x++)
            {
                if (_cells[x, y].OpenFrag)
                {
                    openCount++;
                }
            }
            if (openCount == 4)
            {
                riichiCount++;
            }
            if (openCount == 5)
            {
                _bingoCheck = true;
                Bingo();
                return;
            }
        }
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            int openCount = 0;
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                if (_cells[x, y].OpenFrag)
                {
                    openCount++;
                }
            }
            if (openCount == 4)
            {
                riichiCount++;
            }
            if (openCount == 5)
            {
                _bingoCheck = true;
                Bingo();
                return;
            }
        }
        int openRightCrossCount = 0;
        int openLeftCrossCount = 0;
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            if (_cells[i, i].OpenFrag)
            {
                openRightCrossCount++;
            }
            if (_cells[i, _cells.GetLength(0) - 1 - i].OpenFrag)
            {
                openLeftCrossCount++;
            }
        }
        if (openRightCrossCount == 5 || openLeftCrossCount == 5)
        {
            _bingoCheck = true;
            Bingo();
            return;
        }
        if (openRightCrossCount == 4)
        {
            riichiCount++;
        }
        if (openLeftCrossCount == 4)
        {
            riichiCount++;
        }
        if (riichiCount != 0)
        {
            _riichiText.text = riichiCount + " 列リーチ";
        }
    }
}
