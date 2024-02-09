using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICloseWindow
{
    IClosePrerequisites ClosePrerequisites { get; }
    void Close();
}
public interface IFullScreen
{
    bool IsFullScreen { get; set; }
}
public interface IRectTransform
{
    RectTransform RectTransform { get; }
}

public class Window : MonoBehaviour, ICloseWindow, IFullScreen, IRectTransform
{
    CloseWindow closeObject;
    protected IClosePrerequisites closePrerequisites;
    public IClosePrerequisites ClosePrerequisites
    {
        get { return closePrerequisites; }
    }
    public RectTransform RectTransform
    {
        get
        {
            return GetComponent<RectTransform>();
        }
    }
    public bool IsFullScreen { get; set; }

    public void Close()
    {
        closeObject = transform.GetComponentInChildren<CloseWindow>();
        closeObject.On(this);
    }

}
public interface IClosePrerequisites : IPrerequisites
{

}