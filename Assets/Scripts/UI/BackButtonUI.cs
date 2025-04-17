using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButtonUI : MonoBehaviour
{
    private Button backButton;
    
    void Awake()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        GameManager gmInstance = GameManager.Instance;
        if (gmInstance.gameState == GameState.Finished)
        {
            gmInstance.EndGame();
        }
        UIManager.Instance.ShowPanel(UIManager.Instance.menuPanel);
    }
}
