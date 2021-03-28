using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEffect : AbstractEffect
{
    public override bool ExecuteEffect(Character caster, Vector3 targetPos, GameObject targetObj)
    {
        Character targetChar = targetObj.GetComponent<Character>();
        if (!targetChar) return false;
        //TODO: This only works as an player character spell. If I want enemies taking control of stuff, this will need to change.
        Player localPlayer = Player.Instance;
        return localPlayer.ChangeCharacter(targetChar);
    }
}
