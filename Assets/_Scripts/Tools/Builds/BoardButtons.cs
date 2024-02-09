using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardButtons : MonoBehaviour {
    Board board;
    ScreenShot screenShotObject;
    void Start()
    {
        board = gameObject.GetComponent<Board>();
        screenShotObject = GameObject.Find("ScreenShot").GetComponent<ScreenShot>();
    }

    public void TakeScreenShot()
    {
        screenShotObject.Take_ScreenShot(board);
    }
}
