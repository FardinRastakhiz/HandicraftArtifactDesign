using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTextField : MonoBehaviour {
    public void OnChange(InformationWindow window)
    {
        if (window.Field.text != window.TargetPlan.description || window.Category != window.TargetPlan.category)
        {
            window.SaveButton.interactable = true;
        }
        else
        {
            window.SaveButton.interactable = false;
        }
    }
}
