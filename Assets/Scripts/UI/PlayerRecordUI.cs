using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerRecordUI : MonoBehaviour
{
    public TextMeshProUGUI rank;
    public TextMeshProUGUI name;
    public TextMeshProUGUI wins;
    public TextMeshProUGUI losses;
    public TextMeshProUGUI draws;
    public TextMeshProUGUI score;
    public Button recordButton;
    public void SetPlayerRecord(int playerRank, PlayerTable player)
    {
        rank.text = playerRank.ToString();
        name.text = player.Name;
        wins.text = player.Wins.ToString();
        losses.text = player.Losses.ToString();
        draws.text = player.Draws.ToString();
        score.text = DataManager.Instance.CalculateScore(player).ToString();
        recordButton.onClick.AddListener(() => OnRecordButtonClicked(player));
    }

    public void OnRecordButtonClicked(PlayerTable player)
    {
        UIManager.Instance.ShowRecordPanel(player);
    }
}
