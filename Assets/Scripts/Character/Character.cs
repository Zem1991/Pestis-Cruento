using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Character : MonoBehaviour, ITargetable
{
    [Header("Self references")]
    [SerializeField] private CharacterController _characterController;
    //[SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;

    [Header("Identification")]
    [SerializeField] protected string characterName = "Unknown Character";
    [SerializeField] protected Allegiance allegiance = Allegiance.ENEMY;

    [Header("Health")]
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected int currentHealth = 100;
    [SerializeField] protected int maximumHealth = 100;

    [Header("Rotation and Movement")]
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
        Gizmos.color = GizmoColors.targetablePosition;
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
        if (isDead) return true;

        isDead = CheckNoHealth();
        if (_characterController) _characterController.detectCollisions = !isDead;

        if (_animator)
        {
            _animator.SetBool("Is Dead", isDead);
            _animator.SetTrigger("Hurt");
        }
        return isDead;
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
    public void Die()
    {
        Debug.LogWarning("Die() was called for character " + characterName);
        LoseHealth(currentHealth);
    }
    #endregion

    #region Rotation and Movement
    public virtual bool CanMove()
    {
        bool checkAttack = !attack || !attack.IsAttacking();
        return checkAttack;
    }
    public void Rotation(Vector3 lookAtPosition)
    {
        Vector3 myPos = transform.position;
        lookAtPosition.y = myPos.y;
        Vector3 direction = lookAtPosition - myPos;
        direction.Normalize();
        if (direction == Vector3.zero) return;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    public void Movement(Vector3 direction)
    {
        movement = Vector3.zero;
        if (!CanMove()) return;

        if (direction.magnitude > 1) direction.Normalize();
        movement = direction * movementSpeed;
        //_characterController.SimpleMove(movement);

        if (!_animator) return;
        float rotEulerY = transform.rotation.eulerAngles.y;
        direction = Quaternion.Euler(0, -rotEulerY, 0) * direction;

        Vector3 animatorSpeed = direction;
        ////TODO: I'm not sure if these are right.
        //animatorSpeed.x = Helper.RoundToMultiple(animatorSpeed.x, 1F);
        //animatorSpeed.z = Helper.RoundToMultiple(animatorSpeed.z, 1F);

        animatorSpeed.Normalize();
        _animator.SetFloat("Speed X", animatorSpeed.x);
        _animator.SetFloat("Speed Z", animatorSpeed.z);
    }
    private void ActualMovement()
    {
        if (_characterController && _characterController.enabled)
        {
            Vector3 ccSpeed = movement + impact;
            ccSpeed *= Time.fixedDeltaTime;
            _characterController.Move(ccSpeed);
        }
        //else if (_rigidbody)
        //{
        //    Vector3 rbSpeed = movement * Time.fixedDeltaTime;
        //    _rigidbody.MovePosition(_rigidbody.position + rbSpeed);
        //}

        if (!_animator) return;
        bool animatorInMovement = movement != Vector3.zero;
        _animator.SetBool("In Movement", animatorInMovement);
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

        if (!_animator) return;
        _animator.SetTrigger("Attack");
    }
    #endregion

    #region Statuses
    public Statuses GetStatuses() { return statuses; }
    #endregion

    #region Physics
    public void ReceiveImpact(float impactForce, Vector3 impactPosition, float impactRadius)
    //public void ReceiveImpact(Vector3 impact)
    {
        if (_characterController && _characterController.enabled)
        {
            Vector3 hitPos = GetTargetablePosition();
            float dist = Vector3.Distance(hitPos, impactPosition);
            float ratio = Mathf.InverseLerp(impactRadius, 0, dist);
            Vector3 impact = (hitPos - impactPosition).normalized;
            this.impact += impact * impactForce * ratio / mass;
        }
        //else if (_rigidbody)
        //{
        //    _rigidbody.AddExplosionForce(impactForce, impactPosition, impactRadius, 0F, ForceMode.Impulse);
        //}
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
