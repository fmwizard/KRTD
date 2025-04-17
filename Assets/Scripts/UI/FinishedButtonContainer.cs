using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FinishedButtonContainer : MonoBehaviour
{
    public GameObject finishedButtonContainer;
    public TextMeshProUGUI finishedText;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        finishedButtonContainer.SetActive(false);
        finishedText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameState.Finished)
        {
            if (!isActive)
            {
                finishedButtonContainer.SetActive(true);
                isActive = true;
                SetFinishedText();
            }
        }
        else
        {
            if (isActive)
            {
                finishedButtonContainer.SetActive(false);
                isActive = false;
                finishedText.text = "";
            }
        }
    }

    private void SetFinishedText()
    {
        GameManager instance = GameManager.Instance;
        if (instance.replayGame != null)
        {
            finishedText.text = "回放结束";
        }
        else
        {
            if (instance.isDraw)
            {
                finishedText.text = "平局";
            }
            else
            {
                Player winner = instance.GetCurrentPlayer();
                if (winner.PlayerType == PlayerType.HumanPlayer)
                {
                    HumanPlayer humanPlayer = winner as HumanPlayer;
                    finishedText.text = "" + humanPlayer.PlayerName + "胜利";
                }
                else
                {
                    finishedText.text = "AI胜利";
                }
            }
        }
    }
}
