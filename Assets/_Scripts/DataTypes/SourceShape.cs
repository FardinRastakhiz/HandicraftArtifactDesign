///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SourceShape>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

//prefered widths and heights are 70
public class SourceShape {
    public int index;
    public string name;
    public Sprite image;
    public int width = 70;
    public int heights = 70;
    public Transform parent;
    public GameObject gameObject;

    public SourceShape()
    {
        parent = GameObject.Find("SourceShapes").transform.Find("ScrollContent").Find("Contents");
    }
}
