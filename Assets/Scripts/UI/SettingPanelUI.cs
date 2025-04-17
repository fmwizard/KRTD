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
    public TextMeshProUGUI errorPrompt;
    public Button launchButton;
    
    public TMP_InputField customBoardSizeInputField;
    public TMP_InputField customWinConditionInputField;

    // Start is called before the first frame update
    void Start()
    {
        errorPrompt.gameObject.SetActive(false);
        launchButton.onClick.AddListener(OnLaunchButtonClick);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isValidCustomInput(string customBoardSize, string customWinCondition)
    {
        if (string.IsNullOrEmpty(customBoardSize) || string.IsNullOrEmpty(customWinCondition))
        {
            return false;
        }

        if (!int.TryParse(customBoardSize, out int size) || !int.TryParse(customWinCondition, out int winCondition))
        {
            return false;
        }

        if (size < 3 || size >= 10 || winCondition < 3 || winCondition > size)
        {
            return false;
        }

        return true;
    }

    private (int,int) ParseBoardDropdown()
    {
        string selectedOption = boardSizeDropdown.options[boardSizeDropdown.value].text;
        if (selectedOption == "自定义")
        {
            string customBoardSize = customBoardSizeInputField.text;
            string customWinCondition = customWinConditionInputField.text;
            if (!isValidCustomInput(customBoardSize, customWinCondition))
            {
                return (0, 0);
            }
            int bSize = int.Parse(customBoardSize);
            int bWinCondition = int.Parse(customWinCondition);
            return (bSize, bWinCondition);
        }

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
        if (size == 0 || winCondition == 0)
        {
            errorPrompt.gameObject.SetActive(true);
            return;
        }
        PlayerSetting playerX = ParsePlayerContainer(player1ToggleGroup, player1HumanOption, player1AIOption);
        PlayerSetting playerO = ParsePlayerContainer(player2ToggleGroup, player2HumanOption, player2AIOption);
        if (playerX.playerType == PlayerType.HumanPlayer && playerO.playerType == PlayerType.HumanPlayer)
        {
            if (playerX.playerName == playerO.playerName)
            {
                errorPrompt.gameObject.SetActive(true);
                return;
            }
        }
        GameSetting gameSetting = new GameSetting(size, winCondition, playerX, playerO);
        GameManager.Instance.LaunchGame(gameSetting);
        errorPrompt.gameObject.SetActive(false);
    }
}
