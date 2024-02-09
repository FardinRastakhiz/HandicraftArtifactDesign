using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWindow : MonoBehaviour {
    Window window;
    public void On(Window window)
    {
        this.window = window;
        if (window == null) 
            this.window = transform.GetComponentInParent<Window>();
        StartCoroutine(ProcessClose());
    }

    IEnumerator ProcessClose()
    {
        ShowCloseProcessing();
        yield return new WaitUntil(() => window.ClosePrerequisites.IsSatisfied());
        Destroy(window.gameObject);
        Debug.Log("CloseWindow");
        HideCloseProcessing();
    }
    void ShowCloseProcessing()
    {

    }
    void HideCloseProcessing()
    {

    }
}

