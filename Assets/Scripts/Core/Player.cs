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
