using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player 
{
    private CellMark playerMark;
    private PlayerType playerType;
    public CellMark PlayerMark
    {
        get { return playerMark; }
        set { playerMark = value; }
    }
    public PlayerType PlayerType
    {
        get { return playerType; }
        set { playerType = value; }
    }
    public Player(CellMark mark)
    {
        playerMark = mark;
    }
}

public class HumanPlayer : Player
{
    private string playerName;
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }
    public HumanPlayer(CellMark mark, string name) : base(mark)
    {
        playerName = name;
        PlayerType = PlayerType.HumanPlayer;
    }
}

public class AIPlayer : Player
{
    private Difficulty difficulty;
    private Board board;
    private RuleSystem ruleSystem;
    private StrategySystem StrategySystem;

    public AIPlayer(CellMark mark, Difficulty difficulty, Board board, RuleSystem ruleSystem) : base(mark)
    {
        this.difficulty = difficulty;
        this.board = board;
        this.ruleSystem = ruleSystem;
        PlayerType = PlayerType.AIPlayer;
        ChooseStrategy();
    }

    private void ChooseStrategy()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                StrategySystem = new EasyStrategy();
                break;
            case Difficulty.Medium:
                StrategySystem = new MediumStrategy();
                break;
        }
    }

    public Vector2Int GetNextMove()
    {
        return StrategySystem.GetNextMove(board, PlayerMark, ruleSystem);
    }
}