///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <TabRightClick>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public class TabRightClick : MonoBehaviour {
    static GameObject selectedTab;
    static GameObject rightClick;
    static bool rightClickFound;
    public static bool jumpNext;
    static GameObject rightClickB;
    public static bool StartRightClick()
    {
        if (!rightClickFound)
        {
            rightClick = GameObject.FindGameObjectWithTag("Canvas").
                transform.Find("TabRightClick").gameObject;
            rightClickFound = true;
        }
        if (rightClick.activeSelf)
        {
            rightClick.SetActive(false);
            jumpNext = true;
            return false;
        }
        jumpNext = false;
        return true;
    }

    public static void Show(GameObject tab)
    {
        selectedTab = tab;
        SetRightClickObject();
    }

    public void AddTab()
    {
        Categorization.AddNewCategory();
    }

    public void DeleteTab()
    {
        for (int i = GenPlans.plans.Count-1; i >=0 ; i--)
		{
            if (GenPlans.plans[i].category == selectedTab.name)
            {
                GenPlans.plans[i].category = "main";
                GenPlans.SavePlan(GenPlans.plans[i]);
            }
		}
        if (Categorization.activeCategory == selectedTab.name)
        {
            Categorization.changeImage(selectedTab.transform.parent.Find("main").gameObject);
            Categorization.activeCategory = "main";
            Categorization.DrawCategory(GenPlans.plans, "main");
        }
        SaveBoardPlan.deleteRow("Categories", selectedTab.name);
        Categorization.categories.Remove(selectedTab.name);
        SaveBoardPlan.SaveCategories(Categorization.categories);
        Destroy(selectedTab);
    }

    static void SetRightClickObject()
    {
        rightClick.SetActive(true);
        rightClickB = rightClick.transform.Find("rightClick").gameObject;
        Rect rect = rightClickB.GetComponent<RectTransform>().rect;
        rightClickB.GetComponent<RectTransform>().position = Input.mousePosition - new Vector3(rect.width / 2, rect.height / 2, 0);

    }

}
