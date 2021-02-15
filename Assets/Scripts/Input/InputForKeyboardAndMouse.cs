using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputForKeyboardAndMouse : MonoBehaviour
{
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

    [Header("Modifiers")]
    [SerializeField] private KeyCode modCtrl = KeyCode.LeftControl;

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

    #region Modifiers
    public bool ModCtrl()
    {
        return Input.GetKey(modCtrl);
    }
    #endregion
}
