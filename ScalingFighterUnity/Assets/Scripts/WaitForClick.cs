using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForClick : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return !Input.GetMouseButtonDown(0);
        }
    }

    public WaitForClick()
    {
        
    }
}