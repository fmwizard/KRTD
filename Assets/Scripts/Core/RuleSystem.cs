using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSystem
{
    private int boardSize;
    private int winCondition;

    public int BoardSize => boardSize;
    public int WinCondition => winCondition;

    public RuleSystem(int boardSize, int winCondition)
    {
        this.boardSize = boardSize;
        this.winCondition = Mathf.Min(boardSize, winCondition);
    }

    public bool CheckWinCondition(Board board, CellMark playerMark)
    {
        // Check rows
        for (int row = 0; row < boardSize; row++)
        {
            if (CheckRow(board, row, playerMark))
                return true;
        }

        // Check columns
        for (int col = 0; col < boardSize; col++)
        {
            if (CheckColumn(board, col, playerMark))
                return true;
        }

        // Check diagonals
        if (CheckDiagonal(board, playerMark))
            return true;

        // Check anti-diagonals
        if (CheckAntiDiagonal(board, playerMark))
            return true;

        return false;
    }

    private bool CheckRow(Board board, int row, CellMark playerMark)
    {
        int count = 0;
        for (int col = 0; col < boardSize; col++)
        {
            if (board.GetCell(row, col).CellMark == playerMark)
            {
                count++;
                if (count >= winCondition)
                    return true;
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckColumn(Board board, int col, CellMark playerMark)
    {
        int count = 0;
        for (int row = 0; row < boardSize; row++)
        {
            if (board.GetCell(row, col).CellMark == playerMark)
            {
                count++;
                if (count >= winCondition)
                    return true;
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckDiagonal(Board board, CellMark playerMark)
    {
        for (int sRow = 0; sRow <= boardSize - winCondition; sRow++)
        {
            for (int sCol = 0; sCol <= boardSize - winCondition; sCol++)
            {
                bool win = true;
                for (int i = 0; i < winCondition; i++)
                {
                    if (board.GetCell(sRow + i, sCol + i).CellMark != playerMark)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                    return true;
            }
        }
        return false;
    }

    private bool CheckAntiDiagonal(Board board, CellMark playerMark)
    {
        for (int sRow = 0; sRow <= boardSize - winCondition; sRow++)
        {
            for (int sCol = winCondition - 1; sCol < boardSize; sCol++)
            {
                bool win = true;
                for (int i = 0; i < winCondition; i++)
                {
                    if (board.GetCell(sRow + i, sCol - i).CellMark != playerMark)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                    return true;
            }
        }
        return false;
    }
}
