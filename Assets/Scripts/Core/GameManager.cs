using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum CellMark { Empty, X, O }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Action<int> SetupBoard;
    public int boardSize = 3;

    private Player currentPlayer;
    private void Awake()
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
        SetupBoard?.Invoke(boardSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
