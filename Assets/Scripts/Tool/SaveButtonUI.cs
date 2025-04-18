using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveButtonUI : MonoBehaviour
{
    private Button saveButton;
    public TextMeshProUGUI errorPrompt;
    void Awake()
    {
        saveButton = GetComponent<Button>();
        saveButton.onClick.AddListener(OnSaveButtonClick);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSaveButtonClick()
    {
        EditorManager instance = EditorManager.Instance;
        int validationResult = instance.GetEditorBoard().ValidateEditorBoard();
        if (validationResult == 1)
        {
            ShowErrorPrompt("保存成功!", Color.green);
            instance.SerializeEditorBoard();
        }
        else if (validationResult == 0)
        {
            ShowErrorPrompt("不应保存满的棋盘!", Color.red);
        }
        else if (validationResult == -1)
        {
            ShowErrorPrompt("不应保存空的棋盘!", Color.red);
        }
        else if (validationResult == -2)
        {
            ShowErrorPrompt("不应保存已有一方胜利的棋盘!", Color.red);
        }
    }

    private void ShowErrorPrompt(string message, Color color)
    {
        errorPrompt.color = color;
        errorPrompt.text = message;
        errorPrompt.gameObject.SetActive(true);
    }
}
