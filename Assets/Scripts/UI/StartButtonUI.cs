using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartButtonUI : MonoBehaviour
{
    private Button startButton;
    
    void Awake()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClicked);
    }
    
    private void OnStartButtonClicked()
    {
        UIManager.Instance.ShowPanel(UIManager.Instance.settingPanel);
    }
}
