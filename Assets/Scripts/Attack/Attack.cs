using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Character _character;

    [Header("Settings")]
    [SerializeField] private float hitPivot = 0.75F;
    [SerializeField] private float hitRadius = 1;
    [SerializeField] private float startDuration = 0.2F;
    [SerializeField] private float hitDuration = 0.1F;
    [SerializeField] private float endDuration = 0.2F;
    [SerializeField] private int attackDamage = 10;

    [Header("Execution")]
    [SerializeField] private Vector3 attackPosition;
    [SerializeField] private bool attackStart;
    [SerializeField] private bool attackHit;
    [SerializeField] private bool attackEnd;
    [SerializeField] private float currentDuration;
    [SerializeField] private List<Character> charactersHit = new List<Character>();

    private void OnDrawGizmos()
    {
        Color color = IsAttacking() ? GizmoColors.attackActive : GizmoColors.attackInactive;
        if (attackHit) color = GizmoColors.attackHit;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(attackPosition, hitRadius);
    }

    private void Start()
    {
        _character = GetComponentInParent<Character>();
    }

    private void Update()
    {
        attackPosition = transform.position + (transform.forward * hitPivot);
        if (attackStart)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= startDuration)
            {
                attackStart = false;
                attackHit = true;
                currentDuration = 0;
            }
        }
        else if (attackHit)
        {
            CauseDamage();
            currentDuration += Time.deltaTime;
            if (currentDuration >= hitDuration)
            {
                attackHit = false;
                attackEnd = true;
                currentDuration = 0;
                charactersHit.Clear();
            }
        }
        else if (attackEnd)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= endDuration)
            {
                attackEnd = false;
                currentDuration = 0;
            }
        }
    }

    public bool StartAttack()
    {
        if (IsAttacking()) return false;
        attackStart = true;
        return true;
    }

    public bool IsAttacking()
    {
        return attackStart || attackHit || attackEnd;
    }

    public bool InterruptAttack()
    {
        if (!IsAttacking()) return false;
        attackStart = false;
        attackHit = false;
        attackEnd = false;
        currentDuration = 0;
        return true;
    }

    public void CauseDamage()
    {
        Collider[] candidates = Physics.OverlapSphere(attackPosition, hitRadius);
        foreach(Collider item in candidates)
        {
            GameObject gObj = item.gameObject;
            if (gObj == _character.gameObject) continue;

            Character possibleTarget = item.GetComponent<Character>();
            if (!possibleTarget) continue;

            if (charactersHit.Contains(possibleTarget)) continue;
            charactersHit.Add(possibleTarget);
            possibleTarget.LoseHealth(attackDamage);
        }
    }
}
