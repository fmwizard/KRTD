using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class SettingPanelUI : MonoBehaviour
{
    public TMP_Dropdown boardSizeDropdown;
    public ToggleGroup player1ToggleGroup;
    public ToggleGroup player2ToggleGroup;
    public GameObject player1HumanOption;
    public GameObject player1AIOption;
    public GameObject player2HumanOption;
    public GameObject player2AIOption;
    public Button launchButton;

    // Start is called before the first frame update
    void Start()
    {

        launchButton.onClick.AddListener(OnLaunchButtonClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private (int,int) ParseBoardDropdown()
    {
        string selectedOption = boardSizeDropdown.options[boardSizeDropdown.value].text;
        int size = int.Parse(selectedOption.Split('x')[0]);
        int winCondition;
        switch (size)
        {
            case 3:
                winCondition = 3;
                break;
            case 4:
                winCondition = 4;
                break;
            case 5:
                winCondition = 4;
                break;
            default:
                winCondition = 3;
                break;
        }
        return (size, winCondition);
    }

    private PlayerSetting ParsePlayerContainer(ToggleGroup toggleGroup, GameObject humanOption, GameObject aiOption)
    {
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();

        if (activeToggle.name == "Human")
        {
            string playerName = humanOption.GetComponentInChildren<TMP_InputField>().text;
            return new PlayerSetting(PlayerType.HumanPlayer, playerName, Difficulty.Easy);
        }
        else
        {
            Difficulty difficulty = Difficulty.Easy;
            ToggleGroup aiToggleGroup = aiOption.GetComponentInChildren<ToggleGroup>();
            Toggle aiLevelToggle = aiToggleGroup.ActiveToggles().FirstOrDefault();
            if (aiLevelToggle.name == "EasyToggle")
            {
                difficulty = Difficulty.Easy;
            }
            else if (aiLevelToggle.name == "MediumToggle")
            {
                difficulty = Difficulty.Medium;
            }
            else
            {
                difficulty = Difficulty.Hard;
            }
            return new PlayerSetting(PlayerType.AIPlayer, "", difficulty);
        }

    }

    public void OnLaunchButtonClick()
    {
        (int size, int winCondition) = ParseBoardDropdown();
        PlayerSetting playerX = ParsePlayerContainer(player1ToggleGroup, player1HumanOption, player1AIOption);
        PlayerSetting playerO = ParsePlayerContainer(player2ToggleGroup, player2HumanOption, player2AIOption);

        GameSetting gameSetting = new GameSetting(size, winCondition, playerX, playerO);
        GameManager.Instance.LaunchGame(gameSetting);
    }
}
