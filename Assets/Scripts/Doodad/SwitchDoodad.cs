using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoodad : Doodad, ITargetable, IInteractionTarget, ITelekinesisTarget
{
    [Header("Animator")]
    [SerializeField] protected Animator animator;

    [Header("Targetable")]
    [SerializeField] protected Vector3 targetableOffset;

    [Header("Propagation")]
    [SerializeField] protected List<SwitchDoodad> propagtionList = new List<SwitchDoodad>();

    private void OnDrawGizmos()
    {
        Vector3 position = GetTargetablePosition();
        Gizmos.color = GizmoColors.targetablePosition;
        Gizmos.DrawWireSphere(position, 0.25F);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    #region ITargetable
    public Vector3 GetTargetablePosition()
    {
        return transform.position + targetableOffset;
    }
    #endregion

    #region IInteractionTarget
    public bool CanInteract()
    {
        return !animator.IsInTransition(0);
    }
    public bool Interact()
    {
        if (!CanInteract()) return false;
        Debug.Log(gameObject.name + " ToogleSwitchState()");
        
        string name = "On";
        bool value = animator.GetBool(name);
        animator.SetBool(name, !value);

        foreach (SwitchDoodad forSD in propagtionList)
        {
            forSD.Interact();
        }
        return true;
    }
    public bool CanPropagate()
    {
        throw new System.NotImplementedException();
    }
    public bool Propagate()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region ITelekinesisTarget
    public bool ApplyTelekinesis()
    {
        return Interact();
    }
    #endregion
}
