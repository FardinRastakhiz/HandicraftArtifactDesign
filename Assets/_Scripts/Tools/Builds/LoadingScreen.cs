///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <LoadingScreen>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>


using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public RectTransform rotator;
    Vector3 angles;
	// Use this for initialization
	void Start () {
        angles = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        angles.z -= Time.deltaTime*200;
        rotator.localEulerAngles = angles;
	}
}
