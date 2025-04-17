using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public SQLiteConnection dbConnection;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        string dbPath = System.IO.Path.Combine(Application.streamingAssetsPath, "data.db");
        dbConnection = new SQLiteConnection(dbPath);
        //CreateTables();
        //dbConnection.DropTable<PlayerTable>();
        //dbConnection.DropTable<GameTable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //int a = InsertPlayerRecord("AI", 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTables()
    {
        dbConnection.CreateTable<PlayerTable>();
        dbConnection.CreateTable<GameTable>();
    }

    public int InsertPlayerRecord(string name, int wins, int losses, int draws)
    {
        PlayerTable player = new PlayerTable
        {
            Name = name,
            Wins = wins,
            Losses = losses,
            Draws = draws
        };
        dbConnection.Insert(player);
        return player.Id;
    }

    public void UpdatePlayerRecord(int playerId, int wins, int losses, int draws)
    {
        PlayerTable player = dbConnection.Table<PlayerTable>().FirstOrDefault(p => p.Id == playerId);
        if (player != null)
        {
            player.Wins = wins;
            player.Losses = losses;
            player.Draws = draws;
            dbConnection.Update(player);
        }
    }
    public void InsertGameRecord(int player1Id, int player2Id, int winnerId, string datetime, int boardSize, int winCondition, string moves, int moveStart)
    {
        GameTable game = new GameTable
        {
            Player1Id = player1Id,
            Player2Id = player2Id,
            WinnerId = winnerId,
            Datetime = datetime,
            BoardSize = boardSize,
            WinCondition = winCondition,
            Moves = moves,
            MoveStart = moveStart
        };
        dbConnection.Insert(game);
    }

    public PlayerTable GetPlayerRecordById(int playerId)
    {
        return dbConnection.Table<PlayerTable>().FirstOrDefault(p => p.Id == playerId);
    }
    public int GetPlayerIDByName(string name)
    {
        PlayerTable player = dbConnection.Table<PlayerTable>().FirstOrDefault(p => p.Name == name);
        if (player != null)
        {
            return player.Id;
        }
        return -1;
    }
    public string GetPlayerNameById(int playerId)
    {
        PlayerTable player = dbConnection.Table<PlayerTable>().FirstOrDefault(p => p.Id == playerId);
        if (player != null)
        {
            return player.Name;
        }
        return null;
    }
    public List<PlayerTable> GetAllPlayerRecords()
    {
        return dbConnection.Table<PlayerTable>().ToList();
    }
    public List<GameTable> GetGameRecordsByPlayerId(int playerId)
    {
        return dbConnection.Table<GameTable>().Where(g => g.Player1Id == playerId || g.Player2Id == playerId).ToList();
    }

    public int CalculateScore(PlayerTable player)
    {
        return player.Wins * 3 + player.Draws;
    }
}

public class PlayerTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Draws { get; set; }
}

public class GameTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Player1Id { get; set; }
    public int Player2Id { get; set; }
    public int WinnerId { get; set; }
    public string Datetime { get; set; }
    public int BoardSize { get; set; }
    public int WinCondition { get; set; }
    public string Moves { get; set; }
    public int MoveStart { get; set; }
}
