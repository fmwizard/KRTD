using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerSetting
{
    public PlayerType playerType;
    public string playerName;
    public Difficulty difficulty;
    public PlayerSetting(PlayerType playerType, string playerName, Difficulty difficulty)
    {
        this.playerType = playerType;
        this.playerName = playerName;
        this.difficulty = difficulty;
    }
}
public class GameSetting
{
    public int boardSize;
    public int winCondition;
    public PlayerSetting playerX;
    public PlayerSetting playerO;
    public GameSetting(int boardSize, int winCondition, PlayerSetting playerX, PlayerSetting playerO)
    {
        this.boardSize = boardSize;
        this.winCondition = winCondition;
        this.playerX = playerX;
        this.playerO = playerO;
    }

}
