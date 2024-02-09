///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ShapeInstance>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;

public class ShapeInstance
{
    public int id;
    public Vector2 size;
    public Transform2D transform2D;
    public Color color;
    public UnityEngine.UI.Image.Type imageType;
    public int order;

    public string sourcePlan;
    public string sourceImage;
    public int indx;
    public ShapeType shapeType;
    public RectTransform rectTransform;
}