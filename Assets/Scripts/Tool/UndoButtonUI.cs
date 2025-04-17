using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UndoButtonUI : MonoBehaviour
{
    private Button undoButton;
    void Awake()
    {
        undoButton = GetComponent<Button>();
        undoButton.onClick.AddListener(OnUndoButtonClick);
    }
    
    private void OnUndoButtonClick()
    {
        EditorManager.Instance.UndoMove();
    }
}
