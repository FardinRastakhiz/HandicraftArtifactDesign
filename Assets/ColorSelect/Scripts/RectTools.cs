///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <RectTools>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.Linq;
namespace Fardin.UITools
{
    public class RectTools
    {

        public static Rect ToScreen(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            return new Rect(transform.position.x - (size.x * 0.5f), transform.position.y - (size.y * 0.5f), size.x, size.y);
        }

        public static void Copy(RectTransform origin, RectTransform copy)
        {
            copy.offsetMin = origin.offsetMin;
            copy.offsetMax = origin.offsetMax;
            copy.anchorMax = origin.anchorMax;
            copy.anchorMin = origin.anchorMin;
            copy.localPosition = origin.localPosition;
            copy.localRotation = origin.localRotation;
            copy.localScale = origin.localScale;
            copy.anchoredPosition = origin.anchoredPosition;
        }

        public static Vector3 SetInRect(RectTransform transform, Vector3 position)
        {
            Rect screenRect = ToScreen(transform);
            Vector3 inRectPos = position;
            inRectPos.x = inRectPos.x < screenRect.x ? screenRect.x : inRectPos.x;
            inRectPos.x = inRectPos.x > screenRect.x + screenRect.width ? screenRect.x + screenRect.width : inRectPos.x;
            inRectPos.y = inRectPos.y < screenRect.y ? screenRect.y : inRectPos.y;
            inRectPos.y = inRectPos.y > screenRect.y + screenRect.height ? screenRect.y + screenRect.height : inRectPos.y;
            return inRectPos;
        }


    }
}