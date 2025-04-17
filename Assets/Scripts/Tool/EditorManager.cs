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
    public string saveFileName = "editor_level.json";

    public static EditorManager Instance { get; private set; }
    public GameObject editorCellPrefab;
    public GameObject editorCellContainer;
    private EditorBoard editorBoard;
    private CellMark currentMark;
    public CellMark CurrentMark
    {
        get { return currentMark; }
        set { currentMark = value; }
    }
    [HideInInspector]
    public string moveSequence;

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
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, saveFileName);
        Serialization.saveFileName = saveFileName;
        Serialization.SaveJsonToFile(json, filePath);
    }


    private void SetEditorBoardUI(int size)
    {
        GridLayoutGroup gridLayout = editorCellContainer.GetComponent<GridLayoutGroup>();
        float cellLength = editorCellContainer.GetComponent<RectTransform>().rect.width - (size - 1) * gridLayout.spacing.x;
        gridLayout.cellSize = new Vector2(cellLength / size, cellLength / size);
    }

    public void UndoMove()
    {
        if (moveSequence.Length > 0)
        {
            string[] moves = moveSequence.Split('-')[0..^1];
            moveSequence = string.Join("-", moves, 0, moves.Length - 1);
            if (moveSequence.Length > 0)
            {
                moveSequence += "-";
            }
            else
            {
                moveSequence = "";
            }
            string lastMove = moves[moves.Length - 1];
            string[] coordinates = lastMove.Split(',');
            int x = int.Parse(coordinates[0]);
            int y = int.Parse(coordinates[1]);
            editorBoard.GetEditorCell(x, y).CellMark = CellMark.Empty;
            currentMark = (currentMark == CellMark.X) ? CellMark.O : CellMark.X;
        }
    }
}
