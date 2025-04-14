using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public GameObject menuPanel, setttingPanel, gamePanel, gameOverPanel;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPanel(GameObject panel)
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        panel.SetActive(true);
    }
}
