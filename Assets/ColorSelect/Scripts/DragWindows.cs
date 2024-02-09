using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fardin.UITools
{
    public class DragWindows : MonoBehaviour
    {

        RectTransform rectTra;
        Vector3 mouseStart;
        Vector3 rectStart;
        [SerializeField]
        RectTransform frameWork;
        void Start()
        {
            if (!frameWork)
            {
                frameWork = transform.parent.GetComponent<RectTransform>();
            }
        }
        public void On_Begin_Drag()
        {
            rectTra = gameObject.GetComponent<RectTransform>();
            mouseStart = Input.mousePosition;
            rectStart = rectTra.position;
        }
        public void On_Drag()
        {
            var WorkFrame = RectTools.ToScreen(frameWork);
            rectTra.position = WorkFrame.Contains(Input.mousePosition) ? rectStart + Input.mousePosition - mouseStart : rectTra.position;
        }

    }
}