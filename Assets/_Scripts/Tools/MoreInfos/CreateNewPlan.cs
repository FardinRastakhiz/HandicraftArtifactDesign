///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CreateNewPlan> <PersianCategories>
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
using System.Text.RegularExpressions;

public class CreateNewPlan : MonoBehaviour {
    static GameObject createNewPlan;

    static InputField nameField;
    static InputField descriptionField;
    static Text CategoryPresent;
    
    static Button createButton;
    static Button runButton;
    static Button CategoriesButton;
    static GameObject CategoriesList;
    static string category;
    static string root;
    static string sewing;
    static string design;
    static string planName;
    static string description;
    static bool makeCopy;
    static Board originBoard;

    static GameObject filter; 
    public static void CopyShow(Cupboards cupboard, Board board)
    {
        makeCopy = true;
        originBoard = board;
        StartWindow(cupboard);
    }

    public static void Show(Cupboards cupboard)
    {
        makeCopy = false;
        StartWindow(cupboard);
    }

    static void StartWindow(Cupboards cupboard)
    {
        DefaultShortcuts.allowed = false;
        createNewPlan = Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().createNewPlan) as GameObject;
        createNewPlan.name = "createNewPlan";
        createNewPlan.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

        filter = createNewPlan.transform.parent.Find("ClickFilter").gameObject;
        filter.SetActive(true);
        createButton = createNewPlan.transform.Find("Create").GetComponent<Button>();
        runButton = createNewPlan.transform.Find("Create&Run").GetComponent<Button>();
        //CategoriesButton = createNewPlan.transform.Find("Category").GetComponent<Button>();
        //CategoriesList = createNewPlan.transform.Find("Categories").gameObject;
        //CategoriesList.gameObject.SetActive(false);
        descriptionField = createNewPlan.transform.Find("InputField").GetComponent<InputField>();
        nameField = createNewPlan.transform.Find("NameField").GetComponent<InputField>();
        CategoryPresent = createNewPlan.transform.Find("CategoryPresent").GetComponent<Text>();
        SetRectTransform();
        //category = "main";
        category = cupboard.category;
        root = cupboard.root;
        sewing = cupboard.sewing;
        design = cupboard.design;

