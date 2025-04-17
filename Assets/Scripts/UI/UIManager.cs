using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private bool isPaused = false;
    public GameObject menuPanel, settingPanel, rankPanel, recordPanel, 
        gamePanel, gameOverPanel, pausePanel, playPanel;
    public GameObject rankContainer;
    public GameObject playerRecordPrefab;
    public GameObject rankTitlePrefab;
    public GameObject recordContainer;
    public GameObject recordEntryPrefab;
    public GameObject recordTitlePrefab;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowPausePanel(isPaused);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        // if (gameState == GameState.InProgress)
        // {
        //     gameState = GameState.Setup;
        //     uiManager.ShowPanel(uiManager.pausePanel);
        // }
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        ShowPausePanel(isPaused);
    }

    public void ShowPanel(GameObject panel)
    {
        menuPanel.SetActive(false);
        playPanel.SetActive(false);
        settingPanel.SetActive(false);
        rankPanel.SetActive(false);
        recordPanel.SetActive(false);
        panel.SetActive(true);
    }

    public void SetGameBoardUI(int size)
    {
        GridLayoutGroup gridLayout = gamePanel.GetComponent<GridLayoutGroup>();
        float cellLength = gamePanel.GetComponent<RectTransform>().rect.width - (size - 1) * gridLayout.spacing.x;
        gridLayout.cellSize = new Vector2(cellLength / size, cellLength / size);
    }

    public void ShowPausePanel(bool show)
    {
        if (show)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void ShowRankPanel()
    {
        ClearRankPanel();
        DataManager instance = DataManager.Instance;
        List<PlayerTable> players = instance.GetAllPlayerRecords();
        // rank by score
        if (players.Count > 0)
        {
            players.Sort((x, y) => instance.CalculateScore(y).CompareTo(instance.CalculateScore(x)));
        }
        Instantiate(rankTitlePrefab, rankContainer.transform);
        for (int i = 0; i < players.Count; i++)
        {
            PlayerTable player = players[i];
            // if (player.Name == "AI")
            // {
            //     continue;
            // }
            GameObject playerRecord = Instantiate(playerRecordPrefab, rankContainer.transform);
            PlayerRecordUI playerRecordUI = playerRecord.GetComponent<PlayerRecordUI>();
            playerRecordUI.SetPlayerRecord(i + 1, player);
        }
        ShowPanel(rankPanel);
    }

    public void ClearRankPanel()
    {
        foreach (Transform child in rankContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ShowRecordPanel(PlayerTable player)
    {
        ClearRecordPanel();
        DataManager instance = DataManager.Instance;
        List<GameTable> records = instance.GetGameRecordsByPlayerId(player.Id);
        Instantiate(recordTitlePrefab, recordContainer.transform);
        for (int i = 0; i < records.Count; i++)
        {
            GameTable record = records[i];
            GameObject recordEntry = Instantiate(recordEntryPrefab, recordContainer.transform);
            RecordEntryUI recordEntryUI = recordEntry.GetComponent<RecordEntryUI>();
            recordEntryUI.SetRecordEntry(record, player);
        }
        ShowPanel(recordPanel);
    }

    public void ClearRecordPanel()
    {
        foreach (Transform child in recordContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
