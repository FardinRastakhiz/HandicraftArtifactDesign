using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGrid : MonoBehaviour {

    [SerializeField]
    Window window;
    [SerializeField]
    Board board;

    RectTransform GridRect;
    RectTransform windowRect;
    Vector3 gridStartPos;
    Vector3 mouseStart;
    public static bool isDragging;
    bool isAreaSelect;
    float xMin = 0.0f;
    float yMin = 0.0f;
    float xMax = 0.0f;
    float yMax = 0.0f;
    public void Begin_Drag()
    {
        if (Input.GetMouseButton(1))
        {
            GridRect = GetComponent<RectTransform>();
            windowRect = window.GetComponent<RectTransform>();
            xMin = -GridRect.rect.width * GridRect.localScale.x / 2 + windowRect.rect.width / 2;
            yMin = -GridRect.rect.height * GridRect.localScale.x / 2 + windowRect.rect.height / 2;
            xMax = GridRect.rect.width * GridRect.localScale.y / 2 - windowRect.rect.width / 2;
            yMax = GridRect.rect.height * GridRect.localScale.y / 2 - windowRect.rect.height / 2;
            gridStartPos = GridRect.localPosition;
            mouseStart = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0))
        {
            isAreaSelect = true;
            ToolsUtility.On_Free_Area_Begin_Drag(this.transform);
        }
        GenBoardPlan.SetBoardPlanIndex(board.plan);
    }
    public void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            GridRect.localPosition = gridStartPos + (Input.mousePosition - mouseStart);
            GridRect.localPosition = new Vector3(GridRect.localPosition.x < xMin ? xMin : GridRect.localPosition.x,
                GridRect.localPosition.y < yMin ? yMin : GridRect.localPosition.y, GridRect.localPosition.z);
            GridRect.localPosition = new Vector3(GridRect.localPosition.x > xMax ? xMax : GridRect.localPosition.x,
                GridRect.localPosition.y > yMax ? yMax : GridRect.localPosition.y, GridRect.localPosition.z);
        }
        else if (Input.GetMouseButton(0))
        {
            ToolsUtility.On_Free_Area_Drag(board.plan);
        }
        GenBoardPlan.SetBoardPlanIndex(board.plan);
    }
    public void Drag_Exit()
    {
        ToolsUtility.On_Drag_Exit();
        if (window.IsFullScreen)
            return;
        GenBoardPlan.SetBoardPlanIndex(board.plan);
    }

    public void Click()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isAreaSelect)
                isAreaSelect = false;
            else
                ToolsUtility.On_Free_Area_Click();
        }
        GenBoardPlan.SetBoardPlanIndex(board.plan);
    }

}
