///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <Transforms>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;

public abstract class Transforms
{
    public static bool active;
    public abstract void On_Shape_Click();

    public abstract void On_Shape_Begin_Drag();

    public abstract void On_Free_Area_Click();

    public abstract void On_Free_Area_Begin_Drag(Transform board);

    public abstract void On_Free_Area_Drag(BoardPlan plan);

    public abstract void On_Drag_Exit();

    public abstract void ResetTools();
}