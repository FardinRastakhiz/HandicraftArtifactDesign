///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <RectInstance>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class RectInstance {
    Vector2 offsetMin;
    Vector2 offsetMax;
    Vector2 anchorMax;
    Vector2 anchorMin;
    Vector3 localPosition;
    Quaternion localRotation;
    Vector3 localScale;
    Vector3 anchoredPosition;

    public RectInstance(RectTransform origin)
    {
        GetDataFrom(origin);
    }

    public void GetDataFrom(RectTransform origin)
    {
        offsetMin = origin.offsetMin;
        offsetMax = origin.offsetMax;
        anchorMax = origin.anchorMax;
        anchorMin = origin.anchorMin;
        localPosition = origin.localPosition;
        localRotation = origin.localRotation;
        localScale = origin.localScale;
        anchoredPosition = origin.anchoredPosition;
    }
    public void GiveDataTo(RectTransform origin)
    {

        origin.offsetMin = offsetMin;
        origin.offsetMax = offsetMax;
        origin.anchorMax = anchorMax;
        origin.anchorMin = anchorMin;
        origin.localPosition = localPosition;
        origin.localRotation = localRotation;
        origin.localScale= localScale;
        origin.anchoredPosition = anchoredPosition;
    }
}
