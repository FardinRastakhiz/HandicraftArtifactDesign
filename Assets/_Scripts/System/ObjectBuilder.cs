using UnityEngine;

public class ObjectBuilder : MonoBehaviour {
    public static GameObject InstantiateObject(GameObject targetObject)
    {
        return Instantiate(targetObject);
    }
}
