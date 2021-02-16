using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoodad : Doodad
{
    [Header("Switch")]
    [SerializeField] protected bool switchState = false;

    #region Switch
    public bool GetSwitchState() { return switchState; }
    public bool ToogleSwitchState()
    {
        switchState = !switchState;
        //Returns true because you did change the state.
        return true;
    }
    #endregion
}
