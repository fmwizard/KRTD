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

    public GameState gameState;
    private bool isAITurn;
    private RuleSystem ruleSystem;

    private UIManager uiManager;

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
        uiManager = UIManager.Instance;
        EnterMainMenu();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.InProgress)
        {
            HandlePlayerTurn();
        }

    }

    private void InitBoard(int boardSize)
    {
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
        board.SetupBoard(boardSize);
    }

    private void InitPlayers(PlayerSetting playerSettingX, PlayerSetting playerSettingO)
    {
        if (playerSettingX.playerType == PlayerType.HumanPlayer)
        {
            playerX = new HumanPlayer(CellMark.X, playerSettingX.playerName);
        }
        else
        {
            playerX = new AIPlayer(CellMark.X, playerSettingX.difficulty, board, ruleSystem);
        }
        if (playerSettingO.playerType == PlayerType.HumanPlayer)
        {
            playerO = new HumanPlayer(CellMark.O, playerSettingO.playerName);
        }
        else
        {
            playerO = new AIPlayer(CellMark.O, playerSettingO.difficulty, board, ruleSystem);
        }
        currentPlayer = playerX;
    }

    private void InitRuleSystem(int boardSize, int winCondition)
    {
        ruleSystem = new RuleSystem(boardSize, winCondition);
    }

    private void EnterMainMenu()
    {
        gameState = GameState.Setup;
        uiManager.ShowPanel(uiManager.menuPanel);
    }

    public void LaunchGame(GameSetting gameSetting)
    { 
        uiManager.ShowPanel(uiManager.gamePanel);
        InitBoard(gameSetting.boardSize);
        InitRuleSystem(gameSetting.boardSize, gameSetting.winCondition);
        InitPlayers(gameSetting.playerX, gameSetting.playerO);
        currentPlayer = playerX;
        isAITurn = currentPlayer.PlayerType == PlayerType.AIPlayer;
        gameState = GameState.InProgress;
    }

    public void EndGame()
    {
        gameState = GameState.Setup;
        Destroy(board.gameObject);
        board = null;
        ruleSystem = null;
        playerX = null;
        playerO = null;
        currentPlayer = null;
    }

    private void HandlePlayerTurn()
    {
        if (isAITurn)
        {
            AIPlayer aiPlayer = currentPlayer as AIPlayer;
            Vector2Int move = aiPlayer.GetNextMove();
            if (move.x != -1 && move.y != -1)
            {
                MakeMove(move.x, move.y);
            }
            else
            {
                Debug.Log("No valid move available.");
            }
        }
        else
        {
            // Wait for human player input 
        }
    }

    private void SwitchPlayer()
    {
        currentPlayer = currentPlayer == playerX ? playerO : playerX;
        isAITurn = currentPlayer.PlayerType == PlayerType.AIPlayer;
    }

    private void MakeMove(int x, int y)
    {
        Cell cell = board.GetCell(x, y);
        if (cell && cell.CellMark == CellMark.Empty)
        {
            board.SetCell(x, y, currentPlayer.PlayerMark);
            if (ruleSystem.CheckWinCondition(board, currentPlayer.PlayerMark))
            {
                gameState = GameState.Finished;
                Debug.Log($"{currentPlayer.PlayerMark} wins!");
            }
            else if (board.IsBoardFull())
            {
                gameState = GameState.Finished;
                Debug.Log("It's a draw!");
            }
            else
            {
                SwitchPlayer();
            }
        }
    }
    
    public void OnCellClicked(int x, int y)
    {
        if (gameState == GameState.InProgress && !isAITurn)
        {
            MakeMove(x, y);

        }
    }
}
