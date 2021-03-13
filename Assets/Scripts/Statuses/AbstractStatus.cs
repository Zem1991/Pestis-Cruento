using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractStatus : MonoBehaviour
{
    [Header("Duration")]
    [SerializeField] protected bool neverExpires = false;
    [SerializeField] protected float durationMax = 1F;
    [SerializeField] protected float durationCurrent = 0F;

    public bool UpdateStatus(float durationPerFrame)
    {
        durationCurrent += durationPerFrame;
        return durationCurrent >= durationMax;
    }

    //public abstract void AddStatus(Character target);
    //public abstract void CauseStatus(Character target);
    //public abstract void RemoveStatus(Character target);
}
