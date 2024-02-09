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

public class GenSourceBackgrounds {
    static List<SourceBackground> backgrounds;
    public static void Generate()
    {
        backgrounds = new List<SourceBackground>();
        SourceBackground background = new SourceBackground();
        SSComps.GetParent(GameObject.Find("SourceBackgrounds"));
        for (int i = 0; i < ShapeCenter.backgrounds.Length; i++)
        {
            background.index = i;
            background.name = ShapeCenter.backgrounds[i].image.name;
            background.image = ShapeCenter.backgrounds[i].image;
            background.gameObject = SSComps.createShapeObject(background);
            background.gameObject.GetComponent<Button>().onClick.AddListener(()=>CopyShape.On_Source_Background_Click());
            SubInfo.AddSubInfo(background.gameObject.transform);
            backgrounds.Add(background);
        }
    }
}
