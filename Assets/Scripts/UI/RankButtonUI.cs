using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RankButtonUI : MonoBehaviour
{
    private Button rankButton;
    void Awake()
    {
        rankButton = GetComponent<Button>();
        rankButton.onClick.AddListener(OnRankButtonClicked);
    }

    private void OnRankButtonClicked()
    {
        UIManager.Instance.ShowRankPanel();
    }
}
