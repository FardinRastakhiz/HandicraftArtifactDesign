///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CreatDBsEditor>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Collections.Generic;

[CustomEditor(typeof(CreateDBs))]
public class CreatDBsEditor : Editor
{
    GameObject activePlan;
    Vector2 lastMousePosition;
    string planSource = "";
    //string resx = "";
    //string resy = "";
    //string parx = "";
    //string pary = "";
    //int[] resInt = new int[2];
    //int[] parInt = new int[2];


    public override void OnInspectorGUI()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Label("Plan Source: ", GUILayout.Width(50));
        planSource = GUILayout.TextArea(planSource);
        GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Resolutin: ", GUILayout.Width(50));
        //resx = GUILayout.TextField(resx, GUILayout.Width(40));
        //resy = GUILayout.TextField(resy, GUILayout.Width(40));
        //GUILayout.EndHorizontal();


        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Partitions: ", GUILayout.Width(50));
        //parx = GUILayout.TextField(parx, GUILayout.Width(40));
        //pary = GUILayout.TextField(pary, GUILayout.Width(40));
        //GUILayout.EndHorizontal();

        //resx = Regex.Replace(resx == "" ? "0" : resx, @"[^0-9]", "");
        //resy = Regex.Replace(resy == "" ? "0" : resy, @"[^0-9]", "");
        //parx = Regex.Replace(parx == "" ? "0" : parx, @"[^0-9]", "");
        //pary = Regex.Replace(pary == "" ? "0" : pary, @"[^0-9]", "");

        //resInt[0] = System.Convert.ToInt32(resx);
        //resInt[1] = System.Convert.ToInt32(resy);
        //parInt[0] = System.Convert.ToInt32(parx);
        //parInt[1] = System.Convert.ToInt32(pary);

        if (GUILayout.Button("Add To Database", GUILayout.Width(100)))
        {
            if ((lastMousePosition - Event.current.mousePosition).sqrMagnitude < 10000)
                return;
            lastMousePosition = Event.current.mousePosition;
            SavePlan(planSource);
        }


    }

    public void SavePlan(string path)
    {
        GameObject planObj = new GameObject();
        planObj.AddComponent<Plan>();
        Plan plan = planObj.GetComponent<Plan>();
        plan.name = path;

        GameObject activePlanObj = new GameObject();
        activePlanObj.AddComponent<BoardPlan>();
        BoardPlan activePlan = activePlanObj.GetComponent<BoardPlan>();
        activePlan.name = path;
        activePlan.plan = plan;
        activePlan.sourceImage = "PlansFolder/" + path;
        activePlan.id = LoadBoardPlan.GetID("Plans");
        activePlan.category = "main";
        Sprite[] parts = Resources.LoadAll<Sprite>("PlansFolder/" + path + "_Parts");
        activePlan.primitives = new List<Primitive>();
        activePlan.backgrounds = new List<Background>();
        activePlan.parts = new List<Part>();

        int partIDs = 1;
        //int colorIDs = LoadBoardPlan.GetID("Colors");
        //int transformIDs = LoadBoardPlan.GetID("Transforms");
        //int partWidth = resInt[0] / parInt[0];
        //int partHeight = resInt[1] / parInt[1];
        for (int i = 0; i < parts.Length; i++)
        {
            GameObject testObject = new GameObject();
            testObject.AddComponent<Part>();
            testObject.AddComponent<RectTransform>();
            testObject.AddComponent<UnityEngine.UI.Image>();
            RectTransform rectTra = testObject.GetComponent<RectTransform>();
            UnityEngine.UI.Image image = testObject.GetComponent<UnityEngine.UI.Image>();
            testObject.transform.parent = activePlan.transform;
            Part part = testObject.GetComponent<Part>();
            part.id = partIDs++;
            part.order = part.id;
            part.sourceImage = "PlansFolder/" + path + "_Parts";
            part.index = i;
            part.transform2D = new Transform2D();
            part.transform2D.position = parts[i].textureRect.position;
            //part.transform2D.position.y = resInt[1] - (Mathf.Floor(i / (1.0f * parInt[0])) + 0.5f) * partHeight;
            part.transform2D.rotation = 0.0f;
            part.transform2D.size = 1.0f;
            part.transform2D.scale = new Vector2(1.0f, 1.0f);
            rectTra.localPosition = new Vector3(part.transform.position.x, part.transform.position.y, 0);
            rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rectTra.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            image.color = Color.gray;
            //part.transformID = transformIDs++;
            part.color = Color.gray;
            //part.colorID = colorIDs++;
            part.size.x = parts[i].textureRect.width;
            part.size.y = parts[i].textureRect.height;
            activePlan.parts.Add(part);
        }
        SaveBoardPlan.SaveNew(activePlan);
        DestroyImmediate(activePlan.gameObject, false);
        DestroyImmediate(plan.gameObject, false);
    }
}
