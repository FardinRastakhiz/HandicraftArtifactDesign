///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <MirrorTools>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class MirrorTools : MonoBehaviour {

    public static void HorizontalMirror()
    {
        Vector3 center = CenterOfShapes();
        Vector3 scale = Vector3.one;
        Vector3 position = Vector3.zero;
        foreach (var item in SelectTools.lastShapes)
        {

            scale = item.transform.localScale;
            scale.x *= -1;
            position = item.transform.position;
            position.x = 2 * center.x - position.x;
            item.transform.localScale = scale;
            item.transform.position = position;
            item.transform.localEulerAngles *= -1;
        }
    }

    static Vector3 CenterOfShapes()
    {
        float shapeCounts = SelectTools.lastShapes.Count;
        float minX = 10000, minY = 10000, maxX = -10000, maxY = -10000;
        Vector3 position = Vector3.zero;
        foreach (var item in SelectTools.lastShapes)
        {
            position = item.transform.position;
            minX = (position.x < minX) ? position.x : minX;
            minY = (position.y < minY) ? position.y : minY;
            maxX = (position.x > maxX) ? position.x : maxX;
            maxY = (position.y > maxY) ? position.y : maxY;
        }
        return new Vector3((minX + maxX) / 2.0f, (minY + maxY) / 2.0f, 0.0f);
    }
}
