///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <DeleteWarning>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class DeleteWarning : MonoBehaviour {
    static InformationWindow targetBoard;
    static GameObject deleteWarning;
    static GameObject filter;
    public static void Show(InformationWindow board)
    {
        deleteWarning = Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().deleteWarning) as GameObject;
        deleteWarning.name = "DeleteWarning";
        deleteWarning.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        filter = deleteWarning.transform.parent.Find("ClickFilter").gameObject;
        filter.SetActive(true);
        targetBoard = board;
        SetRectTransform();
    }

    static void SetRectTransform()
    {
        RectTransform rectTra = deleteWarning.GetComponent<RectTransform>();
        rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //rectTra.offsetMin = new Vector2(226, 143);
        //rectTra.offsetMax = new Vector2(-226, -133);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 240);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 125);
        rectTra.anchorMin = new Vector2(0.5f, 0.5f);
        rectTra.anchorMax = new Vector2(0.5f, 0.5f);
        rectTra.localPosition = Vector3.zero;
    }

    public void On_Cancel()
    {
        CloseWindow();
    }

    public void On_Delete()
    {
        targetBoard.DeletePlan();
        CloseWindow();
    }

    void CloseWindow()
    {

        filter.SetActive(false);
        Destroy(deleteWarning);
    }
}
