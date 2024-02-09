///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <CreateNewTab>
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

public class CreateNewTab : MonoBehaviour {

	string categoryName;
	InputField nameField;
	Button createButton;

	void Start () {
		createButton = transform.Find("Create").GetComponent<Button>();
		nameField = transform.Find("InputField").GetComponent<InputField>();
	}

    public void On_Cancel()
    {
        Destroy(this.gameObject);
    }

    public void On_Create()
    {
		Categorization.AddTab(categoryName);
        Destroy(this.gameObject);
    }

	public void On_Name_Change()
	{
		categoryName = nameField.text;
		if (categoryName.Length > 0)
		{
			if (Categorization.categories.Contains(categoryName)) {
				createButton.interactable = false;
				return;
			}
			createButton.interactable = true;
		}
		else
		{
			createButton.interactable = false;
		}
	}
}
