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
public class SourceBackground:SourceShape {
    public SourceBackground()
    {
        parent = GameObject.Find("SourceBackgrounds").transform.Find("ScrollContent").Find("Contents"); 
    }
}
