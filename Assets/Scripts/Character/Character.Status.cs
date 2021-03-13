using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Character : MonoBehaviour, ITargetable
{
    [Header("Status: Deflection")]
    [SerializeField] protected float deflectionArc = 0;

    #region Status: Deflection
    public float GetDeflectionArc() { return deflectionArc; }
    public void SetDeflectionArc(float deflectionArc) { this.deflectionArc = deflectionArc; }
    #endregion
}
