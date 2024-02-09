///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <LoadBoardPlan>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>


using UnityEngine;
using System.Collections;
using System.Data;
using Mono.Data.SqliteClient;
using System.Collections.Generic;

public class LoadBoardPlan {
    
    public static BoardPlan Load(string planName)
    {
        GameObject planObject = new GameObject(planName);
        planObject.AddComponent<BoardPlan>();
        //planObject.transform.parent = ;
        BoardPlan newPlan = planObject.GetComponent<BoardPlan>();
        newPlan.name = planName;

        newPlan.orders = new Dictionary<int, Shape>();


        newPlan.indexInOrder = new List<int>();
        newPlan.shapeIDs = new List<int>();
        LoadPlanBody(newPlan);
        LoadParts(newPlan);
        LoadPrimitives(newPlan);
        LoadBackgrounds(newPlan);
        newPlan.shapeIDs.Sort();
        return newPlan;
    }

    static void LoadPlanBody(BoardPlan plan)
    {
        string query = "SELECT * FROM 'Plans' WHERE name = '" + plan.name + "';";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        if (reader.Read())
        {
            plan.id = (int)reader["id"];
            plan.name = (string)reader["name"];
            plan.sourceImage = (string)reader["sourceImage"];
            plan.category = (string)reader["category"];
            plan.root = (string)reader["root"];
            plan.sewing = (string)reader["sewing"];
            plan.design = (string)reader["design"];
            plan.description = (string)reader["description"];
            plan.Canvas = (int)reader["Canvas"];
            plan.CanvasColor.r = (float)System.Convert.ToDouble(reader["CanvasColorR"]);
            plan.CanvasColor.g = (float)System.Convert.ToDouble(reader["CanvasColorG"]);
            plan.CanvasColor.b = (float)System.Convert.ToDouble(reader["CanvasColorB"]);
            plan.CanvasColor.a = 1.0f;
            plan.isOriginal = (int)reader["Original"] == 1 ? true : false;
        }
    }

    static void LoadParts(BoardPlan plan)
    {
        plan.parts = new List<Part>();
        string query = "SELECT * FROM '" + plan.name + "_parts';";
        IDataReader reader = SQLiteExecute.ChangeReadQuery(query);
        while (reader.Read())
        {
            GameObject partObj = new GameObject("part");
            partObj.AddComponent<Part>();
            //partObj.transform.parent;
            Part newPart = partObj.GetComponent<Part>();
            newPart.id = (int)reader["id"];
            //newPart.colorID = (int)reader["colorID"];
            //newPart.transformID = (int)reader["transformID"];
            newPart.sourceImage = (string)reader["sourceImage"];
            newPart.size.x = (float)System.Convert.ToDouble(reader["width"]);
            newPart.size.y = (float)System.Convert.ToDouble(reader["height"]);
            newPart.index = (int)reader["indx"];
            newPart.transform2D = new Transform2D();
            newPart.color = new Color();
            newPart.color.r = (float)System.Convert.ToDouble(reader["red"]);
            newPart.color.g = (float)System.Convert.ToDouble(reader["green"]);
            newPart.color.b = (float)System.Convert.ToDouble(reader["blue"]);
            newPart.color.a = (float)System.Convert.ToDouble(reader["alpha"]);
            newPart.transform2D.position.x = (float)System.Convert.ToDouble(reader["positionX"]);
            newPart.transform2D.position.y = (float)System.Convert.ToDouble(reader["positionY"]);
            newPart.transform2D.rotation = (float)System.Convert.ToDouble(reader["rotation"]);
            newPart.transform2D.size = (float)System.Convert.ToDouble(reader["Scale"]);
            newPart.transform2D.scale.x = (float)System.Convert.ToDouble(reader["scaleX"]);
            newPart.transform2D.scale.y = (float)System.Convert.ToDouble(reader["scaleY"]);
            plan.shapeIDs.Add(newPart.id);
            newPart.order = (int)reader["layoutOrder"];
            plan.orders.Add(newPart.order, newPart);
            plan.indexInOrder.Add(newPart.order);
            plan.parts.Add(newPart);
        }
    }

