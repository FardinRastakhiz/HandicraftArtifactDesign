///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ColorTools>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>


using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorTools {
    static int testthis = 0;
    public static void SetSelectionsColor(Color color){
        testthis = 0;
        if (SelectTools.lastShapes==null)
            return;

        foreach (var item in SelectTools.lastShapes)
	    {
            testthis++;
            item.color = color;
            item.GetComponent<Image>().color = color;
	    }
    }
}
