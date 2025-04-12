using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
