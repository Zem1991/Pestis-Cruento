using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEffect : MonoBehaviour
{
    public abstract bool ExecuteEffect(Character caster, Vector3 targetPos, Character targetChar);
}
