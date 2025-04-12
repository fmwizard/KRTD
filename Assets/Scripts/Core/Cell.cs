using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite xSprite;
    [SerializeField] private Sprite oSprite;
    private int x;
    private int y;
    private Image image;
    private Button button;
    private CellMark cellMark;
    public CellMark CellMark
    {
        get { return cellMark; }
        set
        {
            cellMark = value;
            UpdateCellSprite();
        }
    }

    void Awake()
    {        
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitCell(int x, int y)
    {
        this.x = x;
        this.y = y;
        CellMark = CellMark.Empty;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCellClicked);
    }

    private void OnCellClicked()
    {
        if (cellMark == CellMark.Empty)
        {
            GameManager.Instance.OnCellClicked(x, y);
        }
    }

    private void UpdateCellSprite()
    {
        switch (cellMark)
        {
            case CellMark.X:
                image.sprite = xSprite;
                break;
            case CellMark.O:
                image.sprite = oSprite;
                break;
            default:
                image.sprite = null;
                break;
        }
    }
}
