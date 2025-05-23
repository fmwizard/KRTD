using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour
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
    private Cell[,] cells;

    void Start()
    {


    }

    public void SetupBoard(int size)
    {        
        cellPrefab = GameManager.Instance.cellPrefab;
        cellContainer = GameManager.Instance.cellContainer;
        Size = size;
    }
    
    public void SetupBoardWithJson(string[][] stateData)
    {
        cellPrefab = GameManager.Instance.cellPrefab;
        cellContainer = GameManager.Instance.cellContainer;

        foreach (Transform child in cellContainer.transform)
        {
            Destroy(child.gameObject);
        }

        size = stateData.Length;
        cells = new Cell[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, cellContainer.transform);
                Cell cell = cellObject.GetComponent<Cell>();
                cell.InitCell(x, y);
                cells[x, y] = cell;

                // Set the cell mark based on the JSON data
                if (stateData[x][y] == "X")
                {
                    cell.CellMark = CellMark.X;
                }
                else if (stateData[x][y] == "O")
                {
                    cell.CellMark = CellMark.O;
                }
                else
                {
                    cell.CellMark = CellMark.Empty;
                }
                
                cell.UpdateCellSprite();
            }
        }
    }

    private void InitializeBoard()
    {
        foreach (Transform child in cellContainer.transform)
        {
            Destroy(child.gameObject);
        }

        cells = new Cell[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject cellObject = Instantiate(cellPrefab, cellContainer.transform);
                Cell cell = cellObject.GetComponent<Cell>();
                cell.InitCell(x, y);
                cells[x, y] = cell;
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < size && y >= 0 && y < size)
        {
            return cells[x, y];
        }
        return null;
    }

    public void SetCell(int x, int y, CellMark mark, bool updateUI = true)
    {
        if (x >= 0 && x < size && y >= 0 && y < size)
        {
            cells[x, y].CellMark = mark;
            if (updateUI)
            {
                cells[x, y].UpdateCellSprite();
            }
        }
    }

    public List<Vector2Int> GetAllEmptyCells()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (cells[x, y].CellMark == CellMark.Empty)
                {
                    emptyCells.Add(new Vector2Int(x, y));
                }
            }
        }
        return emptyCells;
    }

    public bool IsBoardFull()
    {
        foreach (Cell cell in cells)
        {
            if (cell.CellMark == CellMark.Empty)
            {
                return false;
            }
        }
        return true;
    }

    public void DisableAllCells()
    {
        foreach (Cell cell in cells)
        {
            cell.GetComponent<Button>().enabled = false;
        }
    }
}
