using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AbstractSpell spell;

    private void OnTriggerEnter(Collider other)
    {
        MainCharacter mainCharacter = other.GetComponent<MainCharacter>();
        if (!mainCharacter) return;

        bool spellAdded = mainCharacter.GetGrimoire().AddSpell(spell);
        if (spellAdded) Destroy(gameObject);
    }
}
