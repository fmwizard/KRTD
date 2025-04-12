using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    private CellMark playerMark;
    public string playerName;
    public Player(string name, CellMark mark)
    {
        playerName = name;
        playerMark = mark;
    }
    public CellMark PlayerMark
    {
        get { return playerMark; }
        set { playerMark = value; }
    }
}