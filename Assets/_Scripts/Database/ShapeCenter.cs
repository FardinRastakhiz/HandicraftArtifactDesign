///<symmary>
///-----------------------------------------------------------------
///   Namespace:      <Class Namespace>
///   Class:          <ShapeCenter>
///   Description:    <>
///   Author:         <Fardin Rastakhiz>                    Date: <2018/10>
///   Notes:          <Notes>
///   Revision History:
///   Name:          Date:        Description:
///--------- --------------------------------------------------------
///</symmary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShapeCenter :MonoBehaviour{
    [SerializeField]
    Sprite[] _sourceShapes;
    [SerializeField]
    backgroundImage[] _backgrounds;
    [SerializeField]
    Sprite[] _boardCanvas;
    [SerializeField]
    GameObject _borderControl;
    [SerializeField]
    Sprite _border;
    [SerializeField]
    GameObject _areaShape;
    [SerializeField]
    GameObject _moveArea;
    [SerializeField]
    GameObject _rotateArea;
    [SerializeField]
    GameObject _scaleArea;
    [SerializeField]
    GameObject _planSub;
    [SerializeField]
    Sprite _planBackground;

    public static Sprite[] sourceShapes;
    public static backgroundImage[] backgrounds;
    public static Sprite[] boardCanvas;
    public static GameObject borderControl;
    public static Sprite border;
    public static GameObject areaShape;
    public static GameObject moveArea;
    public static GameObject rotateArea;
    public static GameObject scaleArea;
    public static GameObject planSub;
    public static Sprite planBackground;

    [System.Serializable]
    public class backgroundImage
    {
        public Sprite image;
        public UnityEngine.UI.Image.Type type;
    }
    void Awake()
    {
        sourceShapes = _sourceShapes;
        backgrounds = _backgrounds;
        boardCanvas = _boardCanvas;
        borderControl = _borderControl;
        border = _border;
        areaShape = _areaShape;
        moveArea = _moveArea;
        rotateArea = _rotateArea;
        scaleArea = _scaleArea;
        planSub = _planSub;
        planBackground = _planBackground;
    }
}
