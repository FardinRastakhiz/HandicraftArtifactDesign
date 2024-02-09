///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenPlans> <PlanComponents> <planSubInfo>
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
using System.IO;
using System.Linq;

public class GenPlans
{
    public static List<Plan> plans;
    static int lastID;
    public static void Start()
    {
        plans = new List<Plan>();
        TakePlans();
        CreateUIs();
        //Categorization.OnStartSoftware(plans);
    }

    public static Plan GetPlan(string planName){
        return plans.First<Plan>(x => x.name == planName);
    }


    public static Plan AddPlan(string name, string category, string root, string sewing, string design, string description,
        int canvas, Color canvasColor)
    {
        GameObject newObj = new GameObject();
        newObj.AddComponent<Plan>();
        Plan newPlan = newObj.GetComponent<Plan>();
        newPlan.id = ++lastID;
        newObj.name = name;
        newPlan.name = name;
        newPlan.sourceImage = "NewPlan";
        newPlan.category = category;
        newPlan.root = root;
        newPlan.sewing = sewing;
        newPlan.design = design;
        newPlan.description = description;
        newPlan.canvas = canvas;
        newPlan.canvasColor = canvasColor;
        PlanComponents.createPlanObject(newPlan);
        SaveNewPlan(newPlan);
        plans.Add(newPlan);
        return newPlan;
    }

    static void TakePlans()
    {
        string query = "SELECT * FROM Plans";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        while (reader.Read())
        {
            GameObject newShape = new GameObject((string)reader["name"]);
            newShape.AddComponent<Plan>();
            Plan plan = newShape.GetComponent<Plan>();
            plan.id = (int)reader["id"];
            plan.name = newShape.name;
            plan.sourceImage = (string)reader["sourceImage"];
            plan.category = (string)reader["category"];
            plan.root = (string)reader["root"];
            plan.sewing = (string)reader["sewing"];
            plan.design = (string)reader["design"];
            plan.description = (string)reader["description"];
            plan.canvas = (int)reader["Canvas"];
            plan.canvasColor.r = (float)System.Convert.ToDouble(reader["CanvasColorR"]);
            plan.canvasColor.g = (float)System.Convert.ToDouble(reader["CanvasColorG"]);
            plan.canvasColor.b = (float)System.Convert.ToDouble(reader["CanvasColorB"]);
            plan.canvasColor.a = 1.0f;
            plan.isOriginal = (int)reader["Original"]==1?true:false;
            plans.Add(plan);
        }
        reader.Close();
        SQLiteExecute.ExitQuery();
    }

    static void CreateUIs()
    {
        for (int i = 0; i < plans.Count; i++)
        {
            lastID = plans[i].id;
            PlanComponents.createPlanObject(plans[i]);
        }
    }

    static void SaveNewPlan(Plan plan)
    {
        string query = "INSERT INTO Plans(id, name, sourceImage, category, root, sewing, design, description, Canvas, CanvasColorR," +
            " CanvasColorG, CanvasColorB, Original) VALUES('" + plan.id + "', '" + plan.name + "', '" + plan.sourceImage + "', '" + plan.category +
            "', '" + plan.root + "', '" + plan.sewing + "', '" + plan.design + "', '" + plan.description + "', '" + plan.canvas +
            "', '" + plan.canvasColor.r + "', '" + plan.canvasColor.g + "', '" + plan.canvasColor.b + "', '" + (plan.isOriginal ? 1 : 0) + "')";
        SQLiteExecute.NonReaderExecute(query);
        query = "CREATE TABLE IF NOT EXISTS '" + plan.name + "_parts'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourceImage' TEXT, 'width' DOUBLE, 'height' DOUBLE, 'indx' INTEGER, 'red' DOUBLE, 'green' DOUBLE," +
            " 'blue' DOUBLE, 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE" +
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        SQLiteExecute.ChangeNonReadQuery(query);
        query = "CREATE TABLE IF NOT EXISTS '" + plan.name + "_primitives'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourcePrimitive' INTEGER, 'width' DOUBLE, 'height' DOUBLE, 'red' DOUBLE, 'green' DOUBLE, 'blue' DOUBLE" +
            ", 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE" +
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        SQLiteExecute.ChangeNonReadQuery(query);
        query = "CREATE TABLE IF NOT EXISTS '" + plan.name + "_backgrounds'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourceShape' INTEGER, 'imageType' INTEGER, 'width' DOUBLE, 'height' DOUBLE, 'red' DOUBLE, 'green' DOUBLE, 'blue' DOUBLE" +
            ", 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE" +
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        SQLiteExecute.ChangeNonReadQuery(query);
        SQLiteExecute.ExitQuery();
    }

    public static void SavePlan(Plan plan)
    {
        string query = "INSERT OR REPLACE INTO Plans(id, name, sourceImage, category, root, sewing, design"+
        ", description, Canvas, CanvasColorR, CanvasColorG, CanvasColorB, Original) VALUES('" +
            plan.id + "', '" + plan.name + "', '" + plan.sourceImage + "', '" + plan.category + "', '" + plan.root +
            "', '" + plan.sewing + "', '" + plan.design + "', '" + plan.description + "', '" + plan.canvas +
            "', '" + plan.canvasColor.r + "', '" + plan.canvasColor.g + "', '" + plan.canvasColor.b + "', '" + (plan.isOriginal ? 1 : 0) + "')";
        SQLiteExecute.NonReaderExecute(query);
        SQLiteExecute.ExitQuery();
    }

}


