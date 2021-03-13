using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectionStatus : AbstractStatus
{
    [Header("Deflection settings")]
    [SerializeField] protected float deflectionArc = 360;

    public float GetDeflectionArc() { return deflectionArc; }

    //public override void AddStatus(Character target)
    //{
    //    //throw new System.NotImplementedException();
    //}

    //public override void CauseStatus(Character target)
    //{
    //    //throw new System.NotImplementedException();
    //}

    //public override void RemoveStatus(Character target)
    //{
    //    //throw new System.NotImplementedException();
    //}
}
