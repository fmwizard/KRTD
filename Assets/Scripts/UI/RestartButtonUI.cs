using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonUI : MonoBehaviour
{
    private Button restartButton;
    void Awake()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnRestartButtonClicked()
    {
        GameManager instance = GameManager.Instance;
        if (instance.replayGame != null)
        {
            instance.ReplayGame(instance.replayGame);
        }
        else
        {
            instance.RestartGame();
        }
    }
}
