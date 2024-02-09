

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public static class CreateWindow
{
    static Dictionary<string, IWindowFactory> Factories;
    public static Window CreateNewWindow(string windowType)
    {
        if (Factories==null)
        {
            Factories = new Dictionary<string, IWindowFactory>();
            var types = typeof(CreateWindow).Assembly.GetTypes();
            foreach (var t in types)
            {
                if (typeof(IWindowFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    Factories.Add(t.Name, (IWindowFactory)Activator.CreateInstance(t));
                }
            }
        }
        return Factories["" + windowType + "Factory"].Create();
    }
}


public interface IWindowFactory
{
    Window Create();
}



internal class BoardWindowFactory : IWindowFactory
{
    public Window Create()
    {
        return new GameObject().GetComponent<Window>();
    }
}

internal class InformationWindowFactory : IWindowFactory
{
    public Window Create()
    {
        Plan plan = EventSystem.current.currentSelectedGameObject.GetComponent<Plan>();
        DefaultShortcuts.allowed = false;
        GameObject informationWindow = ObjectBuilder.Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<UIController>().openWindow);
        informationWindow.name = plan.name + "_InformationWindow";
        informationWindow.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

        informationWindow.transform.parent.Find("ClickFilter").gameObject.SetActive(true);
        informationWindow.transform.Find("Save").GetComponent<Button>().interactable = false;
        if (plan.isOriginal)
            informationWindow.transform.Find("Delete").GetComponent<Button>().interactable = false;
        else
            informationWindow.transform.Find("Delete").GetComponent<Button>().interactable = true;
        informationWindow.transform.Find("InputField").GetComponent<InputField>().text = plan.description;
        informationWindow.transform.Find("Drag").Find("Text").GetComponent<Text>().text = plan.name;

        Text CategoryPresent = informationWindow.transform.Find("CategoryPresent").GetComponent<Text>();

        string category = plan.category;
        string root = plan.root;
        string sewing = plan.sewing;
        string design = plan.design;

        CategoryPresent.text = PersianCategories.GetPersian(category);
        if (design != sewing)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root) +
               " > " + PersianCategories.GetPersian(sewing) + " > " + PersianCategories.GetPersian(design);
        else if (sewing != root)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root) +
               " > " + PersianCategories.GetPersian(sewing);
        else if (root != category)
            CategoryPresent.text = PersianCategories.GetPersian(category) + " > " + PersianCategories.GetPersian(root);
        else
            CategoryPresent.text = PersianCategories.GetPersian(category);

        return informationWindow.GetComponent<Window>();
    }
}

