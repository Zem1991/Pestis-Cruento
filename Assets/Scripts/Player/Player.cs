using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractSingleton<Player>
{
    [Header("Self references")]
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private GameObject interactionProjector;
    [SerializeField] private GameObject cursorProjector;

    [Header("Other references")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private UIHandler uiHandler;

    [Header("Character")]
    [SerializeField] private MainCharacter mainCharacter;
    [SerializeField] private Character currentCharacter;

    [Header("Interaction target")]
    [SerializeField] private float interactionRange = 1F;
    [SerializeField] private GameObject interactionObj;

    [Header("Cursor target")]
    [SerializeField] private GameObject cursorTargetObj;

    private void Start()
    {
        currentCharacter = mainCharacter;
    }

    private void Update()
    {
        Interaction();

        Inventory();
        Grimoire();
        Combat();

        Rotation();
        Movement();
        Camera();

        SeekInteractionTarget();
        SeekCursorTarget();

        CallManualUIUpdate();
    }

    #region Update methods
    private void Interaction()
    {
        bool interaction = inputHandler.Interaction();
        if (!interaction || interactionObj == null) return;

        IInteractionTarget interactionTarget = interactionObj.GetComponent<IInteractionTarget>();
        if (interactionTarget != null && interactionTarget.CanInteract()) interactionTarget.Interact();
    }

    private void Inventory()
    {
        bool nextItem = inputHandler.NextItem();
        bool prevItem = inputHandler.PreviousItem();
        bool useItem = inputHandler.UseItem();

        if (nextItem || prevItem)
        {
            if (!IsControllingMainCharacter()) return;

            if (nextItem) mainCharacter.NextItem();
            else mainCharacter.PreviousItem();

            Inventory inventory = mainCharacter.GetInventory();
            List<AbstractItem> itemList = inventory.GetItemList();
            AbstractItem selectedItem = inventory.GetSelectedItem();
            uiHandler.ManualUpdateSelection(itemList, selectedItem);
        }
        else if (useItem)
        {
            //If we are controlling something different, we instead change it back to the main character.
            if (ResetCharacter()) return;

            InputCursor inputCursor = inputHandler.GetInputCursor();
            Vector3 targetPos = inputCursor.GetCurrentPosScene();
            GameObject targetObj = inputCursor.GetFoundTargetable();
            mainCharacter.UseItem(targetPos, targetObj);
        }
    }

    private void Grimoire()
    {
        bool nextSpell = inputHandler.NextSpell();
        bool prevSpell = inputHandler.PreviousSpell();
        bool castSpell = inputHandler.CastSpell();

        if (nextSpell || prevSpell)
        {
            if (!IsControllingMainCharacter()) return;

            if (nextSpell) mainCharacter.NextSpell();
            else mainCharacter.PreviousSpell();

            Grimoire grimoire = mainCharacter.GetGrimoire();
            List<AbstractSpell> spellList = grimoire.GetSpellList();
            AbstractSpell selectedSpell = grimoire.GetSelectedSpell();
            uiHandler.ManualUpdateSelection(spellList, selectedSpell);
        }
        else if (castSpell)
        {
            //If we are controlling something different, we instead change it back to the main character.
            if (ResetCharacter()) return;

            InputCursor inputCursor = inputHandler.GetInputCursor();
            Vector3 targetPos = inputCursor.GetCurrentPosScene();
            GameObject targetObj = inputCursor.GetFoundTargetable();
            mainCharacter.CastSpell(targetPos, targetObj);
        }
    }

    private void Combat()
    {
        bool attack = inputHandler.Attack();
        if (attack) currentCharacter.Attack();
    }

    private void Rotation()
    {
        InputCursor inputCursor = inputHandler.GetInputCursor();
        Plane xzPlane = new Plane(transform.up, transform.position.y);
        Ray ray = inputCursor.GetScreenCursorRay();

        //TODO: have something more ready to use inside InputCursor/InputHandler.
        bool success = xzPlane.Raycast(ray, out float enter);
        if (success)
        {
            Vector3 lookPos = ray.GetPoint(enter);
            currentCharacter.SetRotation(lookPos);
        }
    }

    private void Movement()
    {
        Vector3 cameraRotationEuler = playerCamera.transform.rotation.eulerAngles;
        Quaternion dirAdjustmentRot = Quaternion.Euler(0, cameraRotationEuler.y, 0);
        
        Vector3 direction = inputHandler.Movement();
        direction = dirAdjustmentRot * direction;

        currentCharacter.SetMovement(direction);
    }

    private void Camera()
    {
        if (currentCharacter) transform.position = currentCharacter.transform.position;
    }

    private void SeekInteractionTarget()
    {
        interactionObj = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (Collider forColl in colliders)
        {
            IInteractionTarget forInteractionTarget = forColl.GetComponent<IInteractionTarget>();
            if (forInteractionTarget == null) continue;
            interactionObj = forColl.gameObject;
            break;
        }

        if (interactionObj != null)
        {
            interactionProjector.transform.position = interactionObj.transform.position;
            interactionProjector.gameObject.SetActive(true);
        }
        else
        {
            interactionProjector.gameObject.SetActive(false);
            interactionProjector.transform.position = transform.position;
        }
    }

    private void SeekCursorTarget()
    {
        InputCursor inputCursor = inputHandler.GetInputCursor();
        GameObject foundTargetable = inputCursor.GetFoundTargetable();

        if (foundTargetable == mainCharacter) foundTargetable = null;
        if (foundTargetable == currentCharacter) foundTargetable = null;
        cursorTargetObj = foundTargetable;

        if (cursorTargetObj != null)
        {
            cursorProjector.transform.position = cursorTargetObj.transform.position;
            cursorProjector.gameObject.SetActive(true);
        }
        else
        {
            cursorProjector.gameObject.SetActive(false);
            cursorProjector.transform.position = transform.position;
        }
    }

    private void CallManualUIUpdate()
    {
        MainCharacter mainChar = IsControllingMainCharacter() ? mainCharacter : null;
        uiHandler.ManualUpdatePlayer(mainChar);
    }
    #endregion

    #region Character
    public bool IsControllingMainCharacter() { return currentCharacter == mainCharacter; }
    public bool ChangeCharacter(Character target)
    {
        currentCharacter = target;
        return true;
    }
    public bool ResetCharacter()
    {
        if (!IsControllingMainCharacter())
        {
            currentCharacter.Die();
            currentCharacter = mainCharacter;
            return true;
        }
        return false;
    }
    #endregion
}
