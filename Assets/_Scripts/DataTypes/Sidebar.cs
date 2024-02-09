///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Sidebar>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class Sidebar: MonoBehaviour
{
    public int id;
    public string planName;
    public Shape shape;
    public Vector2 size;
    public Transform2D transform2D;
    public string sourceImage;
    public int indx;
    public ShapeType shapeType;
    public void TakeParameters(Shape shape)
    {
        if (shape.GetType()==typeof(Part))
            TakeParameters((Part)shape);
        else if (shape.GetType()==typeof(Primitive))
		    TakeParameters((Primitive)shape);
    }
    public void TakeParameters(Part part){
        size = part.size;
        shape = part;
        transform2D = new Transform2D();
        transform2D.position = part.transform.position;
        sourceImage = part.sourceImage;
        indx = part.index;
        shapeType = ShapeType.PART;
    }
    public void TakeParameters(Primitive prim)
    {
        size = prim.size;
        shape = prim;
        transform2D = new Transform2D();
        transform2D.position = prim.transform.position;
        indx = prim.sourceShape;
        shapeType = ShapeType.PRIMITIVE;
    }
}