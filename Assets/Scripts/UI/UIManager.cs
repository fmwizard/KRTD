using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private bool isPaused = false;
    public GameObject menuPanel, settingPanel, gamePanel, gameOverPanel, pausePanel;
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
        gamePanel.SetActive(false);
        settingPanel.SetActive(false);
        panel.SetActive(true);
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
}
