///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Plan>
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

public class Plan:MonoBehaviour {
    public int id;
    public string sourceImage;
    public string description;
    public int canvas;
    public Color canvasColor;
    public bool isOriginal;

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
}
