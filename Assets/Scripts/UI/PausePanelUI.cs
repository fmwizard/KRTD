using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PausePanelUI : MonoBehaviour
{
    public Button backToMenuButton;
    public Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackToMenuButtonClicked()
    {
        UIManager.Instance.TogglePause();
        GameManager gmInstance = GameManager.Instance;
        if (gmInstance.gameState == GameState.InProgress)
        {
            gmInstance.EndGame();
        }
        UIManager.Instance.ShowPanel(UIManager.Instance.menuPanel);
    }

    public void OnQuitButtonClicked()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
