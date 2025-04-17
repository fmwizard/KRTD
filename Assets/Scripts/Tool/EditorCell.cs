using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditorCell : MonoBehaviour, IPointerClickHandler
{
    public Sprite xSprite;
    public Sprite oSprite;
    private int x;
    private int y;
    private Image image;
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
    public UnityEvent leftClick;
    //public UnityEvent rightClick;
    void Awake()
    {        
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        leftClick = new UnityEvent();
        // rightClick = new UnityEvent();
        leftClick.AddListener(OnLeftClick);
        // rightClick.AddListener(OnRightClick);
        
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClick.Invoke();
        }
    //     else if (eventData.button == PointerEventData.InputButton.Right)
    //     {
    //         rightClick.Invoke();
    //     }
    }

    private void OnLeftClick()
    {
        if (cellMark != CellMark.Empty)
        {
            return;
        }
        EditorManager instance = EditorManager.Instance;
        CellMark = instance.CurrentMark;
        instance.moveSequence += $"{x},{y}-";
        instance.CurrentMark = (instance.CurrentMark == CellMark.X) ? CellMark.O : CellMark.X;
    }
    // private void OnRightClick()
    // {
    //     CellMark = CellMark.O;
    // }

    public void UpdateCellSprite()
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