    static void LoadPrimitives(BoardPlan plan)
    {
        plan.primitives = new List<Primitive>();
        string query = "SELECT * FROM '" + plan.name + "_primitives';";
        IDataReader reader = SQLiteExecute.ChangeReadQuery(query);
        while (reader.Read())
        {
            GameObject primObj = new GameObject("primitive");
            primObj.AddComponent<Primitive>();
            //primObj.transform.parent;
            Primitive newPrim = primObj.GetComponent<Primitive>();
            newPrim.id = (int)reader["id"];
            //newPrim.colorID = (int)reader["colorID"];
            //newPrim.transformID = (int)reader["transformID"];
            newPrim.sourceShape = (int)reader["sourcePrimitive"];
            newPrim.size.x = (float)System.Convert.ToDouble(reader["width"]);
            newPrim.size.y = (float)System.Convert.ToDouble(reader["height"]);
            newPrim.transform2D = new Transform2D();
            newPrim.color = new Color();
            newPrim.color.r = (float)System.Convert.ToDouble(reader["red"]);
            newPrim.color.g = (float)System.Convert.ToDouble(reader["green"]);
            newPrim.color.b = (float)System.Convert.ToDouble(reader["blue"]);
            newPrim.color.a = (float)System.Convert.ToDouble(reader["alpha"]);
            newPrim.transform2D.position.x = (float)System.Convert.ToDouble(reader["positionX"]);
            newPrim.transform2D.position.y = (float)System.Convert.ToDouble(reader["positionY"]);
            newPrim.transform2D.rotation = (float)System.Convert.ToDouble(reader["rotation"]);
            newPrim.transform2D.size = (float)System.Convert.ToDouble(reader["Scale"]);
            newPrim.transform2D.scale.x = (float)System.Convert.ToDouble(reader["scaleX"]);
            newPrim.transform2D.scale.y = (float)System.Convert.ToDouble(reader["scaleY"]);
            newPrim.order = (int)reader["layoutOrder"];
            plan.shapeIDs.Add(newPrim.id);
            plan.orders.Add(newPrim.order, newPrim);
            plan.indexInOrder.Add(newPrim.order);
            plan.primitives.Add(newPrim);
        }
    }

    static void LoadBackgrounds(BoardPlan plan)
    {
        plan.backgrounds = new List<Background>();

        string query = "SELECT * FROM '" + plan.name + "_backgrounds';";
        IDataReader reader = SQLiteExecute.ChangeReadQuery(query);
        while (reader.Read())
        {
            GameObject bgObj = new GameObject("background");
            bgObj.AddComponent<Background>();
            //primObj.transform.parent;
            Background newBG= bgObj.GetComponent<Background>();
            newBG.id = (int)reader["id"];
            //newBG.colorID = (int)reader["colorID"];
            //newBG.transformID = (int)reader["transformID"];
            newBG.sourceShape = (int)reader["sourceShape"];

            newBG.imageType = (UnityEngine.UI.Image.Type)((int)reader["imageType"]);
            newBG.size.x = (float)System.Convert.ToDouble(reader["width"]);
            newBG.size.y = (float)System.Convert.ToDouble(reader["height"]);
            newBG.transform2D = new Transform2D();
            newBG.color = new Color();
            newBG.color.r = (float)System.Convert.ToDouble(reader["red"]);
            newBG.color.g = (float)System.Convert.ToDouble(reader["green"]);
            newBG.color.b = (float)System.Convert.ToDouble(reader["blue"]);
            newBG.color.a = (float)System.Convert.ToDouble(reader["alpha"]);
            newBG.transform2D.position.x = (float)System.Convert.ToDouble(reader["positionX"]);
            newBG.transform2D.position.y = (float)System.Convert.ToDouble(reader["positionY"]);
            newBG.transform2D.rotation = (float)System.Convert.ToDouble(reader["rotation"]);
            newBG.transform2D.size = (float)System.Convert.ToDouble(reader["Scale"]);
            newBG.transform2D.scale.x = (float)System.Convert.ToDouble(reader["scaleX"]);
            newBG.transform2D.scale.y = (float)System.Convert.ToDouble(reader["scaleY"]);
            newBG.order = (int)reader["layoutOrder"];
            plan.shapeIDs.Add(newBG.id);
            plan.orders.Add(newBG.order, newBG);
            plan.indexInOrder.Add(newBG.order);
            plan.backgrounds.Add(newBG);
        }
    }

    public static int GetID(string tableName)
    {
        int id = 1;
        string query = "SELECT id FROM '" + tableName + "' ORDER BY id DESC;";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        if (reader.Read())
        {
            id = (int)reader["id"] + 1;
        }
        reader.Dispose();
        SQLiteExecute.ExitQuery();
        return id;
    }

    public static HashSet<string> LoadCategories()
    {
        HashSet<string> categories = new HashSet<string>();
        string query = "SELECT name FROM 'Categories';";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        while (reader.Read())
        {
            categories.Add((string)reader["name"]);
        }
        return categories;
    }
}
