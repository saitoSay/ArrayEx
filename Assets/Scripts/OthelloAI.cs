using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloAI : MonoBehaviour
{
    public static OthelloAI Instance { get; private set; }
    int _playerNum;
    private void Awake()
    {
        Instance = this;
    }
    public void SetStone(List<OthelloGameCell> list, OthelloGameCell[,] cells)
    {
        
        int min = int.MaxValue;
        int setX = -1;
        int setY = -1;
        foreach (var item in list)
        {
            var buffer = new OthelloGameCell[cells.GetLength(0), cells.GetLength(1)];
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    var buf = Instantiate(cells[x, y], transform);
                    buffer[x, y] = buf;
                    buffer[x, y].CellState = cells[x, y].CellState;
                }
            }
            buffer[item.Column, item.Row].CellState = OthelloCellState.White;
            for (int i = -1; i < 2; i++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if (i == 0 && k == 0)
                    {
                        continue;
                    }
                    ReverseStone(item.Column, item.Row, k, i, buffer);
                }
            }
            Debug.Log(min + " " + CheckAllCells(buffer));
            if (CheckAllCells(buffer) < min)
            {
                min = CheckAllCells(buffer);
                setX = item.Column;
                setY = item.Row;
                Debug.Log("座標" + setX + " " + setY); 
            }
            GameObject temp = GameObject.Find("Buffer");
            foreach (Transform i in temp.transform)
            {
                Destroy(i.gameObject);
            }
        }
        if (setX == -1)
        {
            return;
        }
        OthelloGame.Instance.SetStone(setX, setY);
    }
    public void SetPlayerNum(int playerNum)
    {
        _playerNum = playerNum;
    }
    private int CheckAllCells(OthelloGameCell[,] cells)
    {
        int count = 0;
        for (int y = 0; y < cells.GetLength(1); y++)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                cells[x, y].IsSetStone = false;
                if (cells[x, y].CellState == OthelloCellState.None)
                {
                    if (CanPutButton(x, y, cells))
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }
    public bool CanPutButton(int x, int y, OthelloGameCell[,] cells)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int k = -1; k < 2; k++)
            {
                if (i == 0 && k == 0)
                {
                    continue;
                }
                if (CanPutButton(x, y, k, i, false, cells))
                {
                    cells[x, y].IsSetStone = true;
                    return true;
                }

            }
        }
        return false;
    }
    private bool CanPutButton(int x, int y, int vectorX, int vectorY, bool firstStoneCheck, OthelloGameCell[,] cells)
    {
        int nextX = x + vectorX;
        int nextY = y + vectorY;
        if (nextX >= 0 && nextX < cells.GetLength(0) && nextY >= 0 && nextY < cells.GetLength(1))
        {
            if (OthelloGame.Instance.GetPlayerChangeCounter() % _playerNum == 0)
            {
                if (cells[nextX, nextY].CellState == OthelloCellState.White)
                {
                    firstStoneCheck = true;
                    return CanPutButton(nextX, nextY, vectorX, vectorY, firstStoneCheck, cells);
                }
                else if (cells[nextX, nextY].CellState == OthelloCellState.Black && firstStoneCheck)
                {
                    return true;
                }
            }
            if (OthelloGame.Instance.GetPlayerChangeCounter() % _playerNum == 1)
            {
                if (cells[nextX, nextY].CellState == OthelloCellState.Black)
                {
                    firstStoneCheck = true;
                    return CanPutButton(nextX, nextY, vectorX, vectorY, firstStoneCheck, cells);
                }
                else if (cells[nextX, nextY].CellState == OthelloCellState.White && firstStoneCheck)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void ReverseStone(int x, int y, int vectorX, int vectorY, OthelloGameCell[,] cells)
    {
        
        if (CanPutButton(x, y, vectorX, vectorY, false, cells))
        {
            int nextX = x + vectorX;
            int nextY = y + vectorY;

            if (nextX >= 0 && nextX < cells.GetLength(0) && nextY >= 0 && nextY < cells.GetLength(1))
            {
                if (OthelloGame.Instance.GetPlayerChangeCounter() % _playerNum == 0)
                {
                    if (cells[nextX, nextY].CellState == OthelloCellState.White)
                    {
                        cells[nextX, nextY].CellState = OthelloCellState.Black;
                        ReverseStone(nextX, nextY, vectorX, vectorY, cells);
                    }
                }
                else if (OthelloGame.Instance.GetPlayerChangeCounter() % _playerNum == 1)
                {
                    if (cells[nextX, nextY].CellState == OthelloCellState.Black)
                    {
                        cells[nextX, nextY].CellState = OthelloCellState.White;
                        ReverseStone(nextX, nextY, vectorX, vectorY, cells);
                    }
                }
            }
        }
    }
}
