using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

public interface StrategySystem
{
    Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem);
}

public class EasyStrategy : StrategySystem
{
    private static readonly System.Random random = new System.Random();
    public Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem)
    {
        List<Vector2Int> availableMoves = board.GetAllEmptyCells();

        if (availableMoves.Count > 0)
        {
            int index;
            lock (random) // Locking to ensure thread safety
            {
                index = random.Next(0, availableMoves.Count);
            }
            return availableMoves[index];
        }

        return new Vector2Int(-1, -1);
    }
}


public class HardStrategy : StrategySystem
{
    // More aggressive depth limits for larger boards
    private const int MAX_DEPTH = 3;
    // Move cache to avoid recalculating positions
    private Dictionary<string, int> transpositionTable = new Dictionary<string, int>();
    // Use depth limit based on board size
    private int GetMaxDepth(int boardSize)
    {
        // For 3x3 board, we can search full depth
        if (boardSize <= 3) return 9;
        // For 4x4, limit the depth
        if (boardSize == 4) return 4;
        // For 5x5, use a very aggressive limit
        if (boardSize == 5) return 3;
        // For even larger boards
        return 2;
    }

    public Vector2Int GetNextMove(Board board, CellMark playerMark, RuleSystem ruleSystem)
    {
        List<Vector2Int> availableMoves = board.GetAllEmptyCells();

        if (availableMoves.Count == 0) return new Vector2Int(-1, -1);
        if (availableMoves.Count == 1) return availableMoves[0];

        // For larger boards, if it's the first move, pick the center or near-center position
        if (availableMoves.Count == board.Size * board.Size)
        {
            int center = board.Size / 2;
            return new Vector2Int(center, center);
        }

        CellMark opponentMark = playerMark == CellMark.X ? CellMark.O : CellMark.X;
        int bestValue = int.MinValue;
        Vector2Int bestMove = availableMoves[0]; // Default to first move instead of (-1,-1)
        
        // Clear the transposition table for a new search
        transpositionTable.Clear();
        
        // Sort moves to check possible winning moves or blocking moves first for better pruning
        List<Vector2Int> prioritizedMoves = PrioritizeMoves(board, availableMoves, playerMark, opponentMark, ruleSystem);

        int maxDepth = GetMaxDepth(board.Size);
        foreach (Vector2Int move in prioritizedMoves)
        {
            board.SetCell(move.x, move.y, playerMark, false);
            // Start with depth 0, maximum depth limit determined by board size
            int moveValue = Minimax(board, 0, maxDepth, false, ruleSystem, playerMark, opponentMark, int.MinValue, int.MaxValue);
            board.SetCell(move.x, move.y, CellMark.Empty, false);

            if (moveValue > bestValue)
            {
                bestValue = moveValue;
                bestMove = move;
            }
        }
        return bestMove;
    }
    
    // Helper method to prioritize moves for better alpha-beta pruning
    private List<Vector2Int> PrioritizeMoves(Board board, List<Vector2Int> moves,
                                            CellMark playerMark, CellMark opponentMark,
                                            RuleSystem ruleSystem)
    {
        List<Vector2Int> prioritizedMoves = new List<Vector2Int>(moves);
        List<Vector2Int> winningMoves = new List<Vector2Int>();
        List<Vector2Int> blockingMoves = new List<Vector2Int>();
        
        // Check for winning moves
        foreach (Vector2Int move in moves)
        {
            board.SetCell(move.x, move.y, playerMark, false);
            if (ruleSystem.CheckWinCondition(board, playerMark))
            {
                winningMoves.Add(move);
            }
            board.SetCell(move.x, move.y, CellMark.Empty, false);
        }
        
        // Check for moves that block opponent from winning
        foreach (Vector2Int move in moves)
        {
            board.SetCell(move.x, move.y, opponentMark, false);
            if (ruleSystem.CheckWinCondition(board, opponentMark))
            {
                blockingMoves.Add(move);
            }
            board.SetCell(move.x, move.y, CellMark.Empty, false);
        }
        
        // Create the prioritized list: winning moves, then blocking moves, then others
        prioritizedMoves = new List<Vector2Int>();
        prioritizedMoves.AddRange(winningMoves);
        foreach (Vector2Int move in blockingMoves)
        {
            if (!prioritizedMoves.Contains(move))
                prioritizedMoves.Add(move);
        }
        
        // Add remaining moves
        foreach (Vector2Int move in moves)
        {
            if (!prioritizedMoves.Contains(move))
                prioritizedMoves.Add(move);
        }
        
        return prioritizedMoves;
    }
    
