using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuitButtonUI : MonoBehaviour
{
    private Button quitButton;
    void Awake()
    {
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
