using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveScroll : MonoBehaviour {
    [SerializeField]
    float Sensitivity = 0.1f;
    public void On_Move()
    {
        var mover = EventSystem.current.currentSelectedGameObject.transform;
        var name = mover.name;
        var scrollContent = mover.parent.Find("ScrollContent").GetComponent<UnityEngine.UI.ScrollRect>();
        
        switch (name)
        {
            case "left":
                scrollContent.horizontalScrollbar.value -= Sensitivity;
                break;
            case "right":
                scrollContent.horizontalScrollbar.value += Sensitivity;
                break;
            case "up":
                scrollContent.verticalScrollbar.value += Sensitivity;
                break;
            case "down":
                scrollContent.verticalScrollbar.value -= Sensitivity;
                break;
            default:
                break;
        }
    }
}