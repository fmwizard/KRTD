using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RecordEntryUI : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI type;
    public TextMeshProUGUI result;
    public TextMeshProUGUI datetime;
    public Button replayButton;
    
    public void SetRecordEntry(GameTable game, PlayerTable queryPlayer)
    {
        int opponentId = game.Player1Id == queryPlayer.Id ? game.Player2Id : game.Player1Id;
        name.text = DataManager.Instance.GetPlayerNameById(opponentId);
        type.text = game.Player1Id == queryPlayer.Id ? "先手(X)" : "后手(O)";
        result.text = game.WinnerId == queryPlayer.Id ? "胜" : game.WinnerId == -1 ? "平" : "负";
        datetime.text = game.Datetime;
        SetReplayButton(game);
    }

    private void SetReplayButton(GameTable game)
    {
        replayButton.onClick.RemoveAllListeners();
        replayButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ReplayGame(game);
        });
    }
}
