///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <BoardPlan>
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
using System.Diagnostics;

public class BoardPlan:MonoBehaviour {
    public int id;
    public string sourceImage;
    public Board board;
    public string description;
    public int Canvas;
    public Color CanvasColor;
    public Plan plan;
    public bool isOriginal;
    public int order;

    public List<Background> backgrounds;
    public List<Primitive> primitives;
    public List<Part> parts;
    public List<int> shapeIDs;
    public List<Shape> sideBars;
    public Dictionary<int,Shape> orders;
    public List<int> indexInOrder;

    /// <summary>
    /// pattern and figure
    /// </summary>
    public string category;

    /// <summary>
    /// sistan and baloch
    /// </summary>
    public string root;

    /// <summary>
    /// porkar and polyvar
    /// </summary>
    public string sewing;

    /// <summary>
    /// mosem, juke and fanooji
    /// </summary>
    public string design;

    public void Hide(bool state)
    {
        board.gameObject.SetActive(!state);
        //for (int i = 0; i < primitives.Count; i++)
        //    primitives[i].gameObject.SetActive(!state);
        //for (int i = 0; i < parts.Count; i++)
        //    parts[i].gameObject.SetActive(!state);
    }

    ~BoardPlan()
    {
        //StackTrace stackTrace = new StackTrace();
        //SaveBoardPlan.Save(this);
        backgrounds.Clear();
        backgrounds = null;
        parts.Clear();
        parts = null;
        primitives.Clear();
        primitives = null;
        sideBars.Clear();
        sideBars = null;
        orders.Clear();
        orders = null;
        indexInOrder.Clear();
        indexInOrder = null;
    }
}
