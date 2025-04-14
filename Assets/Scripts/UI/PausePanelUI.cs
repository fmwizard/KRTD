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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackToMenuButtonClicked()
    {
        UIManager.Instance.ShowPanel(UIManager.Instance.menuPanel);
    }
}
