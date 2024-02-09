///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ScreenShot>
///   Description:    <Taking screenShot from target plan for different usecases>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/09>
///   Notes:          <Notes>
///   Revision History:
///   Name:           Date:        Description:
///-----------------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
//using System.Threading;
//using System;


public class ScreenShot : MonoBehaviour {

    [SerializeField]
    GameObject Loading;
    [SerializeField]
    GameObject saveWarning;

    Texture2D screenShot;
    static int width;
    static int height;
    public static bool takingShot;
    GameObject newObj;
    GameObject Gridd;
    Image DrawCanvasImage;
    Image GridImage;
    Sprite gridSprite;
    Color gridColor;
    RectTransform saveRect;
    RectTransform GridrectTra;
    string screenshotPath;
    string screenshotname;
    Board activeBoard;


    public void Take_ScreenShot(Board board)
    {
        if (board.plan.isOriginal)
        {
            SaveWarning.Show(board);
        }
        else
        {
            takingShot = true;
            StartCoroutine(ProcessScreenshot(board));
        }

        //if (!takingShot && board.plan.category != "PatternPlans")
        //{
        //    takingShot = true;
        //    StartCoroutine(ProcessScreenshot(board));
        //}
        //else
        //{
        //    SaveWarning.board = board;
        //    saveWarning.SetActive(true);
        //}
    }
    public static bool TakeCompleteShot = false;
    public IEnumerator Take_All_BoardPlans()
    {
        if (!TakeCompleteShot)
        {
            TakeCompleteShot = true;
            Debug.Log(":::" + BoardPlans.boardPlans.Count);
            for (int i = 0; i < BoardPlans.boardPlans.Count; )
            {
                Debug.Log("ScreenShot.takingShot:" + !ScreenShot.takingShot);
                yield return new WaitUntil(() => !ScreenShot.takingShot);
                Debug.Log("ScreenShot.takingShot:" + ScreenShot.takingShot);
                if (!ScreenShot.takingShot)
                {
                    ScreenShot.takingShot = true;
                    Board board = BoardPlans.boardPlans[i].board;
                    Take_ScreenShot(board);
                    i++;
                }
            }
            yield return new WaitUntil(() => ScreenShot.takingShot == false);
            TakeCompleteShot = false;
        }
    }

    bool DoesntTakingShot()
    {
        return !ScreenShot.takingShot;
    }


    public void ForceTakeScreenShot(Board board)
    {
        StartCoroutine(ProcessScreenshot(board));
    }

    public void TakeCopyScreenShot(Board copyBoard, Board mainBoard)
    {
        StartCoroutine(ProcessScreenshot(copyBoard));
    }

    public IEnumerator ProcessScreenshot(Board board)
    {
        takingShot = true;
        yield return SelectTools.ResetTotal();
        activeBoard = board;
        Loading.SetActive(true);
        yield return SetParameters();
        yield return TakeTheShot();
        
        while (!File.Exists(screenshotPath))
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return RestoreEveryThing();
        yield return LoadTexture(Screen.width, Screen.height);
        yield return CutBorder();
        while (!File.Exists(screenshotPath))
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return SaveImage();
        yield return ChangePlanImage(activeBoard.plan.name, activeBoard.plan.sourceImage);
        yield return GenBoardPlan.SavePlan(activeBoard.plan);
        FinishScreenShot(board);
    }

    void FinishScreenShot(Board board)
    {
        takingShot = false;
        if (Board.close)
        {
            StartCoroutine(board.Close_Active_Plan());
            Board.close = false;
        }
        Loading.SetActive(false);
    }


