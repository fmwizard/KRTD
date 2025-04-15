using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerRecordUI : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI name;
    public TextMeshProUGUI wins;
    public TextMeshProUGUI losses;
    public TextMeshProUGUI draws;
    public TextMeshProUGUI score;

    public void SetPlayerRecord(int playerRank, PlayerTable player)
    {
        rank.text = playerRank.ToString();
        name.text = player.Name;
        wins.text = player.Wins.ToString();
        losses.text = player.Losses.ToString();
        draws.text = player.Draws.ToString();
        score.text = DataManager.Instance.CalculateScore(player).ToString();
    }
}
