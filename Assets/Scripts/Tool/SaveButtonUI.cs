using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveButtonUI : MonoBehaviour
{
    private Button saveButton;
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
        EditorManager.Instance.SerializeEditorBoard();
    }
}