    bool SetParameters()
    {
        screenshotname = "_"+Mathf.Abs(activeBoard.plan.name.GetHashCode());
        screenshotPath = Application.dataPath + "/Resources/" + screenshotname + "_ScreenShot.png";
        width = Screen.height - 1;
        height = Screen.height;
        //ColorPicker.useExternalDrawer = true;
        Gridd = activeBoard.transform.Find("Mask").Find("Grid").gameObject;
        RemoveExcess(Gridd.transform, false);
        //Gridd.transform.FindChild("");
        newObj = new GameObject();
        newObj.AddComponent<RectTransform>();
        saveRect = newObj.GetComponent<RectTransform>();
        GridrectTra = Gridd.GetComponent<RectTransform>();
        DrawCanvasImage = Gridd.transform.Find("DrawCanvas").GetComponent<Image>();
        //gridSprite = DrawCanvasImage.sprite;
        GridImage = Gridd.GetComponent<Image>();
        gridColor = GridImage.color;

        if ((activeBoard.transform.Find("HideBackground").GetComponent<Toggle>().isOn))
        {
            DrawCanvasImage.sprite = null;
            DrawCanvasImage.color = new Color(249.0f/255.0f, 247.0f/255.0f, 174.0f/255.0f, 1.0f);
            GridImage.color = new Color(249.0f / 255.0f, 247.0f / 255.0f, 174.0f / 255.0f, 1.0f);
        }

        RectTools.Copy(GridrectTra, saveRect);
        Gridd.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.parent);
        Gridd.transform.SetAsLastSibling();
        GridrectTra.localPosition = Vector3.zero;
        GridrectTra.localScale = new Vector3(1, 1, 1);
        float scaleSize = Mathf.Min(Screen.width / (GridrectTra.rect.width * 1.0f * GridrectTra.lossyScale.x),
            Screen.height / (GridrectTra.rect.height * 1.0f * GridrectTra.lossyScale.y));
        GridrectTra.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
        if (File.Exists(screenshotPath))
            File.Delete(screenshotPath);
        return true;
    }
    bool TakeTheShot()
    {
        ScreenCapture.CaptureScreenshot(screenshotPath);
        return true;
    }
    bool RestoreEveryThing()
    {
        //yield return new WaitForSeconds(0.5f);
        DrawCanvasImage.sprite = ShapeCenter.boardCanvas[activeBoard.plan.Canvas];
        DrawCanvasImage.color = activeBoard.plan.CanvasColor;
        GridImage.color = gridColor;
        activeBoard.plan.sourceImage = screenshotname + "_ScreenShot";
        Gridd.transform.SetParent(activeBoard.transform.Find("Mask"));
        Gridd.transform.SetAsLastSibling();
        RemoveExcess(Gridd.transform, true);
        RectTools.Copy(saveRect, GridrectTra);
        //ColorPicker.useExternalDrawer = false;
        Destroy(newObj);
        //StartCoroutine(LoadTexture(screenshotPath, Screen.width, Screen.height));
        return true;
    }

    bool LoadTexture(int width, int height)
    {
        Texture2D newTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        if (File.Exists(screenshotPath))
        {
            byte[] texData = File.ReadAllBytes(screenshotPath);
            newTex.LoadImage(texData);
            //yield return newTex.LoadImage(texData);
            if (width < height)
            {
                screenShot = new Texture2D(newTex.width, newTex.width - 1, TextureFormat.RGBA32, false);
                screenShot.SetPixels(newTex.GetPixels(0, (int)((height - width) / 2.0f) + 1,
                    newTex.width, newTex.width - 1));
                ScreenShot.width = width;
                ScreenShot.height = width - 1;
            }
            else
            {
                screenShot = new Texture2D(newTex.height - 1, newTex.height, TextureFormat.RGBA32, false);
                screenShot.SetPixels(newTex.GetPixels((int)((width - height) / 2.0f) + 1, 0,
                    newTex.height - 1, newTex.height));
                ScreenShot.width = height - 1;
                ScreenShot.height = height;
            }
            //yield return new WaitForSeconds(0.25f);
            File.WriteAllBytes(screenshotPath, screenShot.EncodeToPNG());
        }
        return true;
    }

    public static bool ChangePlanImage(string planName, string sourceImage)
    {
        //yield return new WaitForSeconds(1.25f);
        Texture2D newTex = new Texture2D(width, height, TextureFormat.RGB24, false);
        for (int i = 0; i < GenPlans.plans.Count; i++)
        {
            if (GenPlans.plans[i].name == planName)
            {
                if (File.Exists(Application.dataPath + "/Resources/" + sourceImage + ".png"))
                {
                    byte[] imageData = File.ReadAllBytes(Application.dataPath + "/Resources/" + sourceImage + ".png");
                    newTex.LoadImage(imageData);
                    //yield return newTex.LoadImage(imageData);
                    GenPlans.plans[i].transform.Find("PlanImage").GetComponent<Image>().sprite =
                        Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height),
                        new Vector2(0.5f, 0.5f), 1.0f);
                    GenPlans.plans[i].sourceImage = sourceImage;
                }
                break;
            }
        }
        return true;
    }
    

    void RemoveExcess(Transform parent, bool state)
    {
        var _item = parent.Find("Image(Clone)");
        if (_item != null)
        {
            Destroy(_item.gameObject);
        }
        _item = parent.Find("MoveArea(Clone)");

        if (_item != null)
        {
            SetTransformsOn(_item, state);
        }
        _item = parent.Find("RotateArea(Clone)");
        if (_item != null)
        {
            SetTransformsOn(_item, state);
        }
        _item = parent.Find("ScaleArea(Clone)");
        if (_item != null)
        {
            SetTransformsOn(_item, state);
        }
    }

    void SetTransformsOn(Transform traObj, bool state)
    {
        traObj.Find("Right").gameObject.SetActive(state);
        traObj.Find("Top").gameObject.SetActive(state);
        traObj.Find("Left").gameObject.SetActive(state);
        traObj.Find("Down").gameObject.SetActive(state);
        traObj.Find("border").gameObject.SetActive(state);
    }
    bool CutBorder()
    {
        //yield return new WaitForSeconds(1.7f);

        Color32[] pixels = screenShot.GetPixels32();
        System.Collections.Generic.HashSet<Color32> BGColor = new System.Collections.Generic.HashSet<Color32>();
        for (int i = 0; i < 10; i++)
        {
            BGColor.Add(new Color((249.0f-3.0f*i) / 255.0f, (247.0f-3.0f*i) / 255.0f, (174.0f-2.0f*i) / 255.0f, 1.0f));
        }
        Color32 Target = new Color(0, 0, 0, 0);
        for (int i = pixels.Length - 1; i >= 0; i--)
        {
            if (BGColor.Contains(pixels[i]))
            {
                pixels[i] = Target;
            }
        }
        screenShot.SetPixels32(pixels);
        return true;
    }
    bool SaveImage()
    {
        File.WriteAllBytes(screenshotPath, screenShot.EncodeToPNG());
        SaveOutputs.Save(activeBoard.plan, screenShot.width, screenShot.height, screenShot.EncodeToPNG());
        return true;
    }
}

