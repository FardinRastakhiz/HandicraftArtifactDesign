///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <RightClick>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class RightClick : MonoBehaviour {

    static Transform activeShape;
    public static bool wasDragging;
    static GameObject rightClick;
    public static Vector3 mousePosition;
    static bool jumpNext;

    [Header("Order tools")]
    [SerializeField]
    Button _moveForward;
    [SerializeField]
    Button _moveBackward;
    [SerializeField]
    Button _oneStepForward;
    [SerializeField]
    Button _oneStepBackward;

    [Space]
    [Header("Mirror tools")]
    [SerializeField]
    Button _horizontalMirror;

    [Space]
    [Header("Copy tools")]
    [SerializeField]
    Button _duplicate;
    [SerializeField]
    Button _cut;
    [SerializeField]
    Button _copy;
    [SerializeField]
    Button _paste;

    [Space]
    [Header("Color tools")]
    [SerializeField]
    Button _colorBoard;
    [SerializeField]
    Button _colorPicker;
    [SerializeField]
    Image _colorPalette;
    [SerializeField]
    GameObject _colorTools;

    [Space]
    [Header("Delete")]
    [SerializeField]
    Button _delete;

    //Order tools
    static Button moveForward;
    static Button moveBackward;
    static Button oneStepForward;
    static Button oneStepBackward;

    //Mirror tools
    static Button horizontalMirror;

    //Copy tools
    static Button duplicate;
    static Button cut;
    static Button copy;
    static Button paste;

    //Color tools
    static Button colorBoard;
    static Button colorPicker;
    static Image colorPalette;
    static GameObject colorTools;

    //Delete
    static Button delete;

    static bool colorMethodAdded;
    static Fardin.ColorTools.ColorPick colorPick;
    static Fardin.ColorTools.ColorTerminal colorTerminal;

    static bool controlWindow;

    void Awake()
    {
        moveForward = _moveForward;
        moveBackward = _moveBackward;
        oneStepForward = _oneStepForward;
        oneStepBackward = _oneStepBackward;

        horizontalMirror = _horizontalMirror;

        duplicate = _duplicate;
        copy = _copy;
        cut = _cut;
        paste = _paste;

        colorBoard = _colorBoard;
        colorPicker = _colorPicker;
        colorPalette = _colorPalette;
        colorTools = _colorTools;

        delete = _delete;

        colorTerminal = colorTools.gameObject.GetComponent<Fardin.ColorTools.ColorTerminal>();
        colorPick = colorPicker.gameObject.GetComponent<Fardin.ColorTools.ColorPick>();
    }

    void Update()
    {
        if (colorPick.isPicking)
        {
            colorPalette.color = colorPick.TokenColor();
            if (!colorTerminal.gameObject.activeSelf)
                ColorTools.SetSelectionsColor(colorPalette.color);
        }
    }
    public static void UpdateRightClick(Rect paletteBoard)
    {
        if ((Input.GetMouseButtonUp(1) || jumpNext) && ToolsUtility.canMoveShape == true)
        {
            if (!StartRightClick())
                return;
            controlWindow = false;
            jumpNext = false;
            if (OnGrid.isDragging)
            {
                OnGrid.isDragging = false;
                return;
            }
            if (paletteBoard.Contains(mousePosition))
            {
                for (int i = BoardPlans.ordersList.Count - 1; i >= 0; i--)
                {
                    BoardPlan activePlan = BoardPlans.inOrders[BoardPlans.ordersList[i]];
                    Rect rect = RectTools.ToScreen(activePlan.board.GetComponent<RectTransform>());
                    if (rect.Contains(mousePosition))
                    {
                        BoardPlans.ActiveIndex = BoardPlans.boardPlans.IndexOf(activePlan);
                        RectTransform topRect = activePlan.board.transform.Find("ControlWindow").Find("Drag").GetComponent<RectTransform>();
                        if (RectTools.ToScreen(topRect).Contains(mousePosition))
                        {
                            controlWindow = true;
                        }
                        On_Right_Click();
                        return;
                    }
                }
                if (BoardPlans.ActiveIndex == -1)
                {
                    On_Right_Click();
                }
            }
        }
    }
    public static void On_Right_Click()
    {
        if (ToolsUtility.highlightedShape!=null)
        {
            activeShape = ToolsUtility.highlightedShape;
            OnShapeRightClick();
        }
        else
        {
            if (controlWindow)
            {
                OnWindowRightClick();
            }
            else
            {
                SelectTools.ResetTotal();
                OnFreeRightClick();
            }
        }
    }

    static GameObject rightClickB;
    static void OnShapeRightClick(){
        SetRightClickObject();

        //Order tools
        SetButton(moveForward, delegate { MoveForward(); });
        SetButton(moveBackward, delegate { MoveBackward(); });
        SetButton(oneStepForward, delegate { OneStepForward(); });
        SetButton(oneStepBackward, delegate { OneStepBackward(); });

        //Mirror tools
        SetButton(horizontalMirror, delegate { HorizontalMirror(); });

        //Copy tools
        SetButton(duplicate, delegate { Duplicate(); });
        SetButton(copy, delegate { CopyInstances(); });
        SetButton(cut, delegate { CutInstances(); });
        if (ClipBoard.instances == null)
            paste.interactable = false;
        else
            SetButton(paste, delegate { PasteInstances(); });

        colorBoard.interactable = false;
        colorPicker.interactable = false;
        colorTools.SetActive(false);
        //Color tools
        SetButton(colorBoard, delegate { ShowColorBoard(); });
        SetButton(colorPicker, delegate { });

        //Delete
        SetButton(delete, delegate { DeleteSelects(); });
        
    }

    static void OnFreeRightClick()
    {
        SetRightClickObject();
        moveForward.interactable = false;
        moveBackward.interactable = false;
        oneStepForward.interactable = false;
        oneStepBackward.interactable = false;

        horizontalMirror.interactable = false;

        duplicate.interactable = false;
        copy.interactable = false;
        cut.interactable = false;
        if (ClipBoard.instances == null)
            paste.interactable = false;
        else
            SetButton(paste, delegate { PasteInstances(); });


        colorBoard.interactable = false;

        delete.interactable = false;

    }

    static void OnWindowRightClick()
    {
        SetRightClickObject();

        SetButton(moveForward, delegate { WindowMoveForward(); });
        SetButton(moveBackward, delegate { WindowMoveBackward(); });
        SetButton(oneStepForward, delegate { WindowOneStepForward(); });
        SetButton(oneStepBackward, delegate { WindowOneStepBackward(); });

        horizontalMirror.interactable = false;

        duplicate.interactable = false;
        copy.interactable = false;
        cut.interactable = false;
        if (ClipBoard.instances == null)
            paste.interactable = false;
        else
            SetButton(paste, delegate { PasteInstances(); });



        delete.interactable = false;
    }

    static void SetButton(Button button, UnityEngine.Events.UnityAction call)
    {
        //Button button = rightClickB.transform.Find(name).GetComponent<Button>();
        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(call);
    }


    static void MoveForward()
    {
        SelectTools.RefineSelection(activeShape);
        SetShapesPriority.MoveForward();
        ShowRightClick(false);
    }
    static void MoveBackward()
    {
        SelectTools.RefineSelection(activeShape);
        SetShapesPriority.MoveBackward();
        ShowRightClick(false);
    }
    static void OneStepForward()
    {
        SelectTools.RefineSelection(activeShape);
        GameObject.FindGameObjectWithTag("Canvas").transform.
            Find("rightClick").gameObject.SetActive(SetShapesPriority.OneStepForward());
    }
    static void OneStepBackward()
    {
        GameObject.FindGameObjectWithTag("Canvas").transform.
            Find("rightClick").gameObject.SetActive(SetShapesPriority.OneStepBackward());
    }

    static void WindowMoveForward()
    {
        SetWindowPriority.MoveForward();
        ShowRightClick(false);
    }
    static void WindowMoveBackward()
    {
        SetWindowPriority.MoveBackward();
        ShowRightClick(false);
    }
    static void WindowOneStepForward()
    {
        ShowRightClick(SetWindowPriority.OneStepForward());
    }
    static void WindowOneStepBackward()
    {
        ShowRightClick(SetWindowPriority.OneStepBackward());
    }



    static void HorizontalMirror()
    {
        SelectTools.RefineSelection(activeShape);
        MirrorTools.HorizontalMirror();
        ShowRightClick(false);
    }

    static void Duplicate()
    {
        SelectTools.RefineSelection(activeShape);
        DuplicateShapes.Duplicate();
        ShowRightClick(false);
    }
    public static void CopyInstances()
    {
        SelectTools.RefineSelection(activeShape);
        CopyTool.TakeShapes();
        ShowRightClick(false);
    }
    public static void CutInstances()
    {
        SelectTools.RefineSelection(activeShape);
        CutTool.TakeShapes();
        ShowRightClick(false);
    }
    public static void PasteInstances()
    {
        PasteTool.DropShapes(ClipBoard.mousePosition);
        ShowRightClick(false);
    }

    public static void ShowColorBoard()
    {
        if (!colorMethodAdded)
        {
            colorTerminal.changedColor += On_Color_Change;
            colorMethodAdded = true;
        }
        SelectTools.RefineSelection(activeShape);
        colorTerminal.starterColor = colorPalette.color;
        colorTools.SetActive(true);
        RectTransform colorRect = colorTools.GetComponent<RectTransform>();
        RectTransform workingRect = rightClick.GetComponent<RectTransform>();
        RectTransform rightRect = rightClickB.GetComponent<RectTransform>();
        Vector2 colorSize = new Vector2(colorRect.rect.width,colorRect.rect.height);
        Vector3 targetPos = rightRect.localPosition + new Vector3(colorSize.x, -colorSize.y - rightRect.rect.height) / 2;
        Vector3 bottomPoint = targetPos + new Vector3(colorSize.x, -colorSize.y) / 2.0f;
        workingRect.rect.Contains(bottomPoint);
        if (bottomPoint.y < -0.5f * workingRect.rect.height)
        {
            targetPos.y = targetPos.y - 0.5f * workingRect.rect.height - bottomPoint.y;
            if (bottomPoint.x <= 0.5f * workingRect.rect.width)
                targetPos.x = rightRect.localPosition.x  + 0.5f * (rightRect.rect.width + colorSize.x);
        }
        if (bottomPoint.x > 0.5f * workingRect.rect.width)
            targetPos.x = rightRect.localPosition.x  - 0.5f * (rightRect.rect.width + colorSize.x);
        colorRect.localPosition = targetPos;
    }

    public static void OnPickingColor(Color color)
    {
        SelectTools.RefineSelection(activeShape);

        //HideRightClick();
    }

    static void DeleteSelects()
    {
        SelectTools.RefineSelection(activeShape);
        DeleteShapes.Delete();
        ShowRightClick(false);
    }

    static GameObject Filter;
    static bool StartRightClick()
    {

        rightClick = GameObject.FindGameObjectWithTag("Canvas").transform.Find("rightClick").gameObject;
        Filter = rightClick.transform.parent.Find("EndRightClick").gameObject;
        if (rightClick.activeSelf)
        {
            rightClick.SetActive(false);
            Filter.SetActive(false);
            jumpNext = true;
            return false;
        }
        mousePosition = Input.mousePosition;
        return true;
    }

    static void SetRightClickObject()
    {
        rightClick.SetActive(true);
        Filter.SetActive(true);
        if (activeShape!=null)
            colorPalette.color = activeShape.GetComponent<Image>().color;
        rightClickB = rightClick.transform.Find("rightClick").gameObject;
        RectTransform rightRect = rightClickB.GetComponent<RectTransform>();
        Rect rect = rightRect.rect;
        rightClickB.GetComponent<RectTransform>().position = Input.mousePosition +
            new Vector3(rect.width, rect.height, 0) * 0.5f * rightRect.lossyScale.x;
    }

    protected static void On_Color_Change(object o, Fardin.ColorTools.OnChangeColorHandler e)
    {
        ColorTools.SetSelectionsColor(e.form.RGB);
        colorPalette.color = e.form.RGB; 
    }

    public void On_Color_Certify()
    {
        colorTools.SetActive(false);
    }

    static void ShowRightClick(bool state)
    {
        GameObject.FindGameObjectWithTag("Canvas").transform.Find("EndRightClick").gameObject.SetActive(state);
        GameObject.FindGameObjectWithTag("Canvas").transform.Find("rightClick").gameObject.SetActive(state);
    }
}
