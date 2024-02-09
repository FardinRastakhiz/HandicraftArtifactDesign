///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Categorization> <TabComponents> <TabClick>
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
using System.Reflection.Emit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Categorization : MonoBehaviour {


    //Dictionary<>
    static Button addButton;
    //static bool needSave;
    public static HashSet<string> categories;
    static GameObject lastTab = null;
    public static string activeCategory;
	
	static SpriteState currentState;
    static SpriteState lastState;
    static GameObject rightClick;

    public static void OnStartSoftware(List<Plan> plans)
    {
        addButton = GameObject.FindGameObjectWithTag("PlanCategories").transform.Find("Tabs").Find("Content").Find("Add").GetComponent<Button>();
        addButton.onClick.AddListener(AddNewCategory);
        categories = LoadBoardPlan.LoadCategories();
        foreach (var item in categories)
        {
            AddTab(item);
        }
        activeCategory = "main";
        DrawCategory(plans, activeCategory);
    }

    public static void SetActiveTitle()
    {
        GameObject categoryButton = EventSystem.current.currentSelectedGameObject;
        changeImage(categoryButton);
        activeCategory = categoryButton.name;
        DrawCategory(GenPlans.plans, activeCategory);
    }

    public static void SetActiveCategory()
    {
        GameObject categoryButton = EventSystem.current.currentSelectedGameObject;
        changeImage(categoryButton);
        activeCategory = categoryButton.name;
        DrawCategory(GenPlans.plans, activeCategory);
    }

    public static void On_Tab_RightClick(GameObject tabObject)
    {
        if (Input.GetMouseButtonDown(1) || TabRightClick.jumpNext)
        {
            if (TabRightClick.StartRightClick())
                TabRightClick.Show(tabObject);
        }
    }

    



    public static void changeImage(GameObject categoryButton)
    {
		currentState = categoryButton.GetComponent<Button> ().spriteState;
		categoryButton.GetComponent<Image>().sprite = currentState.pressedSprite;
		if (lastTab != null) {
			lastState = lastTab.GetComponent<Button> ().spriteState;
			lastTab.GetComponent<Image>().sprite = currentState.disabledSprite;
			lastState.highlightedSprite = currentState.highlightedSprite;
			lastTab.GetComponent<Button> ().spriteState = lastState;
		}
		currentState.highlightedSprite = currentState.pressedSprite;
		categoryButton.GetComponent<Button> ().spriteState = currentState;
        lastTab = categoryButton;
    }
    

    public static void DrawCategory(List<Plan> plans, string name)
    {
        /*for (int i = 0; i < plans.Count; i++)
        {
            if (!categories.Contains(plans[i].category))
            {
                categories.Add(plans[i].category);
                needSave = true;
            }
            if (plans[i].category == name)
            {
                plans[i].gameObject.SetActive(true);
            }
            else
            {
                plans[i].gameObject.SetActive(false);
            }
        }


        if (needSave)
        {
            SaveBoardPlan.SaveCategories(categories);
        }
         */
    }
    public static void AddNewCategory()
    {
        GameObject newObj = Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().addNewTab) as GameObject;
        newObj.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        newObj.transform.localPosition = Vector3.zero;

    }

    public static void AddTab(string name)
    {
        GameObject newObj = Instantiate(addButton.gameObject) as GameObject;
        newObj.name = name;
        newObj.transform.SetParent(addButton.transform.parent);
        //Destroy(newObj.GetComponent<Button>());
        TabComponents.SetTabs(newObj);
        categories.Add(name);
        SaveBoardPlan.SaveCategories(categories);
        addButton.transform.SetAsLastSibling();

        changeImage(newObj);
        activeCategory = name;
        DrawCategory(GenPlans.plans, name);
    }
}

class TabComponents
{
    public static void SetTabs(GameObject Tab)
    {
        SetText(Tab.transform.Find("Text").GetComponent<Text>(), Tab.name);
        SetRectTransform(Tab.GetComponent<RectTransform>());
        SetLayout(Tab.GetComponent<LayoutElement>());
        SetButton(Tab.GetComponent<Button>());
        SetEventTrigger(Tab);
    }

    static void SetText(Text text, string name)
    {
        text.fontSize = 14;
        text.text = name;
    }

    static void SetRectTransform(RectTransform rectTra)
    {
        rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    
    static void SetLayout(LayoutElement element)
    {
        element.preferredWidth = 65;
    }

    static void SetButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { Categorization.SetActiveCategory(); });
    }
    static void SetEventTrigger(GameObject trigger)
    {
        //TabClick click = trigger.AddComponent<TabClick>();

        //trigger.AddComponent<EventTrigger>();
        //trigger.GetComponent<EventTrigger>().delegates = new List<EventTrigger.Entry>();
        //EventTrigger.Entry entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerClick;
        //entry.callback.AddListener(delegate { Categorization.On_Tab_RightClick(trigger); });
        //trigger.GetComponent<EventTrigger>().delegates.Add(entry);
    }
}

public class TabClick : MonoBehaviour, IPointerDownHandler//, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Categorization.On_Tab_RightClick(this.gameObject);
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Categorization.On_Tab_RightClick(this.gameObject);
    //    // Do action
    //}
}