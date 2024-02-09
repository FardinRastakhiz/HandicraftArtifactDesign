///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <GenColors>
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

public class GenColors {
    List<Color> colors;
    public GenColors()//need a little more work
    {
        colors = new List<Color>();
        string query = "SELECT * FROM Colors";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        while (reader.Read())
        {
            Color color = new Color();
            color.r = (int)reader["red"];
            color.g = (int)reader["green"];
            color.b = (int)reader["blue"];
            color.a = (int)reader["alpha"];
            colors.Add(color);
        }
        reader.Dispose();
        reader.Close();
        SQLiteExecute.ExitQuery();

    }
}
