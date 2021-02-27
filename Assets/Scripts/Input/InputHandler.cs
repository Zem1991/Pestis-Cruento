using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private InputCursor inputCursor;
    [SerializeField] private InputForKeyboardAndMouse inputKBM;

    private void Update()
    {
        inputCursor.ReadCursor(Camera.main);
    }

    #region Cursor
    public InputCursor GetInputCursor() { return inputCursor; }
    #endregion

    #region Modifiers
    public bool ModCtrl()
    {
        return inputKBM.ModCtrl();
    }
    #endregion

    #region Movement
    public Vector3 Movement()
    {
        return inputKBM.Movement();
    }
    #endregion

    #region Combat
    public bool Attack()
    {
        return inputKBM.Attack();
    }
    #endregion

    #region Inventory
    public bool UseItem()
    {
        return inputKBM.UseItem();
    }
    public bool PreviousItem()
    {
        return inputKBM.PreviousItem();
    }
    public bool NextItem()
    {
        return inputKBM.NextItem();
    }
    #endregion

    #region Grimoire
    public bool CastSpell()
    {
        return inputKBM.CastSpell();
    }
    public bool PreviousSpell()
    {
        return inputKBM.PreviousSpell();
    }
    public bool NextSpell()
    {
        return inputKBM.NextSpell();
    }
    #endregion

    #region Interaction
    public bool Interaction()
    {
        return inputKBM.Interaction();
    }
    #endregion
}
