using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanWindow : Window
{
    void Start()
    {
        closePrerequisites = new CloseCPWPrerequisites();
    }
    internal class CloseCPWPrerequisites : IClosePrerequisites
    {
        internal CloseCPWPrerequisites()
        {
            Debug.Log("CloseCPWPrerequisites");
        }

        public bool IsSatisfied()
        {
            throw new System.NotImplementedException();
        }
    }
}