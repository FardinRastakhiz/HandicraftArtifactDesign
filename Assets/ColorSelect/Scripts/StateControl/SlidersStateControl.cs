using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Fardin.ColorTools
{
    public class SlidersStateControl : MonoBehaviour
    {
        [SerializeField]
        List<Toggle> toggles;
        [SerializeField]
        List<GameObject> sliders;
        [SerializeField]
        List<GameObject> squares;
        [SerializeField]
        bool isSquares;
        GameObject lastObj;


        void Start()
        {
            toggles[0].isOn = true;
            lastObj = toggles[0].gameObject;
        }

        public void On_Toggle_Click()
        {
            GameObject obj = EventSystem.current.currentSelectedGameObject;
            if (!obj)
                return;
            int j = toggles.IndexOf(obj.GetComponent<Toggle>());
            if (obj == lastObj)
            {
                toggles[j].isOn = true;
                return;
            }

            lastObj = obj;
            for (int i = toggles.Count - 1; i >= 0; i--)
            {
                if (i != j)
                    toggles[i].isOn = false;
                sliders[i].SetActive(toggles[i].isOn);
            }

            if (isSquares)
                for (int i = squares.Count - 1; i >= 0; i--)
                    squares[i].SetActive(toggles[i].isOn);
        }
    }
}