///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SaveBoardPlan>
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

public class SaveBoardPlan {
    static string query = "";

    public static void StartQuery()
    {
        query = "begin;";
    }
    public static void Save(BoardPlan plan)
    {
        UpdatePlan(plan);
        StartQuery();
        query += "INSERT OR REPLACE INTO Plans VALUES('" + plan.id + "', '" + plan.name + "', '" + plan.sourceImage +
            "', '" + plan.category + "', '" + plan.root + "', '" + plan.sewing + "', '" + plan.design +
            "', '" + plan.description + "', '" + plan.Canvas + "', '" + plan.CanvasColor.r + "', '" + plan.CanvasColor.g +
            "', '" + plan.CanvasColor.b + "', '" + (plan.isOriginal ? 1 : 0) + "');";
        
        ReSaveParts(plan.name + "_parts", plan.parts);
        ReSavePrimitives(plan.name + "_primitives", plan.primitives);
        ReSaveBackgrounds(plan.name + "_backgrounds", plan.backgrounds);
        RunQuery();
    }
    
    public static void SaveNew(BoardPlan plan)
    {
        UpdatePlan(plan);
        StartQuery();
        query += "INSERT INTO Plans VALUES('" + plan.id + "', '" + plan.name + "', '" + plan.sourceImage +
            "', '" + plan.category + "', '" + plan.root + "', '" + plan.sewing + "', '" + plan.design +
            "', '" + plan.description + "', '" + plan.Canvas + "', '" + plan.CanvasColor.r + "', '" + plan.CanvasColor.g +
            "', '" + plan.CanvasColor.b + "', '" + (plan.isOriginal ? 1 : 0) + "');";
        
        CreatePartTables(plan.name + "_parts");
        CreatePrimitiveTables(plan.name + "_primitives");
        CreateBackgroundTables(plan.name + "_backgrounds");

        SaveParts(plan.name + "_parts", plan.parts, true);
        SavePrimitives(plan.name + "_primitives", plan.primitives, true);
        SaveBackgrounds(plan.name + "_backgrounds", plan.backgrounds, true);
        RunQuery();
    }

    static void UpdatePlan(BoardPlan activePlan)
    {
        activePlan.plan.canvas = activePlan.Canvas;
        activePlan.plan.canvasColor = activePlan.CanvasColor;
        activePlan.plan.category = activePlan.category;
        activePlan.plan.description = activePlan.description;
        activePlan.plan.design = activePlan.design;
    }

    public static void CreatePartTables(string name)
    {
        query += "CREATE TABLE IF NOT EXISTS '" + name + "'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourceImage' TEXT, 'width' DOUBLE, 'height' DOUBLE, 'indx' INTEGER, 'red' DOUBLE, 'green' DOUBLE,"+
            " 'blue' DOUBLE, 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE" +
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }
    public static void CreatePrimitiveTables(string name)
    {
        query += "CREATE TABLE IF NOT EXISTS '" + name + "'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourcePrimitive' INTEGER, 'width' DOUBLE, 'height' DOUBLE, 'red' DOUBLE, 'green' DOUBLE, 'blue' DOUBLE" +
            ", 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE"+
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }
    public static void CreateBackgroundTables(string name)
    {
        query += "CREATE TABLE IF NOT EXISTS '" + name + "'('id' INTEGER PRIMARY KEY UNIQUE," +
            " 'sourceShape' INTEGER, 'imageType' INTEGER, 'width' DOUBLE, 'height' DOUBLE, 'red' DOUBLE, 'green' DOUBLE, 'blue' DOUBLE" +
            ", 'alpha' DOUBLE, 'positionX' DOUBLE, 'positionY' DOUBLE, 'rotation' DOUBLE, 'scale' DOUBLE" +
        ", 'scaleX' DOUBLE, 'scaleY' DOUBLE, 'layoutOrder' INTEGER);";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }
    public static void ReSaveParts(string path, List<Part> parts)
    {
        DeleteTable(path);
        CreatePartTables(path);
        
        SaveParts(path, parts, true);
    }
    public static void ReSavePrimitives(string path, List<Primitive> primitives)
    {
        DeleteTable(path);
        CreatePrimitiveTables(path);
        SavePrimitives(path, primitives, true);
    }

    public static void ReSaveBackgrounds(string path, List<Background> backgrounds)
    {
        DeleteTable(path);
        CreateBackgroundTables(path);
        SaveBackgrounds(path, backgrounds, true);
    }

