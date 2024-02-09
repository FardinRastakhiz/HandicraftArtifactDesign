///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SaveOutputs>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.IO;

public class SaveOutputs : MonoBehaviour {
    public static void Save(BoardPlan plan, int width, int height, byte[] Image)
    {
        SQLiteExecute.SetDataPath("URI=file:" + Application.dataPath + "/ArtifactBuilds.db");
        string query = "INSERT OR REPLACE INTO Plans VALUES('" + plan.id + "', '" + plan.name + "', '" + width + "', '" + height + "', @Image)";
        SQLiteExecute.CreateQuery(query);
        SQLiteExecute.BindParameter<byte[]>("@Image", ref Image);
        SQLiteExecute.Execute();
        SQLiteExecute.ExitQuery();
        SQLiteExecute.SetDataPath();
        string path = Application.dataPath + "/ScreenShots/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllBytes(path + plan.sourceImage + ".png", Image);
        SQLiteExecute.SetDataPath();
    }

    public static void DeleteRow(int id)
    {
        SQLiteExecute.SetDataPath("URI=file:" + Application.dataPath + "/ArtifactBuilds.db");
        string query = "DELETE FROM 'Plans' WHERE id = " + id + ";";
        SQLiteExecute.CompleteExecute(query);
        SQLiteExecute.SetDataPath();
    }
}