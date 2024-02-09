using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardWindow : Window
{
    void Start()
    {
        closePrerequisites = new CloseBWPrerequisites();
    }

    internal class CloseBWPrerequisites : IClosePrerequisites
    {
        internal CloseBWPrerequisites()
        {
            Debug.Log("CloseBWPrerequisites");
        }

        public bool IsSatisfied()
        {
            throw new System.NotImplementedException();
        }
    }
}