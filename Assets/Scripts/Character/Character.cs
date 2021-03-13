using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Character : MonoBehaviour, ITargetable
{
    [Header("Self references")]
    [SerializeField] private CharacterController _characterController;

    [Header("Identification")]
    [SerializeField] protected string characterName = "Unknown Character";
    [SerializeField] protected Allegiance allegiance = Allegiance.ENEMY;

    [Header("Health")]
    [SerializeField] protected int currentHealth = 100;
    [SerializeField] protected int maximumHealth = 100;

    [Header("Movement")]
    [SerializeField] protected float movementSpeed = 8F;
    
    [Header("Combat")]
    [SerializeField] protected Attack attack;
    [SerializeField] protected float projSpawnHeight = 1.5F;
    [SerializeField] protected float projSpawnDistance = 0.5F;

    [Header("Statuses")]
    [SerializeField] protected Statuses statuses;

    [Header("Physics")]
    [SerializeField] protected Vector3 movement;
    [SerializeField] protected Vector3 impact;
    [SerializeField] protected float mass = 100F;

    //public Collider GetCollider() { return _characterController; }

    private void OnDrawGizmos()
    {
        Vector3 position = GetTargetablePosition();
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(position, 0.25F);
    }

    protected virtual void Update()
    {
        //UpdateStatusList();
    }

    protected virtual void FixedUpdate()
    {
        AbsorbImpact();
        ActualMovement();
    }

    #region Identification
    public string GetCharacterName() { return characterName; }
    public Allegiance GetAllegiance() { return allegiance; }
    public bool CheckOpponent(Character other)
    {
        if (!other) return false;
        Allegiance otherAllegiance = other.GetAllegiance();
        return allegiance.CheckOpponent(otherAllegiance);
    }
    #endregion

    #region Health
    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaximumHealth() { return maximumHealth; }
    public bool LoseHealth(int amount)
    {
        if (amount < 0) amount = 0;
        currentHealth -= amount;
        //TODO: will use negative health to check for gibbing
        //if (currentHealth < 0) currentHealth = 0;
        return CheckNoHealth();
    }
    public bool CheckNoHealth() { return currentHealth <= 0; }
    public bool GainHealth(int amount)
    {
        if (amount < 0) amount = 0;
        currentHealth += amount;
        if (currentHealth > maximumHealth) currentHealth = maximumHealth;
        return CheckFullHealth();
    }
    public bool CheckFullHealth() { return currentHealth >= maximumHealth; }
    #endregion

    #region Movement
    public virtual bool CanMove()
    {
        bool checkAttack = !attack || !attack.IsAttacking();
        return checkAttack;
    }
    public void Movement(Vector3 direction)
    {
        movement = Vector3.zero;
        if (!CanMove()) return;

        movement = direction.normalized;
        movement *= movementSpeed;
        //_characterController.SimpleMove(movement);

        if (movement != Vector3.zero)
        {
            Vector3 lookTarget = transform.position + movement;
            transform.LookAt(lookTarget);
        }
    }
    private void ActualMovement()
    {
        Vector3 speed = movement + impact;
        speed *= Time.fixedDeltaTime;
        _characterController.Move(speed);
    }
    #endregion

    #region Combat
    public bool CanAttack()
    {
        bool checkAttack = attack && !attack.IsAttacking();
        return checkAttack;
    }
    public void Attack()
    {
        if (!CanAttack()) return;
        attack.StartAttack();
    }
    #endregion

    #region Statuses
    public Statuses GetStatuses() { return statuses; }
    #endregion

    #region Physics
    public void ReceiveImpact(Vector3 impact)
    {
        this.impact += impact / mass;
    }
    private void AbsorbImpact()
    {
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.fixedDeltaTime);
        if (impact.magnitude < 0.5F) impact = Vector3.zero;
    }
    #endregion

    #region Spawning
    public void SpawnProjectile(Projectile projectile, GameObject targetObj)
    {
        Vector3 pos = transform.position;
        pos += transform.forward * projSpawnDistance;
        pos.y += projSpawnHeight;
        Quaternion rot = transform.rotation;
        Projectile proj = Instantiate(projectile, pos, rot);
        proj.Initialize(gameObject, targetObj);
    }
    public void SpawnCharacter(Character character)
    {
        Vector3 pos = transform.position;
        pos += transform.forward * projSpawnDistance * 2F;
        Quaternion rot = transform.rotation;
        Character chara = Instantiate(character, pos, rot);
        //chara.Initialize(this, null);
    }
    #endregion

    #region ITargetable
    public Vector3 GetTargetablePosition()
    {
        return transform.position + _characterController.center;
    }
    #endregion
}
