using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputForKeyboardAndMouse : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField] private KeyCode modCtrl = KeyCode.LeftControl;

    [Header("Movement")]
    [SerializeField] private string movementAxisH = "Horizontal";
    [SerializeField] private string movementAxisV = "Vertical";

    [Header("Combat")]
    [SerializeField] private KeyCode attack = KeyCode.Mouse0;

    [Header("Inventory")]
    [SerializeField] private KeyCode useItem = KeyCode.F;
    [SerializeField] private KeyCode useItemModCtrl = KeyCode.Mouse0;
    [SerializeField] private KeyCode prevItem = KeyCode.Z;
    [SerializeField] private KeyCode nextItem = KeyCode.X;

    [Header("Grimoire")]
    [SerializeField] private KeyCode castSpell = KeyCode.R;
    [SerializeField] private KeyCode castSpellModCtrl = KeyCode.Mouse1;
    [SerializeField] private KeyCode prevSpell = KeyCode.C;
    [SerializeField] private KeyCode nextSpell = KeyCode.V;

    [Header("Interaction")]
    [SerializeField] private KeyCode interaction = KeyCode.E;

    #region Modifiers
    public bool ModCtrl()
    {
        return Input.GetKey(modCtrl);
    }
    #endregion

    #region Movement
    public Vector3 Movement()
    {
        Vector3 result = new Vector3();
        result.x = Input.GetAxisRaw(movementAxisH);
        result.z = Input.GetAxisRaw(movementAxisV);
        result = result.normalized;
        return result;
    }
    #endregion

    #region Combat
    public bool Attack()
    {
        return !ModCtrl() && Input.GetKey(attack);
        //return Input.GetKeyDown(attack);
    }
    #endregion

    #region Inventory
    public bool UseItem()
    {
        bool directKey = Input.GetKeyDown(useItem);
        bool modCtrlKey = ModCtrl() && Input.GetKeyDown(useItemModCtrl);
        return directKey || modCtrlKey;
    }
    public bool PreviousItem()
    {
        return Input.GetKeyDown(prevItem);
    }
    public bool NextItem()
    {
        return Input.GetKeyDown(nextItem);
    }
    #endregion

    #region Grimoire
    public bool CastSpell()
    {
        bool directKey = Input.GetKeyDown(castSpell);
        bool modCtrlKey = ModCtrl() && Input.GetKeyDown(castSpellModCtrl);
        return directKey || modCtrlKey;
    }
    public bool PreviousSpell()
    {
        return Input.GetKeyDown(prevSpell);
    }
    public bool NextSpell()
    {
        return Input.GetKeyDown(nextSpell);
    }
    #endregion

    #region Interaction
    public bool Interaction()
    {
        return Input.GetKeyDown(interaction);
    }
    #endregion
}
