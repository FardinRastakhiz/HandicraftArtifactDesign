using UnityEngine;
using System.Collections;
using System;
namespace Fardin.ColorTools
{
public class ColorTerminal : MonoBehaviour {

    public ColorForm colorForm;
    OnChangeColorHandler changeColorHandler;
    public event EventHandler<OnChangeColorHandler> changedColor;
    public Color starterColor = Color.yellow;

    [SerializeField]
    ColorPick colorPick;
    public void SetColorForm(Color color)
    {
        FillColorForm.ByRGBA(color, colorForm);
    }
    void Start()
    {
        colorForm.Initialize(starterColor);
        changeColorHandler = new OnChangeColorHandler();
        changeColorHandler.form = colorForm;
        colorPick.OnPickColor += OnPickColorChange;
    }

    void FixedUpdate()
    {
        if (colorForm.isChanged)
        {
            changedColor(this, changeColorHandler);
            colorForm.isChanged = false;
        }
    }

    void OnPickColorChange(object o, OnPickColorHandler e)
    {
        FillColorForm.ByRGBA(e.color, colorForm);
    }


    public bool methodRegistered(EventHandler<OnChangeColorHandler> method)
    {
        if (changedColor == null)
            return false;
        Delegate[] invocationList = changedColor.GetInvocationList();
        for (int i = invocationList.Length-1; i >=0; i--)
        {
            if (invocationList[i].Equals(method))
            {
                return true;
            }
        }
        return false;
    }
}

public class OnChangeColorHandler : EventArgs
{
    public ColorForm form;
}

}