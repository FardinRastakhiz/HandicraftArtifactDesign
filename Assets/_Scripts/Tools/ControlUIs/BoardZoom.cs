using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardZoom : MonoBehaviour {

    [SerializeField]
    Board board;
    [SerializeField]
    RectTransform grid;

    static Image image;
    Vector3 mouseStart;
    float startSize;

    void Start()
    {
        image = GetComponent<Image>();
    }
    public static bool Interactible
    {
        get { return image.raycastTarget; }
        set { image.raycastTarget = value; }
    }

    public void On_Mouse_Scroll()
    {
        startSize = grid.localScale.x;
        ZoomBoard.Zoom(board, grid, startSize, Input.mouseScrollDelta.y / 20.0f);
    }

    public void On_Begin_Zoom_Drag()
    {
        if (DefaultShortcuts.shiftActive)
        {
            mouseStart = Input.mousePosition;
            startSize = grid.localScale.x;
        }
    }

    public void On_Zoom_Drag()
    {
        if (DefaultShortcuts.shiftActive)
        {

            Vector3 mouseMovement = Input.mousePosition - mouseStart;
            float zoomSize = mouseMovement.x + mouseMovement.y;
            ZoomBoard.Zoom(board, grid, startSize, zoomSize / 1000.0f);
        }
    }


}
