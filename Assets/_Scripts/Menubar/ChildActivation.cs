using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChildActivation : MonoBehaviour
{
    public static void SetActive(Transform parentTransform, string childName)
    {
        try
        {
            parentTransform.Find(childName).gameObject.SetActive(true);
        }
        catch (System.NullReferenceException nex)
        {
            Debug.Log("" + nex.Message + "  . Can't fine the menu with argument 'name': " + childName + ".");
            throw;
        }
    }

    public static void DeActivate(Transform parentTransform, string childName)
    {
        try
        {
            parentTransform.Find(childName).gameObject.SetActive(false);
        }
        catch (System.NullReferenceException nex)
        {
            Debug.Log("" + nex.Message + "  . Can't fine the child with argument 'name': " + childName + ".");
            throw;
        }
    }
}
