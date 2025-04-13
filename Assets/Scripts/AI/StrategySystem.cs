using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StrategySystem
{
    Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem);
}

public class EasyStrategy : StrategySystem
{
    public Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem)
    {
        List<Vector2Int> availableMoves = board.GetAllEmptyCells();

        if (availableMoves.Count > 0)
        {
            return availableMoves[Random.Range(0, availableMoves.Count)];
        }

        return new Vector2Int(-1, -1);
    }
}

public class MediumStrategy : StrategySystem
{
    public Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem)
    {
        List<Vector2Int> availableMoves = board.GetAllEmptyCells();

        if (availableMoves.Count == 0) return new Vector2Int(-1, -1);
        if (availableMoves.Count == 1) return availableMoves[0];

        CellMark opponentMark = playerMark == CellMark.X ? CellMark.O : CellMark.X;
        int bestValue = int.MinValue;
        Vector2Int bestMove = new Vector2Int(-1, -1);
        foreach (Vector2Int move in availableMoves)
        {
            board.SetCell(move.x, move.y, playerMark, false);
            int moveValue = Minimax(board, 0, false, ruleSystem, playerMark, opponentMark, int.MinValue, int.MaxValue);
            board.SetCell(move.x, move.y, CellMark.Empty, false);

            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = move;
            }
        }
        return bestMove;
    }
    
    private int Minimax(Board board, int depth, bool isMaximizing, RuleSystem ruleSystem,
        CellMark playerMark, CellMark opponentMark, int alpha, int beta)
    {
        if (ruleSystem.CheckWinCondition(board, playerMark))
        {
            return 10 - depth; // Player wins
        }
        else if (ruleSystem.CheckWinCondition(board, opponentMark))
        {
            return depth - 10; // Opponent wins
        }
        else if (board.GetAllEmptyCells().Count == 0)
        {
            return 0; // Draw
        }

        if (isMaximizing)
        {
            int bestValue = int.MinValue;
            foreach (Vector2Int move in board.GetAllEmptyCells())
            {
                board.SetCell(move.x, move.y, playerMark);
                bestValue = Mathf.Max(bestValue, Minimax(board, depth + 1, false, ruleSystem, playerMark, opponentMark, alpha, beta));
                board.SetCell(move.x, move.y, CellMark.Empty);
                alpha = Mathf.Max(alpha, bestValue);
                if (beta <= alpha)
                    break;
            }
            return bestValue;
        }
        else
        {
            int bestValue = int.MaxValue;
            foreach (Vector2Int move in board.GetAllEmptyCells())
            {
                board.SetCell(move.x, move.y, opponentMark);
                bestValue = Mathf.Min(bestValue, Minimax(board, depth + 1, true, ruleSystem, playerMark, opponentMark, alpha, beta));
                board.SetCell(move.x, move.y, CellMark.Empty);
                beta = Mathf.Min(beta, bestValue);
                if (beta <= alpha)
                    break;
            }
            return bestValue;
        }

    }
}

