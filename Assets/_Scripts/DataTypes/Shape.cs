///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Shape>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public enum ShapeType { PART, PRIMITIVE, BACKGROUND};

public class Shape : MonoBehaviour {
    public int id;
    public Vector2 size;
    public Transform2D transform2D;
    public Color color;
    public bool selected;
    public int order;
}