    // Generate a hash key for the current board state to use in transposition table
    private string GenerateBoardKey(Board board, bool isMaximizing, CellMark playerMark, CellMark opponentMark)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < board.Size; i++)
        {
            for (int j = 0; j < board.Size; j++)
            {
                sb.Append(board.GetCell(i, j).CellMark.ToString()[0]);
            }
        }
        sb.Append(isMaximizing ? "1" : "0");
        return sb.ToString();
    }

    private int Minimax(Board board, int depth, int maxDepth, bool isMaximizing, RuleSystem ruleSystem,
        CellMark playerMark, CellMark opponentMark, int alpha, int beta)
    {
        // Check transposition table first
        string boardKey = GenerateBoardKey(board, isMaximizing, playerMark, opponentMark);
        if (transpositionTable.ContainsKey(boardKey))
        {
            return transpositionTable[boardKey];
        }

        // Terminal conditions
        if (ruleSystem.CheckWinCondition(board, playerMark))
        {
            return 100 - depth; // Player wins (higher score for quicker wins)
        }
        else if (ruleSystem.CheckWinCondition(board, opponentMark))
        {
            return depth - 100; // Opponent wins (lower score for quicker losses)
        }
        else if (board.GetAllEmptyCells().Count == 0)
        {
            return 0; // Draw
        }
        
        // Depth limit reached, evaluate the position heuristically
        if (depth >= maxDepth)
        {
            int score = EvaluateBoard(board, playerMark, opponentMark, ruleSystem);
            transpositionTable[boardKey] = score;
            return score;
        }

        if (isMaximizing)
        {
            int bestValue = int.MinValue;
            List<Vector2Int> moves = GetSortedMoves(board, playerMark, opponentMark, ruleSystem);
            
            foreach (Vector2Int move in moves)
            {
                board.SetCell(move.x, move.y, playerMark, false);
                bestValue = Mathf.Max(bestValue, Minimax(board, depth + 1, maxDepth, false, ruleSystem, playerMark, opponentMark, alpha, beta));
                board.SetCell(move.x, move.y, CellMark.Empty, false);
                alpha = Mathf.Max(alpha, bestValue);
                if (beta <= alpha)
                    break;
            }
            transpositionTable[boardKey] = bestValue;
            return bestValue;
        }
        else
        {
            int bestValue = int.MaxValue;
            List<Vector2Int> moves = GetSortedMoves(board, opponentMark, playerMark, ruleSystem);
            
            foreach (Vector2Int move in moves)
            {
                board.SetCell(move.x, move.y, opponentMark, false);
                bestValue = Mathf.Min(bestValue, Minimax(board, depth + 1, maxDepth, true, ruleSystem, playerMark, opponentMark, alpha, beta));
                board.SetCell(move.x, move.y, CellMark.Empty, false);
                beta = Mathf.Min(beta, bestValue);
                if (beta <= alpha)
                    break;
            }
            transpositionTable[boardKey] = bestValue;
            return bestValue;
        }
    }
    
    // Get a sorted list of moves for better alpha-beta pruning
    private List<Vector2Int> GetSortedMoves(Board board, CellMark currentMark, CellMark otherMark, RuleSystem ruleSystem)
    {
        List<Vector2Int> emptyCells = board.GetAllEmptyCells();
        Dictionary<Vector2Int, int> moveScores = new Dictionary<Vector2Int, int>();
        
        // Quickly assess each move's value
        foreach (Vector2Int move in emptyCells)
        {
            // Try the move
            board.SetCell(move.x, move.y, currentMark, false);
            
            // Check for immediate win (highest priority)
            if (ruleSystem.CheckWinCondition(board, currentMark))
            {
                board.SetCell(move.x, move.y, CellMark.Empty, false);
                moveScores[move] = 1000;
                continue;
            }
            
            // Undo and try opponent's move there to check for blocks
            board.SetCell(move.x, move.y, CellMark.Empty, false);
            board.SetCell(move.x, move.y, otherMark, false);
            
            if (ruleSystem.CheckWinCondition(board, otherMark))
            {
                board.SetCell(move.x, move.y, CellMark.Empty, false);
                moveScores[move] = 900;
                continue;
            }
            
            // Neither win nor block, use center and corner preference
            board.SetCell(move.x, move.y, CellMark.Empty, false);
            
            // Prefer center
            int center = board.Size / 2;
            if (move.x == center && move.y == center)
            {
                moveScores[move] = 500;
                continue;
            }
            
            // Then corners
            if ((move.x == 0 || move.x == board.Size - 1) &&
                (move.y == 0 || move.y == board.Size - 1))
            {
                moveScores[move] = 300;
                continue;
            }
            
            // Default score
            moveScores[move] = 100;
        }
        
        // Sort moves by score (descending)
        return emptyCells.OrderByDescending(move => moveScores.ContainsKey(move) ? moveScores[move] : 0).ToList();
    }
    
    // Simplified heuristic evaluation function for non-terminal states
    private int EvaluateBoard(Board board, CellMark playerMark, CellMark opponentMark, RuleSystem ruleSystem)
    {
        // Simplified and faster evaluation
        int score = 0;
        int boardSize = board.Size;
        int winCondition = ruleSystem.WinCondition;
        
        // Count the number of two-in-a-rows for both players
        int playerTwos = CountSequences(board, playerMark, 2, winCondition);
        int opponentTwos = CountSequences(board, opponentMark, 2, winCondition);
        
        // For win condition of 3 or more, count sequences of length winCondition-1
        if (winCondition >= 3)
        {
            int playerAlmostWins = CountSequences(board, playerMark, winCondition - 1, winCondition);
            int opponentAlmostWins = CountSequences(board, opponentMark, winCondition - 1, winCondition);
            
            score += playerAlmostWins * 10 - opponentAlmostWins * 10;
        }
        
        // Add in the two-in-a-rows with less weight
        score += playerTwos * 3 - opponentTwos * 3;
        
        // Center control bonus
        int center = boardSize / 2;
        if (board.GetCell(center, center).CellMark == playerMark)
            score += 5;
        else if (board.GetCell(center, center).CellMark == opponentMark)
            score -= 5;
            
        return score;
    }
    
    // Count sequences of a given length for a player
    private int CountSequences(Board board, CellMark mark, int seqLength, int winLength)
    {
        int count = 0;
        int boardSize = board.Size;
        
        // Simplified counting that just checks for patterns without complex evaluation
        
        // Rows
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col <= boardSize - winLength; col++)
            {
                if (IsSequence(board, row, col, 0, 1, seqLength, mark, winLength))
                    count++;
            }
        }
        
        // Columns
        for (int col = 0; col < boardSize; col++)
        {
            for (int row = 0; row <= boardSize - winLength; row++)
            {
                if (IsSequence(board, row, col, 1, 0, seqLength, mark, winLength))
                    count++;
            }
        }
        
        // Diagonals
        for (int row = 0; row <= boardSize - winLength; row++)
        {
            for (int col = 0; col <= boardSize - winLength; col++)
            {
                if (IsSequence(board, row, col, 1, 1, seqLength, mark, winLength))
                    count++;
            }
        }
        
        // Anti-diagonals
        for (int row = 0; row <= boardSize - winLength; row++)
        {
            for (int col = winLength - 1; col < boardSize; col++)
            {
                if (IsSequence(board, row, col, 1, -1, seqLength, mark, winLength))
                    count++;
            }
        }
        
        return count;
    }
    
    // Check if there's a sequence of exactly 'seqLength' marks with potential to win
    private bool IsSequence(Board board, int startRow, int startCol, int rowInc, int colInc,
                          int seqLength, CellMark mark, int winLength)
    {
        int markCount = 0;
        int emptyCount = 0;
        
        for (int i = 0; i < winLength; i++)
        {
            int row = startRow + i * rowInc;
            int col = startCol + i * colInc;
            
            CellMark cellMark = board.GetCell(row, col).CellMark;
            if (cellMark == mark)
                markCount++;
            else if (cellMark == CellMark.Empty)
                emptyCount++;
        }
        
        // We want exactly seqLength marks and the rest empty
        return markCount == seqLength && markCount + emptyCount == winLength;
    }
}


