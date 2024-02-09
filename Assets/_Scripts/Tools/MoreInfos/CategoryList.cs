///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CategoryList>
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
using UnityEngine.UI;

public class CategoryList : MonoBehaviour {
    static HashSet<string> TabNames;
    static List<GameObject> ButtonObjects;
    static Transform informationWindow;

    static GameObject sampleCategory;

    public static void DrawButtons(string menuName)
    {
        informationWindow = GameObject.FindGameObjectWithTag("Canvas").transform.Find(menuName);
        sampleCategory = informationWindow.Find("Categories").Find("Buttons").Find("Contents").Find("main").gameObject;
        sampleCategory.SetActive(true);
        ResetObjects();
        TabNames = new HashSet<string>();
        TabNames = Categorization.categories;
        ButtonObjects = new List<GameObject>();
        foreach (var item in TabNames)
        {
            var newObj = Instantiate(sampleCategory) as GameObject;
            newObj.name = item;
            newObj.transform.SetParent(sampleCategory.transform.parent);
            newObj.transform.Find("Text").GetComponent<Text>().text = item;
            newObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //newObj.GetComponent<Button>().onClick.
            ButtonObjects.Add(newObj);
        }
        sampleCategory.SetActive(false);
    }

    static void ResetObjects()
    {
        if (ButtonObjects == null)
            return;
        for (int i = ButtonObjects.Count-1; i >=0 ; i--)
        {
            var removeObj = ButtonObjects[i];
            Destroy(removeObj);
            ButtonObjects.RemoveAt(i);
        }
    }

    static void AddComponents(GameObject obj)
    {
        obj.AddComponent<LayoutElement>();
        obj.AddComponent<Button>();
        //obj.AddComponent<Text>();
        obj.AddComponent<Image>();
        obj.AddComponent<RectTransform>();
    }
    static void SetComponents(GameObject obj)
    {
        SetImage(obj.GetComponent<Image>());
        SetButton(obj.GetComponent<Button>());
        //SetText(obj.GetComponent<Text>(), obj.name);
        SetRectTransform(obj.GetComponent<RectTransform>());
        SetLayoutElement(obj.GetComponent<LayoutElement>());
    }

    static void SetImage(Image image)
    {

    }
    static void SetButton(Button button)
    {
        /*switch (menuName)
        {
            case "createNewPlan":
                CreateNewPlan createN = GameObject.FindGameObjectWithTag("Canvas").transform.Find("createNewPlan").GetComponent<CreateNewPlan>();
                //button.onClick.AddListener(createN.On_Select_New_Category);
                break;
            case "InformationWindow":
                InformationWindow desc = GameObject.FindGameObjectWithTag("Canvas").transform.Find("InformationWindow").GetComponent<InformationWindow>();
                //button.onClick.AddListener(desc.On_Select_New_Category);
                break;
        }*/
    }
    static void SetText(Text text, string name)
    {
        text.text = name;
        text.alignment = TextAnchor.MiddleLeft;
        text.fontSize = 16;
        text.color = Color.black;
    }
    static void SetRectTransform(RectTransform rectTra)
    {
        rectTra.anchorMin = new Vector2(0.0f, 1.0f);
        rectTra.anchorMax = new Vector2(0.0f, 1.0f);
        rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    static void SetLayoutElement(LayoutElement element)
    {
        element.preferredHeight = 28;
    }
}
