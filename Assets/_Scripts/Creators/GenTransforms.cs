///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Clss Namespacea>
///   Class:          <GenTransforms>
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

public class GenTransforms {
    List<Transform2D> transforms;

    public GenTransforms()//need a little more work
    {
        transforms = new List<Transform2D>();
        string query = "SELECT * FROM Transforms";
        IDataReader reader = SQLiteExecute.ReadExecute(query);
        while (reader.Read())
        {
            Transform2D transform = new Transform2D();
            transform.position.x = (float)reader["X"];
            transform.position.y = (float)reader["Y"];
            transform.rotation = (float)reader["rotation"];
            transform.size = (float)reader["size"];
            transforms.Add(transform);
        }
        reader.Dispose();
        reader.Close();
        SQLiteExecute.ExitQuery();
    }
}
