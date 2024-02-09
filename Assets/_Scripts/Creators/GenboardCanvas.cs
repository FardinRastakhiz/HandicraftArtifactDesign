///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenSourceShaps> <SSComps> <SourceShapeSubInfo>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GenboardCanvas
{
    static List<SourceShape> canvasButtons;
    public static void Generate()
    {
        canvasButtons = new List<SourceShape>();
        SSComps.GetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("BoardCanvasTextures").gameObject);
        for (int i = 0; i < ShapeCenter.boardCanvas.Length; i++)
        {
            SourceShape canvasButton = new SourceShape();
            canvasButton.index = i;
            canvasButton.name = ShapeCenter.boardCanvas[i].name;
            canvasButton.image = ShapeCenter.boardCanvas[i];
            canvasButton.gameObject = SSComps.createShapeObject(canvasButton);
            canvasButton.gameObject.GetComponent<Button>().onClick.
                AddListener(() => ChangeCanvas.ChangeBoardCanvas(canvasButton.gameObject.transform.GetSiblingIndex()));
            SubInfo.AddSubInfo(canvasButton.gameObject.transform);
            canvasButtons.Add(canvasButton);
        }
    }
}
