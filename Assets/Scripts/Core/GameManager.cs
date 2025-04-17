using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
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
    private int playerXID;
    private Player playerO;
    private int playerOID;
    private Player currentPlayer;

    private bool isAIThink;
    private bool aiMoveReady = false;
    private Vector2Int aiNextMove;

    private Board board;
    private string moveSequence;
    public bool isDraw;
    public GameObject cellPrefab;
    public GameObject cellContainer;

    public GameTable replayGame;
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
        UIManager.Instance.SetGameBoardUI(boardSize);
    }

    private void InitPlayers(PlayerSetting playerSettingX, PlayerSetting playerSettingO)
    {
        if (playerSettingX.playerType == PlayerType.HumanPlayer)
        {
            playerX = new HumanPlayer(CellMark.X, playerSettingX.playerName);
            int id = DataManager.Instance.GetPlayerIDByName(playerSettingX.playerName);
            if (id == -1)
            {
                id = DataManager.Instance.InsertPlayerRecord(playerSettingX.playerName, 0, 0, 0);
            }
            playerXID = id;
        }
        else
        {
            playerX = new AIPlayer(CellMark.X, playerSettingX.difficulty, board, ruleSystem);
            playerXID = 1; // AI player ID
        }
        if (playerSettingO.playerType == PlayerType.HumanPlayer)
        {
            playerO = new HumanPlayer(CellMark.O, playerSettingO.playerName);
            int id = DataManager.Instance.GetPlayerIDByName(playerSettingO.playerName);
            if (id == -1)
            {
                id = DataManager.Instance.InsertPlayerRecord(playerSettingO.playerName, 0, 0, 0);
            }
            playerOID = id;
        }
        else
        {
            playerO = new AIPlayer(CellMark.O, playerSettingO.difficulty, board, ruleSystem);
            playerOID = 1; // AI player ID
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
        //uiManager.ShowRankPanel();
        uiManager.ShowPanel(uiManager.menuPanel);
    }

    public void LaunchGame(GameSetting gameSetting)
    { 
        uiManager.ShowPanel(uiManager.playPanel);
        InitBoard(gameSetting.boardSize);
        InitRuleSystem(gameSetting.boardSize, gameSetting.winCondition);
        InitPlayers(gameSetting.playerX, gameSetting.playerO);
        currentPlayer = playerX;
        moveSequence = "";
        isAITurn = currentPlayer.PlayerType == PlayerType.AIPlayer;
        gameState = GameState.InProgress;
    }

    public void ReplayGame(GameTable game)
    {
        replayGame = game;
        uiManager.ShowPanel(uiManager.playPanel);
        InitBoard(game.BoardSize);
        board.DisableAllCells();
        moveSequence = game.Moves;
        gameState = GameState.InProgress;
        StartCoroutine(ReplaySequence(moveSequence));
    }

    private IEnumerator ReplaySequence(string sequence)
    {
        string[] moves = sequence.Split('-');
        for (int i = 0; i < moves.Length; i++)
        {            
            yield return new WaitForSeconds(1f);
            string[] coords = moves[i].Split(',');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            CellMark mark = i % 2 == 0 ? CellMark.X : CellMark.O;
            if (gameState != GameState.InProgress)
            {
                yield break;
            }
            board.SetCell(x, y, mark);
        }
        Debug.Log("Replay finished.");
        gameState = GameState.Finished;
    }

    public void EndGame()
    {
        gameState = GameState.Setup;
        isAIThink = false;
        Destroy(board.gameObject);
        board = null;
        ruleSystem = null;
        playerX = null;
        playerO = null;
        currentPlayer = null;
        replayGame = null;
    }

    public void RestartGame()
    {
        board.SetupBoard(ruleSystem.BoardSize);
        currentPlayer = playerX;
        isAITurn = currentPlayer.PlayerType == PlayerType.AIPlayer;
        moveSequence = "";
        gameState = GameState.InProgress;
        uiManager.ShowPanel(uiManager.playPanel);
    }

    private void HandlePlayerTurn()
    {
        if (isAITurn)
        {
            if (!isAIThink && !aiMoveReady)
            {
                HandleAIThink();
            }
            else if (!isAIThink && aiMoveReady)
            {
                if (aiNextMove.x != -1 && aiNextMove.y != -1)
                {
                    MakeMove(aiNextMove.x, aiNextMove.y);
                }
                else
                {
                    Debug.Log("No valid move available.");
                }
                aiMoveReady = false;
            }

        }
        else
        {
            return;
        }
    }

    private void HandleAIThink()
    {
        isAIThink = true;
        AIPlayer aiPlayer = currentPlayer as AIPlayer;
        ThreadPool.QueueUserWorkItem(_ =>
        {
            Vector2Int move = aiPlayer.GetNextMove();
            aiNextMove = move;
            if (gameState != GameState.InProgress)
            {
                return;
            }
            aiMoveReady = true;
            isAIThink = false;
        });
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
                moveSequence += $"{x},{y}";
                if (playerXID != playerOID)
                {
                    int winnerId = currentPlayer == playerX ? playerXID : playerOID;
                    int loserId = currentPlayer == playerX ? playerOID : playerXID;
                    PlayerTable winner = DataManager.Instance.GetPlayerRecordById(winnerId);
                    PlayerTable loser = DataManager.Instance.GetPlayerRecordById(loserId);
                    DataManager.Instance.InsertGameRecord(playerXID, playerOID, winnerId, DateTime.Now.ToString(), ruleSystem.BoardSize, ruleSystem.WinCondition, moveSequence);
                    DataManager.Instance.UpdatePlayerRecord(winnerId, winner.Wins + 1, winner.Losses, winner.Draws);
                    DataManager.Instance.UpdatePlayerRecord(loserId, loser.Wins, loser.Losses + 1, loser.Draws);
                }
                isDraw = false;
            }
            else if (board.IsBoardFull())
            {
                gameState = GameState.Finished;
                moveSequence += $"{x},{y}";
                if (playerXID != playerOID)
                {
                    int winnerId = -1; // Draw
                    PlayerTable playerX = DataManager.Instance.GetPlayerRecordById(playerXID);
                    PlayerTable playerO = DataManager.Instance.GetPlayerRecordById(playerOID);
                    DataManager.Instance.InsertGameRecord(playerXID, playerOID, winnerId, DateTime.Now.ToString(), ruleSystem.BoardSize, ruleSystem.WinCondition, moveSequence);
                    DataManager.Instance.UpdatePlayerRecord(playerXID, playerX.Wins, playerX.Losses, playerX.Draws + 1);
                    DataManager.Instance.UpdatePlayerRecord(playerOID, playerO.Wins, playerO.Losses, playerO.Draws + 1);
                }
                isDraw = true;
            }
            else
            {            
                moveSequence += $"{x},{y}-";
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

    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }
}