    static void SaveParts(string path, List<Part> parts, bool isNew)
    {
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].id = i + 1;
            RectTransform rectTra = parts[i].gameObject.GetComponent<RectTransform>();
            UnityEngine.UI.Image image = parts[i].gameObject.GetComponent<UnityEngine.UI.Image>();
            query += "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourceImage,width,height, indx, red, green, blue, alpha," +
            " positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder) VALUES('" + parts[i].id + "', '" + parts[i].sourceImage + "', '" + parts[i].size.x +
            "', '" + parts[i].size.y + "', '" + parts[i].index + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
            "', '" + image.color.a + "', '" + rectTra.anchoredPosition.x + "', '" + rectTra.anchoredPosition.y + "', '" + rectTra.localEulerAngles.z +
            "', '" + rectTra.localScale.x + "', '" + rectTra.sizeDelta.x + "', '" + rectTra.sizeDelta.y +
            "', '" + parts[i].order + "');";
            //if (i == parts.Count - 1)
            //{
            //    if (i==0)
            //        SQLiteExecute.NonReaderExecute(query);
            //    else
            //        SQLiteExecute.ChangeNonReadQuery(query);
            //    SQLiteExecute.ExitQuery();
            //}
            //else if (i==0)
            //    SQLiteExecute.NonReaderExecute(query);
            //else
            //    SQLiteExecute.ChangeNonReadQuery(query);
        }
    }

    static void SavePrimitives(string path, List<Primitive> primitives, bool isNew)
    {
        for (int i = 0; i < primitives.Count; i++)
        {
            primitives[i].id = i + 1;
            RectTransform prim = primitives[i].gameObject.GetComponent<RectTransform>();
            UnityEngine.UI.Image image = primitives[i].gameObject.GetComponent<UnityEngine.UI.Image>();
            query += "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourcePrimitive,width,height, red, green, blue, alpha" +
                ", positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder)" +
                " VALUES('" + primitives[i].id + "', '" + primitives[i].sourceShape + "', '" + primitives[i].size.x +
                "', '" + primitives[i].size.y + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
                "', '" + image.color.a + "', '" + prim.anchoredPosition.x + "', '" + prim.anchoredPosition.y + "', '" + prim.localEulerAngles.z +
                "', '" + prim.localScale.x + "', '" + prim.sizeDelta.x + "', '" + prim.sizeDelta.y +
                "', '" + primitives[i].order + "');";

            //if (i == primitives.Count - 1)
            //{
            //    if (i == 0)
            //        SQLiteExecute.NonReaderExecute(query);
            //    else
            //        SQLiteExecute.ChangeNonReadQuery(query);
            //    SQLiteExecute.ExitQuery();
            //}
            //else if (i == 0)
            //    SQLiteExecute.NonReaderExecute(query);
            //else
            //    SQLiteExecute.ChangeNonReadQuery(query);
		}
    }
    static void SaveBackgrounds(string path, List<Background> background, bool isNew)
    {
        for (int i = 0; i < background.Count; i++)
        {
            background[i].id = i + 1;
            RectTransform bg = background[i].gameObject.GetComponent<RectTransform>();
            UnityEngine.UI.Image image = background[i].gameObject.GetComponent<UnityEngine.UI.Image>();
            query += "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourceShape, imageType, width, height, red," +
                " green, blue, alpha, positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder)" +
                " VALUES('" + background[i].id + "', '" + background[i].sourceShape + "', '" + (int)image.type + "', '" + background[i].size.x +
                "', '" + background[i].size.y + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
                "', '" + image.color.a + "', '" + bg.anchoredPosition.x + "', '" + bg.anchoredPosition.y + "', '" + bg.localEulerAngles.z +
                "', '" + bg.localScale.x + "', '" + bg.sizeDelta.x + "', '" + bg.sizeDelta.y +
                "', '" + background[i].order + "');";

            //if (i == primitives.Count - 1)
            //{
            //    if (i == 0)
            //        SQLiteExecute.NonReaderExecute(query);
            //    else
            //        SQLiteExecute.ChangeNonReadQuery(query);
            //    SQLiteExecute.ExitQuery();
            //}
            //else if (i == 0)
            //    SQLiteExecute.NonReaderExecute(query);
            //else
            //    SQLiteExecute.ChangeNonReadQuery(query);
        }
    }
    /*
    public static void SaveTransforms(int id, Transform2D t)
    {
        string query = "INSERT INTO Transforms(id, positionX, positionY, rotation, size) VALUES(" +
            id + ", " + t.position.x + ", " + t.position.y + ", " + t.rotation + ", " + t.size + ");";
        SQLiteExecute.ChangeNonReadQuery(query);
    }
    public static void SaveColors(int id, Color c)
    {
        string query = "INSERT INTO Colors(id, red, green, blue, alpha) VALUES(" +
            id + ", " + c.r + ", " + c.g + ", " + c.b + ", " + c.a + ");";
        SQLiteExecute.ChangeNonReadQuery(query);
    }
    */
    public static void AddNewPart(string path, Part part, bool isNew)
    {
        RectTransform rectTra = part.gameObject.GetComponent<RectTransform>();
        UnityEngine.UI.Image image = part.gameObject.GetComponent<UnityEngine.UI.Image>();
        string query = "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourceImage,width,height, indx, red, green, blue, alpha,"+
        " positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder) VALUES('" + part.id + "', '" + part.sourceImage + "', '" + part.size.x +
        "', '" + part.size.y + "', '" + part.index + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
        "', '" + image.color.a + "', '" + rectTra.anchoredPosition.x + "', '" + rectTra.anchoredPosition.y + "', '" + rectTra.localEulerAngles.z +
        "', '" + rectTra.localScale.x + "', '" + rectTra.sizeDelta.x + "', '" + rectTra.sizeDelta.y +
        "', '" + part.order + "');";
        SQLiteExecute.NonReaderExecute(query);
        SQLiteExecute.ExitQuery();
    }

    public static void AddNewPrimitive(string path, Primitive primitive, bool isNew)
    {
        RectTransform prim = primitive.gameObject.GetComponent<RectTransform>();
        UnityEngine.UI.Image image = primitive.gameObject.GetComponent<UnityEngine.UI.Image>();
        string query = "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourcePrimitive,width,height, red, green, blue, alpha" +
            ", positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder) VALUES('" + primitive.id + "', '" + primitive.sourceShape + "', '" +
            primitive.size.x + "', '" + primitive.size.y + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
            "', '" + image.color.a + "', '" + prim.anchoredPosition.x + "', '" + prim.anchoredPosition.y + "', '" + prim.localEulerAngles.z +
            "', '" + prim.localScale.x + "', '" + prim.sizeDelta.x + "', '" + prim.sizeDelta.y +
             "', '" + primitive.order + "');";
        SQLiteExecute.NonReaderExecute(query);
        SQLiteExecute.ExitQuery();
    }
    public static void AddNewBackground(string path, Background background, bool isNew)
    {
        RectTransform bg = background.gameObject.GetComponent<RectTransform>();
        UnityEngine.UI.Image image = background.gameObject.GetComponent<UnityEngine.UI.Image>();
        query += "INSERT" + (isNew ? "" : " OR REPLACE") + " INTO '" + path + "'(id, sourceShape, imageType, width, height, red," +
                " green, blue, alpha, positionX, positionY, rotation, scale, scaleX, scaleY, layoutOrder)" +
                " VALUES('" + background.id + "', '" + background.sourceShape + "', '" + (int)image.type + "', '" + background.size.x +
                "', '" + background.size.y + "', '" + image.color.r + "', '" + image.color.g + "', '" + image.color.b +
                "', '" + image.color.a + "', '" + bg.anchoredPosition.x + "', '" + bg.anchoredPosition.y + "', '" + bg.localEulerAngles.z +
                "', '" + bg.localScale.x + "', '" + bg.sizeDelta.x + "', '" + bg.sizeDelta.y +
                "', '" + background.order + "');";
        SQLiteExecute.NonReaderExecute(query);
        SQLiteExecute.ExitQuery();
    }

    public static void DeleteTable(string path)
    {
        query += "DROP TABLE IF EXISTS '" + path + "';";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }
    public static void deleteRow(string path, int id)
    {
        query += "DELETE FROM '"+path+"' WHERE id = "+id+";";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }
    public static void deleteRow(string path, string name)
    {
        query += "DELETE FROM '" + path + "' WHERE name = " + name + ";";
        //SQLiteExecute.NonReaderExecute(query);
        //SQLiteExecute.ExitQuery();
    }

    public static void RunQuery()
    {
        query += "end;";
        SQLiteExecute.CompleteExecute(query);
    }
    public static void SaveCategories(HashSet<string> categories)
    {
        int i = 1;
        foreach (var item in categories)
	    {
            string query = "INSERT OR REPLACE INTO Categories(id, name) VALUES('" + (i++) + "', '" + item + "');";
            SQLiteExecute.NonReaderExecute(query);
            SQLiteExecute.ExitQuery();
	    }
    }
}