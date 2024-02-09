///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Cupboards>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cupboards : MonoBehaviour {
    static GameObject lastCupboard;
    static LayoutElement lastCupboardLayout;
    static GameObject currentCupboard;
    static LayoutElement currentCupboardLayout;
    float timer;
    static float parentHeight;
    bool resetLast;
    static bool isLoading;

    public string category;
    public string root;
    public string sewing;
    public string design;
    void Awake()
    {
        timer = 3.0f;
        isLoading = false;
    }
    void FixedUpdate()
    {
        if (timer<0.5f)
        {
            currentCupboardLayout.preferredHeight = Mathf.Lerp(currentCupboardLayout.preferredHeight, parentHeight, timer*2.0f);

            if (resetLast && lastCupboardLayout != currentCupboardLayout)
                lastCupboardLayout.preferredHeight = Mathf.Lerp(lastCupboardLayout.preferredHeight, 25, timer * 2.0f);
            timer += Time.fixedDeltaTime;
        }
        else if (timer<1.0f)
        {
            lastCupboard = currentCupboard;
            lastCupboardLayout = currentCupboardLayout;
            timer += 3.0f;
            isLoading = false;
        }
    }

    
    public void expand()
    {
        if (this.gameObject == lastCupboard)
        {
            timer = 0.0f;
            parentHeight = parentHeight == 25 ? (currentCupboard.transform.parent.GetComponent<RectTransform>().rect.height -
                (currentCupboard.transform.parent.childCount - 1) * 25.0f) : 25;
            isLoading = true;
            return;
        }
        if (isLoading)
            return;

        resetLast = true;
        if (lastCupboard == null)
            resetLast = false;
        currentCupboard = this.gameObject;
        currentCupboardLayout = currentCupboard.GetComponent<LayoutElement>();
        timer = 0.0f;
        parentHeight = currentCupboard.transform.parent.GetComponent<RectTransform>().rect.height -
            (currentCupboard.transform.parent.childCount-1)*25.0f;
        isLoading = true;
    }


}
