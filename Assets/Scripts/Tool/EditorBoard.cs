using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BoardData
{
    public int boardSize;
    public int winCondition;
    public string[][] initialState;
    public string currentPlayer;
}

public class EditorBoard : MonoBehaviour
{
    private GameObject cellPrefab;
    private GameObject cellContainer;
    private int size;
    public int Size
    {
        get { return size; }
        set
        {
            size = value;
            InitializeBoard();
        }
    }
    private EditorCell[,] cells;
    public void SetupBoard(int size)
    {        
        cellPrefab = EditorManager.Instance.editorCellPrefab;
        cellContainer = EditorManager.Instance.editorCellContainer;
        Size = size;
    }
    
    private void InitializeBoard()
    {
        foreach (Transform child in cellContainer.transform)
        {
            Destroy(child.gameObject);
        }

        cells = new EditorCell[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, cellContainer.transform);
                EditorCell cell = cellObject.GetComponent<EditorCell>();
                cell.InitCell(x, y);
                cells[x, y] = cell;
            }
        }
    }

    public EditorCell GetEditorCell(int x, int y)
    {
        if (x >= 0 && x < size && y >= 0 && y < size)
        {
            return cells[x, y];
        }
        return null;
    }

    public void SetEditorCell(int x, int y, CellMark mark)
    {
        if (x >= 0 && x < size && y >= 0 && y < size)
        {
            cells[x, y].CellMark = mark;
        }
    }

    public List<Vector2Int> GetAllEmptyEditorCells()
    {
        List<Vector2Int> emptyEditorCells = new List<Vector2Int>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (cells[x, y].CellMark == CellMark.Empty)
                {
                    emptyEditorCells.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyEditorCells;
    }

    public bool IsBoardFull()
    {
        foreach (EditorCell cell in cells)
        {
            if (cell.CellMark == CellMark.Empty)
            {
                return false;
            }
        }
        return true;
    }

    public string[][] GetBoardState()
    {
        string[][] boardState = new string[size][];
        for (int x = 0; x < size; x++)
        {
            boardState[x] = new string[size];
            for (int y = 0; y < size; y++)
            {
                if (cells[x, y].CellMark == CellMark.X)
                    boardState[x][y] = "X";
                else if (cells[x, y].CellMark == CellMark.O)
                    boardState[x][y] = "O";
                else
                    boardState[x][y] = "E";
            }
        }
        return boardState;
    }
}