class PlanComponents
{
    static Transform parent;
    static string sourceImage;
    static GameObject planImageComp;
    static Transform parentPlan;

    static void setParent(Plan plan)
    {
        if (parentPlan == null)
        {
            parentPlan = GameObject.Find("PlanCategories").transform;
        }

        if (plan.root == "BalochPlans")
        {
            if (plan.sewing == "Porkar")
            {
                parent = parentPlan.Find("Categories").Find(plan.category).Find(plan.root).Find(plan.sewing).Find("Contents").
                    Find(plan.design).Find("Plans").Find("Contents");
            }
            else
            {
                parent = parentPlan.Find("Categories").Find(plan.category).Find(plan.root).Find(plan.sewing).Find("Plans").Find("Contents");
            }
        }
        else if (plan.root == "SistanPlans")
        {
            parent = parentPlan.Find("Categories").Find(plan.category).Find(plan.root).Find("Plans").Find("Contents");
        }
    }

    public static void createPlanObject(Plan plan)
    {
        setParent(plan);
        sourceImage = plan.sourceImage;
        plan.transform.SetParent(parent);
        CreatePlanImageObject(plan);;
        AddComponents(plan.gameObject);
        SetComponents(plan.gameObject);
        planSubInfo.AddPlanSubInfo(plan.transform);
    }

    static void CreatePlanImageObject(Plan plan)
    {
        
        planImageComp = new GameObject("PlanImage");
        planImageComp.transform.SetParent(plan.transform);
        UnityEngine.UI.Image img = planImageComp.AddComponent<UnityEngine.UI.Image>();
        
        if (File.Exists(Application.dataPath + "/Resources/" + sourceImage+".png"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().LoadPlanImage(plan);
        }
        else
        {
            try
            {
                Texture2D targetImg = Resources.Load<Texture2D>(sourceImage);
                img.sprite = Sprite.Create(targetImg, new Rect(0, 0, targetImg.width, targetImg.height),
                    new Vector2(0.5f, 0.5f), 1.0f);
            }
            catch (System.Exception)
            {
                Debug.Log(sourceImage);
            }
        }
        RectTransform rect = planImageComp.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        //rect.SetAsFirstSibling();
    }



    static void AddComponents(GameObject shape)
    {
        shape.AddComponent<RectTransform>();
        shape.AddComponent<CanvasRenderer>();
        shape.AddComponent<UnityEngine.UI.Image>();
        shape.AddComponent<UnityEngine.UI.Button>();
        shape.AddComponent<UnityEngine.UI.LayoutElement>();
        shape.AddComponent<UnityEngine.UI.Outline>();
    }

    static void SetComponents(GameObject shape)
    {
        SetRectTransform(shape.GetComponent<RectTransform>());
        SetImage(shape.GetComponent<UnityEngine.UI.Image>());
        SetButton(shape.GetComponent<UnityEngine.UI.Button>());
        SetLayoutElement(shape.GetComponent<UnityEngine.UI.LayoutElement>());
        SetLayoutOutline(shape.GetComponent<UnityEngine.UI.Outline>());
    }



    static void SetRectTransform(RectTransform rectTransform)
    {
        rectTransform.localScale = new Vector3(1, 1, 1);
    }
    static void SetImage(UnityEngine.UI.Image image)
    {
        image.sprite = ShapeCenter.planBackground;
    }


    static void SetButton(UnityEngine.UI.Button button)
    {
        new GenBoardPlan(); 
        button.onClick.AddListener(delegate { CreateWindow.CreateNewWindow("InformationWindow"); });
    }
    static void SetLayoutElement(UnityEngine.UI.LayoutElement element)
    {
        element.preferredHeight = 165;
        element.preferredWidth = 165;
    }

    static void SetLayoutOutline(UnityEngine.UI.Outline outline)
    {
        outline.effectDistance = new Vector2(2, -2);
        outline.effectColor = new Color(35 / 255.0f, 35 / 255.0f, 35 / 255.0f);
    }
}

class planSubInfo:MonoBehaviour
{
    public static void AddPlanSubInfo(Transform planTra)
    {
        GameObject subInfo = Instantiate(ShapeCenter.planSub) as GameObject;
        subInfo.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = planTra.name;
        subInfo.transform.SetParent(planTra);
        SetRectTransform(subInfo.GetComponent<RectTransform>());
        SetImage(subInfo.GetComponent<UnityEngine.UI.Image>());
    }

    static void SetRectTransform(RectTransform rectTra){
        rectTra.localScale = Vector3.one;
        rectTra.offsetMin = new Vector2(0, 0);
        rectTra.offsetMax = new Vector2(0, 0);
        Vector3 pos = rectTra.anchoredPosition;
        pos.y = 13.2f;
        rectTra.anchoredPosition = pos;
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 26.4f);
    }

    static void SetImage(UnityEngine.UI.Image img)
    {
        Color col = img.color;
        col.a = 0.5f;
        img.color = col;
    }
}