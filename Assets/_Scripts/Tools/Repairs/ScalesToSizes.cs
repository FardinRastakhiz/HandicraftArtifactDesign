using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalesToSizes : MonoBehaviour
{
    public static void Repair()
    {
        foreach (var item in GenPlans.plans)
        {
            BoardPlan plan = LoadBoardPlan.Load(item.name);
            plan.plan = item;
            CreateDesignBoard(plan);
            SetPrimitiveComponents.SetBoardPlanPrimitives(plan);
            SetBackgroundComponents.SetBoardPlanBackgrounds(plan);
            SetPartComponents.SetactivePlanParts(plan);


            plan.Canvas = plan.plan.canvas;
            plan.CanvasColor = plan.plan.canvasColor;
            plan.category = plan.plan.category;
            plan.description = plan.plan.description;
            plan.design = plan.plan.design;

            for (int i = plan.parts.Count - 1; i >= 0; i--)
            {
                RectTransform rectTra = plan.parts[i].gameObject.GetComponent<RectTransform>();
                float size = plan.parts[i].transform2D.size;
                rectTra.sizeDelta = plan.parts[i].size * size;
                plan.parts[i].transform2D.scale = rectTra.sizeDelta;
                rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                rectTra.anchoredPosition = plan.parts[i].transform2D.position;
                rectTra.localEulerAngles = new Vector3(rectTra.localEulerAngles.x, rectTra.localEulerAngles.y, plan.parts[i].transform2D.rotation);
                plan.parts[i].transform2D.size = 1.0f;
            }
            for (int i = plan.primitives.Count - 1; i >= 0; i--)
            {
                RectTransform rectTra = plan.primitives[i].gameObject.GetComponent<RectTransform>();
                float size = plan.primitives[i].transform2D.size;
                rectTra.sizeDelta = plan.primitives[i].size * size;//new Vector2(image.sprite.textureRect.width, image.sprite.textureRect.height)
                plan.primitives[i].transform2D.scale = rectTra.sizeDelta;
                rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                rectTra.anchoredPosition = plan.primitives[i].transform2D.position;
                rectTra.localEulerAngles = new Vector3(rectTra.localEulerAngles.x, rectTra.localEulerAngles.y, plan.primitives[i].transform2D.rotation);
                plan.primitives[i].transform2D.size = 1.0f;
            }


            SaveBoardPlan.Save(plan);

            for (int i = plan.parts.Count - 1; i >= 0; i--)
                Destroy(plan.parts[i].gameObject);

            for (int i = plan.primitives.Count - 1; i >= 0; i--)
                Destroy(plan.primitives[i].gameObject);

            Destroy(plan.board.gameObject);
            Destroy(plan.gameObject);
        }
    }

    static void CreateDesignBoard(BoardPlan plan)
    {
        GameObject newBoard = (GameObject)MonoBehaviour.Instantiate(
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().designBoards);
        newBoard.name = "Design Board";
        newBoard.transform.SetParent(GameObject.FindGameObjectWithTag("PaletteBoard").transform);
        newBoard.transform.parent.Find("ShowButtons").SetAsLastSibling();
        plan.board = newBoard.GetComponent<Board>();
        plan.board.UIObject = newBoard;
        //plan.board.Index = 0;
        plan.board.SetBoardRect();
        plan.board.plan = plan;
        plan.transform.SetParent(newBoard.transform);
        UnityEngine.UI.Image boardCanvas = newBoard.transform.Find("Mask").Find("Grid").Find("DrawCanvas").GetComponent<UnityEngine.UI.Image>();
        boardCanvas.sprite = ShapeCenter.boardCanvas[plan.Canvas];
        boardCanvas.color = plan.CanvasColor;
    }
}

