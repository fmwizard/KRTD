using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToggleUI : MonoBehaviour
{
    public Toggle humanToggle;
    public GameObject playerOption;

    public Toggle aiToggle;
    public GameObject aiOption;
    // Start is called before the first frame update
    void Start()
    {
        
        humanToggle.onValueChanged.AddListener(OnHumanToggleChanged);
        aiToggle.onValueChanged.AddListener(OnAIToggleChanged);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnHumanToggleChanged(bool isOn)
    {
        if (isOn)
        {
            playerOption.SetActive(true);
            aiOption.SetActive(false);
        }
    }

    private void OnAIToggleChanged(bool isOn)
    {
        if (isOn)
        {
            playerOption.SetActive(false);
            aiOption.SetActive(true);
        }
    }    
}
