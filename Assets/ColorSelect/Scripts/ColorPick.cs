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
using System;
namespace Fardin.ColorTools
{
    public class ColorPick : MonoBehaviour {
        [SerializeField]
        Transform cursorObj;
        [SerializeField]
        RectTransform filter;
        UnityEngine.UI.Image showColor;

        RenderTexture screenTex;
        Texture2D tex;
        Rect rectRead;
        [SerializeField]
        Camera mainCamera;
        [SerializeField]
        Transform canvas;
        public bool isPicking;
        Vector2 targetPos;
        Color targetColor;
        bool isInWork;
        Color DefaultColor;

        public event EventHandler<OnPickColorHandler> OnPickColor;
        OnPickColorHandler onPickHandler;

        public Color TokenColor()
        {
            return targetColor;
        }
    
        public void On_Start_Picking()
        {
            if (!mainCamera)
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            if (!canvas)
                canvas = GetComponentInParent<Canvas>().transform;
            showColor = cursorObj.Find("Color").GetComponent<UnityEngine.UI.Image>();

            rectRead = new Rect(0, 0, 1, 1);
            screenTex = new RenderTexture(Screen.width, Screen.height, 24);
            tex = new Texture2D((int)rectRead.width, (int)rectRead.height, TextureFormat.RGB24, false);

            mainCamera.targetTexture = screenTex;
            RenderTexture.active = screenTex;
            //Cursor.visible = false;
            cursorObj.gameObject.SetActive(true);
            filter.gameObject.SetActive(true);
            cursorObj.SetParent(canvas);
            filter.SetParent(canvas);
            cursorObj.SetAsLastSibling();
            filter.SetAsLastSibling();
            filter.anchorMin = Vector2.zero;
            filter.anchorMax = Vector2.one;
            filter.offsetMin = Vector2.zero;
            filter.offsetMax = Vector2.zero;
            isPicking = true;
            onPickHandler = new OnPickColorHandler();
            onPickHandler.color = DefaultColor;
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
            if (mainCamera.targetTexture != screenTex)
            {
                mainCamera.targetTexture = screenTex;
                RenderTexture.active = screenTex;
                mainCamera.Render();
            }
            tex.ReadPixels(rectRead, 0, 0);


            targetColor = tex.GetPixel(0, 0);
            showColor.color = targetColor;
            onPickHandler.color = targetColor;
            if (OnPickColor!=null)
                OnPickColor(this, onPickHandler);
        }

        public void pick()
        {
            isPicking = false;
            isInWork = false;
            cursorObj.gameObject.SetActive(false);
            cursorObj.SetParent(transform);
            filter.SetParent(transform);
            filter.gameObject.SetActive(false);
            //Cursor.visible = true;
            mainCamera.targetTexture = null;
            RenderTexture.active = null;
            if (screenTex!=null)
            {
                DestroyImmediate(screenTex);
            }
        }

        int setInBound(int target, int min, int max)
        {
            return target < min ? min : (target > max ? max : target);
        }

        public void CutomChange(Color color)
        {
            onPickHandler = new OnPickColorHandler();
            onPickHandler.color = color;
            DefaultColor = color;
            targetColor = color;
            if (OnPickColor != null)
                OnPickColor(this, onPickHandler);
        }
    }

    public class OnPickColorHandler : EventArgs
    {
        public Color color;
    }
}