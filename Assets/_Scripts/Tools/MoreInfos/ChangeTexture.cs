///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ChangeTexture>
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
using UnityEngine.EventSystems;

public class ChangeTexture : MonoBehaviour
{
    public Sprite tabsRegular;
    public Sprite tabsSelected;
    public Sprite toolsRegular;
    public Sprite toolsSelected;
    static GameObject lastTools;
    static GameObject lastTabs;

    public void Alter_Change()
    {
        var currentObj = EventSystem.current.currentSelectedGameObject;
        switch (currentObj.tag)
        {
            case "Tool":
                ChangeTools(currentObj);
                break;
            case "Tab":
                ChangeTabs(currentObj);
                break;
            default:
                break;
        }
    }

    void ChangeTools(GameObject current)
    {
        if (lastTools != null)
            lastTools.GetComponent<Image>().sprite = toolsRegular;
        lastTools = current;
        lastTools.GetComponent<Image>().sprite = toolsSelected;
    }

    void ChangeTabs(GameObject current)
    {
        if (lastTabs!=null)
            lastTabs.GetComponent<Image>().sprite = tabsRegular;
        lastTabs = current;
        current.GetComponent<Image>().sprite = tabsSelected;
    }
}
