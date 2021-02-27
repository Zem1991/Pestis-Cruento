using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] protected float speed;
    [SerializeField] protected float homingSpeed;
    [SerializeField] protected float durationMax;
    [SerializeField] protected AbstractEffect effect;

    [Header("Initialization")]
    [SerializeField] protected Character owner;
    [SerializeField] protected GameObject homingTarget;

    [Header("Duration")]
    [SerializeField] protected float durationCurrent;

    public void Initialize(Character owner, GameObject homingTarget)
    {
        this.owner = owner;
        this.homingTarget = homingTarget;

        if (homingTarget)
        {
            transform.LookAt(homingTarget.transform);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (HasGuidance())
        {
            Color homingColor = Color.red;
            homingColor.b = 0.5F;
            Vector3 position = homingTarget.transform.position;
            Gizmos.color = homingColor;
            Gizmos.DrawLine(transform.position, position);
        }
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(_collider, owner.GetCollider());
    }

    protected virtual void Update()
    {
        Guidance();
        Duration();
    }

    private void FixedUpdate()
    {
        ActualMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 point = contact.point;

        GameObject targetObj = collision.gameObject;
        effect.ExecuteEffect(owner, point, targetObj);
        //_rigidbody.velocity = Vector3.zero;
        Destroy(gameObject);
    }

    protected bool CheckValidHomingTarget(GameObject homingTarget)
    {
        Character chara = homingTarget.GetComponent<Character>();
        bool charaIsNotOwner = chara != owner;
        bool charaIsOpponent = owner.CheckOpponent(chara);
        bool isCharacter = chara && charaIsNotOwner && charaIsOpponent;

        TelekinesisEffect tkEffect = effect as TelekinesisEffect;
        ITelekinesisTarget tkTarget = homingTarget.GetComponent<ITelekinesisTarget>();
        bool isTkTarget = tkEffect && tkTarget != null;

        return isCharacter || isTkTarget;
    }
    
    private bool HasGuidance()
    {
        return homingSpeed > 0 && homingTarget != null;
    }
    
    private void Guidance()
    {
        if (HasGuidance())
        {
            Vector3 targetPos = homingTarget.transform.position;
            float step = homingSpeed * Time.deltaTime;

            Character homingCharacter = homingTarget.GetComponent<Character>();
            if (homingCharacter) targetPos += homingCharacter.GetTargetablePosition();

            Vector3 direction = targetPos - transform.position;
            Vector3 rotation = Vector3.RotateTowards(transform.forward, direction, step, 0F);
            transform.rotation = Quaternion.LookRotation(rotation);
        }
    }

    private void Duration()
    {
        durationCurrent += Time.deltaTime;
        if (durationCurrent >= durationMax) Destroy(gameObject);
    }

    private void ActualMovement()
    {
        Vector3 movement = Vector3.forward * speed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.Self);
        Vector3 position = transform.position + movement;
        _rigidbody.MovePosition(position);
    }
}
