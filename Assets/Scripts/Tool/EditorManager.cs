using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class EditorManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardSize;
    public int winCondition;


    public static EditorManager Instance { get; private set; }
    public GameObject editorCellPrefab;
    public GameObject editorCellContainer;
    private EditorBoard editorBoard;
    public string moveSequence;
    private CellMark currentMark;
    public CellMark CurrentMark
    {
        get { return currentMark; }
        set { currentMark = value; }
    }
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
        InitBoard(boardSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitBoard(int boardSize)
    {
        GameObject editorBoardObject = new GameObject("EditorBoard");
        editorBoard = editorBoardObject.AddComponent<EditorBoard>();        
        SetEditorBoardUI(boardSize);
        editorBoard.SetupBoard(boardSize);
        currentMark = CellMark.X;
        moveSequence = "";
    }

    public void SerializeEditorBoard()
    {
        BoardData boardData = new BoardData
        {
            boardSize = boardSize,
            winCondition = winCondition,
            initialState = editorBoard.GetBoardState(),
            currentPlayer = currentMark.ToString(),
            moveSequence = moveSequence
        };
        string json = Serialization.SaveBoardToJson(boardData);
        string fileName = "editor_level.json";
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);
        Serialization.SaveJsonToFile(json, filePath);
    }


    private void SetEditorBoardUI(int size)
    {
        GridLayoutGroup gridLayout = editorCellContainer.GetComponent<GridLayoutGroup>();
        float cellLength = editorCellContainer.GetComponent<RectTransform>().rect.width - (size - 1) * gridLayout.spacing.x;
        gridLayout.cellSize = new Vector2(cellLength / size, cellLength / size);
    }
}
