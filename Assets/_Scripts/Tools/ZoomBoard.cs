///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ZoomBoard>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class ZoomBoard {
    public static void Zoom(Transform shape)
    {
        RectTransform grid = shape.parent.GetComponent<RectTransform>();
        Zoom(grid, grid.localScale.x, Input.mouseScrollDelta.y / 20.0f);
    }


    public static void Zoom(Board board, RectTransform grid, float startSize, float addSize)
    {
        board.Set_BoardPlan_Index();
        Zoom(grid, startSize, addSize);
    }


    static void Zoom(RectTransform grid, float startSize, float addSize)
    {
        RectTransform rectTra = grid.parent.GetComponent<RectTransform>();
        Vector2 localPos1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid, Input.mousePosition,
            grid.GetComponentInParent<Canvas>().GetComponent<Camera>(), out localPos1);

        grid.localScale = new Vector3(startSize + addSize, startSize + addSize, startSize + addSize);
        grid.localScale = grid.localScale.x < 0.1f ? new Vector3(0.1f, 0.1f, 0.1f) : grid.localScale;
        grid.localScale = grid.localScale.x > 4.0f ? new Vector3(4.0f, 4.0f, 4.0f) : grid.localScale;

        Vector2 localPos2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(grid, Input.mousePosition,
            grid.GetComponentInParent<Canvas>().GetComponent<Camera>(), out localPos2);


        grid.localPosition += new Vector3(localPos2.x - localPos1.x, localPos2.y - localPos1.y, 0.0f) * grid.localScale.x;

        fitGridPosition(grid, rectTra);
    }


    static void fitGridPosition(RectTransform Grid, RectTransform rectTra)
    {
        float xMin = -Grid.rect.width * Grid.localScale.x / 2 + rectTra.rect.width / 2;
        float yMin = -Grid.rect.height * Grid.localScale.x / 2 + rectTra.rect.height / 2;
        float xMax = Grid.rect.width * Grid.localScale.y / 2 - rectTra.rect.width / 2;
        float yMax = Grid.rect.height * Grid.localScale.y / 2 - rectTra.rect.height / 2;
        Grid.localPosition = new Vector3(Grid.localPosition.x < xMin ? xMin : Grid.localPosition.x,
            Grid.localPosition.y < yMin ? yMin : Grid.localPosition.y, Grid.localPosition.z);
        Grid.localPosition = new Vector3(Grid.localPosition.x > xMax ? xMax : Grid.localPosition.x,
            Grid.localPosition.y > yMax ? yMax : Grid.localPosition.y, Grid.localPosition.z);
    }

}
