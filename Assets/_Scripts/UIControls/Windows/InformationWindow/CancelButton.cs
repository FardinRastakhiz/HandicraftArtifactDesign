using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour {
    public void On(Window window)
    {
        window.Close();
    }
}
