using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlanButton : MonoBehaviour {
    
    public void On(InformationWindow window)
    {
        DeleteWarning.Show(window);
    }

}
