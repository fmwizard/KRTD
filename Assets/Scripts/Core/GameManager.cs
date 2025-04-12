using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CellMark { Empty, X, O }
public enum PlayerType { AIPlayer, HumanPlayer }
public enum Difficulty { Easy, Medium, Hard }
public enum GameState { Setup, InProgress, Finished }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action<int> SetupBoard;
    public int boardSize = 3;

    private GameState gameState;
    private bool isAITurn;

    private Player playerX;
    private Player playerO;
    private Player currentPlayer;

    private Board board;
    public GameObject cellPrefab;
    public GameObject cellContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        gameState = GameState.Setup;
    }
    // Start is called before the first frame update
    void Start()
    {
        LaunchGame();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.InProgress)
        {
            HandlePlayerTurn();
        }
    }
    private void InitBoard()
    {
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
        board.SetupBoard(boardSize);
    }
    private void InitPlayers()
    {
        playerX = new HumanPlayer(CellMark.X, "Player X");
        currentPlayer = playerX;
    }

    private void LaunchGame()
    {
        gameState = GameState.Setup;
        InitBoard();
        InitPlayers();
        currentPlayer = playerX;
        isAITurn = currentPlayer.PlayerType == PlayerType.AIPlayer;
        gameState = GameState.InProgress;
    }

    private void HandlePlayerTurn()
    {
        if (isAITurn)
        {
            // AI logic to make a move
        }
        else
        {
            // Wait for human player input 
        }
    }

    public void OnCellClicked(int x, int y)
    {
        if (gameState == GameState.InProgress && !isAITurn)
        {
            Cell cell = board.GetCell(x, y);
            if (cell && cell.CellMark == CellMark.Empty)
            {
                cell.CellMark = currentPlayer.PlayerMark;
            }
        }
    }
}
