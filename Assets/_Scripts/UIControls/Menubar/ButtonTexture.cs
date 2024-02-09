using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTexture : MonoBehaviour {

    SpriteState currentState;
    SpriteState lastState;
    GameObject lastTab = null;
    public void changeImage()
    {
        GameObject categoryButton = EventSystem.current.currentSelectedGameObject;
        ChangeTargetImage(categoryButton);
    }

    public void ChangeTargetImage(GameObject categoryButton)
    {

        currentState = categoryButton.GetComponent<Button>().spriteState;
        categoryButton.GetComponent<Image>().sprite = currentState.pressedSprite;
        if (lastTab != null)
        {
            lastState = lastTab.GetComponent<Button>().spriteState;
            lastTab.GetComponent<Image>().sprite = currentState.disabledSprite;
            lastState.highlightedSprite = currentState.highlightedSprite;
            lastTab.GetComponent<Button>().spriteState = lastState;
        }
        currentState.highlightedSprite = currentState.pressedSprite;
        categoryButton.GetComponent<Button>().spriteState = currentState;
        lastTab = categoryButton;
    }
    
}
