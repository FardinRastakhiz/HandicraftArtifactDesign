///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <SQLiteExecute>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Data;
using Mono.Data.SqliteClient;

public static class SQLiteExecute {
    private static string dbPath = "URI=file:Assets/SistanArtifacts.db";
    private static IDbConnection conn;
    private static IDbCommand cmd;
    public static void SetDataPath()
    {
        dbPath = "URI=file:" + Application.dataPath + "/SistanArtifacts.db";
    }
    public static void Execute()
    {
        cmd.ExecuteNonQuery();
    }
    public static void CompleteExecute(string SQLQuery)
    {
        SQLiteExecute.NonReaderExecute(SQLQuery);
        SQLiteExecute.ExitQuery();
    }
    public static void SetDataPath(string path)
    {
        dbPath = path;
    }
    public static IDataReader ReadExecute(string SQLQuery)
    {
        return CreateQuery(SQLQuery).ExecuteReader();
    }

    public static IDataReader ChangeReadQuery(string SQLQuery)
    {
        cmd.CommandText = SQLQuery;
        return cmd.ExecuteReader();
    }

    public static void ChangeNonReadQuery(string SQLQuery)
    {
        cmd.CommandText = SQLQuery;
        cmd.ExecuteNonQuery();
    }

    public static void NonReaderExecute(string SQLQuery)
    {
        CreateQuery(SQLQuery).ExecuteNonQuery();
    }

    public static void BindParameter<T>(string name, ref T value)
    {
        SqliteParameter parameter = new SqliteParameter(name,value);
        cmd.Parameters.Add(parameter);
    }

    public static void ExitQuery()
    {
        conn.Close();
        cmd.Dispose();
    }
    
    public static IDbCommand CreateQuery(string SQLQuery)
    {
        StartQuery();
        cmd.CommandText = SQLQuery;
        return cmd;
    }

    public static void StartQuery()
    {
        conn = new SqliteConnection(dbPath);
        conn.Open();
        cmd = conn.CreateCommand();
    }
}