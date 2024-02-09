///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <InformationWindow>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InformationWindow : Window
{
    private Plan targetPlan;
    private InputField field;
    private Button saveButton;
    private Text CategoryPresent;
    private string category;
    private string root;
    private string sewing;
    private string design;
    public Plan TargetPlan { get { return targetPlan; } }
    public InputField Field { get { return field; } }
    public Button SaveButton { get { return saveButton; } }
    public string Category { get { return category; } }

    void Start()
    {
        closePrerequisites = new CloseIWPrerequisites(this);
        field = transform.Find("InputField").GetComponent<InputField>();
        saveButton = transform.Find("Save").GetComponent<Button>();
        targetPlan = GenPlans.GetPlan(transform.name.Replace("_InformationWindow", ""));
        category = targetPlan.category;
        root = targetPlan.root;
        sewing = targetPlan.sewing;
        design = targetPlan.design;
        SetRectTransform();
    }
    void SetRectTransform()
    {
        RectTransform rectTra = GetComponent<RectTransform>();
        rectTra.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 440);
        rectTra.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 270);
        rectTra.anchorMin = new Vector2(0.5f, 0.5f);
        rectTra.anchorMax = new Vector2(0.5f, 0.5f);
        rectTra.localPosition = Vector3.zero;
    }


    public void Save_Changes()
    {
        targetPlan.description = field.text;
        targetPlan.category = category;
        targetPlan.root = root;
        targetPlan.sewing = sewing;
        targetPlan.design = design;
        saveButton.interactable = false;
        GenPlans.SavePlan(targetPlan);
        Categorization.DrawCategory(GenPlans.plans, category);
        //save
    }


    public void On_TextField_Change()
    {
        if (field.text != targetPlan.description || category != targetPlan.category)
        {
            saveButton.interactable = true;
        }
        else
        {
            saveButton.interactable = false;
        }
    }
    
    internal class CloseIWPrerequisites : IClosePrerequisites
    {
        Window window;
        internal CloseIWPrerequisites(Window window)
        {
            this.window = window;
        }

        public bool IsSatisfied()
        {
            DefaultShortcuts.allowed = true;
            window.transform.parent.Find("ClickFilter").gameObject.SetActive(false);
            return true;
        }
    }
    public void DeletePlan()
    {
        SaveOutputs.DeleteRow(targetPlan.id);

        SaveBoardPlan.StartQuery();
        SaveBoardPlan.DeleteTable(targetPlan.name + "_parts");
        SaveBoardPlan.DeleteTable(targetPlan.name + "_primitives");
        SaveBoardPlan.DeleteTable(targetPlan.name + "_backgrounds");
        SaveBoardPlan.deleteRow("Plans", targetPlan.id);
        SaveBoardPlan.RunQuery();
        //ColorPicker.useExternalDrawer = false;
        string imagePath = Application.dataPath + "/Resources/" + targetPlan.sourceImage + ".png";
        if (System.IO.File.Exists(imagePath))
            System.IO.File.Delete(imagePath);
        GenPlans.plans.Remove(targetPlan);
        Destroy(targetPlan.gameObject);
        this.Close();
    }
}
