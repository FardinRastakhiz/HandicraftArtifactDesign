///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ColorPick>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>


using UnityEngine;
using System.Collections;

public class ColorPick : MonoBehaviour {
    [SerializeField]
    Transform cursorObj;
    [SerializeField]
    GameObject filter;
    UnityEngine.UI.Image showColor;

    RenderTexture screenTex;
    Texture2D tex;
    Rect rectRead;
    Camera mainCam;
    bool isPicking;
    Vector2 targetPos;
    Color targetColor;
    bool isInWork;

    public void StartPicking()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rectRead = new Rect(0, 0, 1, 1);
        screenTex = new RenderTexture(Screen.width, Screen.height, 24);
        tex = new Texture2D((int)rectRead.width, (int)rectRead.height, TextureFormat.RGB24, false);

        mainCam.targetTexture = screenTex;
        RenderTexture.active = screenTex;
        Cursor.visible = false;
        cursorObj.gameObject.SetActive(true);
        filter.SetActive(true);
        showColor = cursorObj.Find("Color").GetComponent<UnityEngine.UI.Image>();
        isPicking = true;
    }

    void LateUpdate()
    {
        if (isPicking)
        {
            StartCoroutine(GetScreenColor());
            if (Input.GetMouseButtonUp(0))
            {
                if (!isInWork)
                    isInWork = true;
                else
                    pick();
            }
        }
    }

    IEnumerator GetScreenColor()
    {
        yield return new WaitForEndOfFrame();
        targetPos = Input.mousePosition;
        cursorObj.position = targetPos;
        rectRead.x = setInBound((int)targetPos.x, 0, Screen.width - 1);
        rectRead.y = setInBound((int)targetPos.y, 0, Screen.height - 1);
        if (mainCam.targetTexture != screenTex)
        {
            mainCam.targetTexture = screenTex;
            RenderTexture.active = screenTex;
            mainCam.Render();
        }
        tex.ReadPixels(rectRead, 0, 0);


        targetColor = tex.GetPixel(0, 0);
        showColor.color = targetColor;
    }

    public void pick()
    {
        isPicking = false;
        isInWork = false;
        cursorObj.gameObject.SetActive(false);
        filter.SetActive(false);
        Cursor.visible = true;
        mainCam.targetTexture = null;
        RenderTexture.active = null;
        if (screenTex!=null)
        {
            DestroyImmediate(screenTex);
        }
        ColorTools.SetSelectionsColor(targetColor);

    }

    int setInBound(int target, int min, int max)
    {
        return target < min ? min : (target > max ? max : target);
    }
}