        CategoryPresent.text = PersianCategories.GetPersian(category);
        if (design != sewing)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root) +
               " > " + PersianCategories.GetPersian(sewing) + " > " + PersianCategories.GetPersian(design);
        else if (sewing != root)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root) +
               " > " + PersianCategories.GetPersian(sewing);
        else if (root != category)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root);
        else
            CategoryPresent.text = PersianCategories.GetPersian(category);
        //SetCategoryName(category);
        //field.text = plan.description;
        createButton.interactable = false;
        runButton.interactable = false;
        //ColorPicker.useExternalDrawer = true;
    }

    static void SetRectTransform()
    {
        RectTransform rectTra = createNewPlan.GetComponent<RectTransform>();
        rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //rectTra.offsetMin = new Vector2(226, 143);
        //rectTra.offsetMax = new Vector2(-226, -133);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 440);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 270);
        rectTra.anchorMin = new Vector2(0.5f, 0.5f);
        rectTra.anchorMax = new Vector2(0.5f, 0.5f);
        rectTra.localPosition = Vector3.zero;
    }

    public void Close_New_Plan()
    {
        Cancel_Changes();
    }
    public void Cancel_Changes()
    {
        //ColorPicker.useExternalDrawer = false;
        closeWindow();
    }

    /*
        public void On_Select_New_Category()
        {
            category = EventSystem.current.currentSelectedGameObject.name;
            SetCategoryName(category);
            On_Cancel_Change_Category();
        }
        public void On_Cancel_Change_Category()
        {
            CategoriesButton.interactable = true;
            CategoriesList.SetActive(false);
        }

        public void On_Change_Category()
        {
            CategoriesButton.interactable = false;
            CategoriesList.SetActive(true);
            CategoryList.DrawButtons("createNewPlan");
        }
        static void SetCategoryName(string category)
        {
            CategoriesButton.transform.Find("Text").GetComponent<Text>().text = category;
        }
    
     */
    public void On_Name_Change()
    {
        planName = nameField.text;
        planName = planName.Replace(" ", "_");
        planName = Regex.Replace(planName, @"[^a-zA-Z0-9ا-ی0-9_][a-zA-Z0-9]*$", "");
        nameField.text = planName;
        if (planName.Length > 0)
        {
            createButton.interactable = true;
            runButton.interactable = true;
            for (int i = GenPlans.plans.Count - 1; i >= 0; i--)
            {
                if (GenPlans.plans[i].name == planName)
                {
                    createButton.interactable = false;
                    runButton.interactable = false;
                }
            }
        }
        else
        {
            createButton.interactable = false;
            runButton.interactable = false;
        }
    }
    public void On_Description_Change()
    {
        description = descriptionField.text;
    }

    public void On_Create_And_Run_Plan()
    {
        Plan targetPlan = GenPlans.AddPlan(planName, category, root, sewing, design, description, 0, Color.white);
        GenBoardPlan.Generate(targetPlan);
        StartCoroutine(MakeCopy(false, targetPlan));
    }
    public void On_Create_Plan()
    {
        Plan targetPlan = GenPlans.AddPlan(planName, category, root, sewing, design, description, 0, Color.white);
        StartCoroutine(MakeCopy(true, targetPlan));
    }

    IEnumerator MakeCopy(bool isTemporary, Plan targetPlan)
    {
        if (makeCopy)
        {
            if (Board.close)
            {
                originBoard.Close_Without_Save();
                yield return new WaitUntil(() =>
                {
                    return originBoard!=null;
                });
                Board.close = false;
            }
            GenBoardPlan.Generate(targetPlan);
            BoardPlan activePlan = GenBoardPlan.Generate(targetPlan);
            yield return new WaitUntil(() =>
            {
                return activePlan.board == null;
            });
            BoardPlans.ActiveIndex = BoardPlans.boardPlans.IndexOf(activePlan);
            if (isTemporary)
            {
                Board.close = true;
                Board.closePlan = true;
            }
            PasteTool.DropShapes(RightClick.mousePosition);
            SelectTools.ResetTotal();
            CopyTool.TakeShapes();
            yield return new WaitForEndOfFrame();
            GameObject.Find("ScreenShot").GetComponent<ScreenShot>().ForceTakeScreenShot(BoardPlans.boardPlans[BoardPlans.ActiveIndex].board);
        }
        closeWindow();
    }


    

    void closeWindow()
    {
        DefaultShortcuts.allowed = true;
        filter.SetActive(false);
        Destroy(createNewPlan);
    }
}

public class PersianCategories{
    public static System.Collections.Generic.Dictionary<string, string> englishToPersian;
    static void StartDictionary()
    {
        englishToPersian = new System.Collections.Generic.Dictionary<string, string>();
        englishToPersian.Add("PatternPlans", "نمونه ها");
        englishToPersian.Add("FigurePlans", "نقوش");
        englishToPersian.Add("BalochPlans", "سوزن دوزي بلوچستان");
        englishToPersian.Add("SistanPlans", "خامه دوزي سيستان");
        englishToPersian.Add("Porkar", "پرکار");
        englishToPersian.Add("Polivar", "پليوار");
        englishToPersian.Add("Mosem", "دوخت موسم");
        englishToPersian.Add("Fanooji", "دوخت فنوجي");
        englishToPersian.Add("Juke", "دوخت جوک");
        englishToPersian.Add("Charpar", "چهار پر");
        englishToPersian.Add("Dopar", "دو پر");
    }
    public static string GetPersian(string englishName)
    {
        if (englishToPersian==null)
        {
            StartDictionary();
        }
        return englishToPersian[englishName];
    }

}