///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Menubar>
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
using UnityEngine.EventSystems;

public class Menubar : MonoBehaviour {
    bool isActive;
    bool transition;

    GameObject currenMenu;
    public GameObject menuFilter;


    public void On_Free_Space()
    {
        isActive = false;
        transition = true;
        On_Menu_over(null);
    }
    public void On_Menu_Click()
    {
        isActive = !isActive;
        transition = true;
        On_Menu_over(EventSystem.current.currentSelectedGameObject);
    }
    public void On_SubMenu_Click(GameObject menuButton)
    {
        isActive = !isActive;
        transition = true;
        On_Menu_over(menuButton);
    }

    public void On_Menu_over(GameObject menuButton)
    {
        if (transition)
        {
            if (currenMenu != null)
            {
                ChildActivation.DeActivate(transform, currenMenu.name);
            }
            transition = false;
        }

        menuFilter.SetActive(isActive);
        if (isActive)
        {
            if (currenMenu != null && currenMenu != menuButton)
            {
                ChildActivation.DeActivate(transform, currenMenu.name);
            }
            currenMenu = menuButton;
            ChildActivation.SetActive(transform, currenMenu.name);
        }

    }

}


