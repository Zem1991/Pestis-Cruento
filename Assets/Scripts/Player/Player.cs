using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private PlayerCamera playerCamera;

    [Header("Other references")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private UIHandler uiHandler;

    [Header("Character")]
    [SerializeField] private MainCharacter mainCharacter;
    [SerializeField] private Character currentCharacter;

    private void Start()
    {
        currentCharacter = mainCharacter;
    }

    private void Update()
    {
        Inventory();
        Grimoire();
        Combat();
        Movement();
        Camera();

        MainCharacter mainChar = mainCharacter == currentCharacter ? mainCharacter : null;
        uiHandler.ManualUpdatePlayer(mainChar);
    }

    private void Inventory()
    {
        if (currentCharacter != mainCharacter) return;

        bool nextItem = inputHandler.NextItem();
        bool prevItem = inputHandler.PreviousItem();
        bool useItem = inputHandler.UseItem();

        if (nextItem || prevItem)
        {
            if (nextItem) mainCharacter.NextItem();
            else mainCharacter.PreviousItem();

            Inventory inventory = mainCharacter.GetInventory();
            List<AbstractItem> itemList = inventory.GetItemList();
            AbstractItem selectedItem = inventory.GetSelectedItem();
            uiHandler.ManualUpdateSelection(itemList, selectedItem);
        }
        else if (useItem)
        {
            InputCursor inputCursor = inputHandler.GetInputCursor();
            Vector3 targetPos = inputCursor.GetCurrentPosScene();
            Character targetChar = inputCursor.GetFoundCharacter();
            mainCharacter.UseItem(targetPos, targetChar);
        }
    }

    private void Grimoire()
    {
        if (currentCharacter != mainCharacter) return;

        bool nextSpell = inputHandler.NextSpell();
        bool prevSpell = inputHandler.PreviousSpell();
        bool castSpell = inputHandler.CastSpell();

        if (nextSpell || prevSpell)
        {
            if (nextSpell) mainCharacter.NextSpell();
            else mainCharacter.PreviousSpell();

            Grimoire grimoire = mainCharacter.GetGrimoire();
            List<AbstractSpell> spellList = grimoire.GetSpellList();
            AbstractSpell selectedSpell = grimoire.GetSelectedSpell();
            uiHandler.ManualUpdateSelection(spellList, selectedSpell);
        }
        else if (castSpell)
        {
            InputCursor inputCursor = inputHandler.GetInputCursor();
            Vector3 targetPos = inputCursor.GetCurrentPosScene();
            Character targetChar = inputCursor.GetFoundCharacter();
            mainCharacter.CastSpell(targetPos, targetChar);
        }
    }

    private void Combat()
    {
        bool attack = inputHandler.Attack();
        if (attack) currentCharacter.Attack();
    }

    private void Movement()
    {
        Vector3 cameraRotationEuler = playerCamera.transform.rotation.eulerAngles;
        Quaternion rotationAdjustment = Quaternion.Euler(0, cameraRotationEuler.y, 0);

        Vector3 direction = inputHandler.Movement();
        direction = rotationAdjustment * direction;
        currentCharacter.Movement(direction);
    }
    
    private void Camera()
    {
        if (currentCharacter) transform.position = currentCharacter.transform.position;
    }
}
