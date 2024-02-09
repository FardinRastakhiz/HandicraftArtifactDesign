using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlanButton : MonoBehaviour
{

    public void On(InformationWindow window)
    {
        window.Save_Changes();
    }
}
