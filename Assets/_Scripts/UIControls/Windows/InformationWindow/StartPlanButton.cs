using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlanButton : MonoBehaviour
{
    public void On(InformationWindow window)
    {
        window.Save_Changes();
        GenBoardPlan.Generate(window.TargetPlan);
        window.Close();
    }
}
