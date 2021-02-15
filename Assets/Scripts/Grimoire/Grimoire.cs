using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimoire : MonoBehaviour
{
    private MainCharacter _mainCharacter;

    [Header("Settings")]
    [SerializeField] private List<AbstractSpell> spellList = new List<AbstractSpell>();

    [Header("Execution")]
    [SerializeField] private AbstractSpell selectedSpell;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Character targetChar;
    [SerializeField] private bool spellStart;
    [SerializeField] private bool spellEnd;
    [SerializeField] private float currentDuration;

    public List<AbstractSpell> GetSpellList() { return spellList; }
    public bool AddSpell(AbstractSpell spell)
    {
        AbstractSpell actualSpell = null;
        foreach (AbstractSpell abSpell in spellList)
        {
            if (abSpell.CheckSame(spell))
            {
                actualSpell = abSpell;
                break;
            }
        }
        if (!actualSpell) actualSpell = Instantiate(spell, transform);

        if (!spellList.Contains(actualSpell)) spellList.Add(actualSpell);
        if (!selectedSpell) selectedSpell = actualSpell;
        return true;
    }

    public AbstractSpell GetSelectedSpell() { return selectedSpell; }

    private void Start()
    {
        _mainCharacter = GetComponentInParent<MainCharacter>();
    }

    private void Update()
    {
        if (!selectedSpell) return;
        float startDuration = selectedSpell.GetStartDuration();
        float endDuration = selectedSpell.GetEndDuration();

        if (spellStart)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= startDuration)
            {
                spellStart = false;
                spellEnd = true;
                currentDuration = 0;
                selectedSpell.Cast(_mainCharacter, targetPos, targetChar);
            }
        }
        else if (spellEnd)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= endDuration)
            {
                targetPos = Vector3.zero;
                targetChar = null;
                spellEnd = false;
                currentDuration = 0;
            }
        }
    }

    public void NextSpell()
    {
        int index = spellList.IndexOf(selectedSpell) + 1;
        if (index >= spellList.Count) index = 0;
        selectedSpell = spellList[index];
    }

    public void PreviousSpell()
    {
        int index = spellList.IndexOf(selectedSpell) - 1;
        if (index < 0) index = spellList.Count - 1;
        selectedSpell = spellList[index];
    }

    public bool StartSpell(Vector3 targetPos, Character targetChar)
    {
        if (IsCastingSpell()) return false;
        if (!selectedSpell) return false;

        if (selectedSpell.IsCastOnSelf()) targetChar = _mainCharacter;
        if (!selectedSpell.CanCast(_mainCharacter, targetPos, targetChar)) return false;

        this.targetPos = targetPos;
        this.targetChar = targetChar;
        spellStart = true;
        return true;
    }

    public bool IsCastingSpell()
    {
        return spellStart || spellEnd;
    }

    public bool InterruptSpell()
    {
        if (!InterruptSpell()) return false;
        targetPos = Vector3.zero;
        targetChar = null;
        spellStart = false;
        spellEnd = false;
        currentDuration = 0;
        return true;
    }
}
