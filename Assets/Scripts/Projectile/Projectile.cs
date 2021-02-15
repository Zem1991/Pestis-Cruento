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
    [SerializeField] protected Character homingTarget;

    [Header("Duration")]
    [SerializeField] protected float durationCurrent;

    public void Initialize(Character owner, Character homingTarget)
    {
        this.owner = owner;
        this.homingTarget = homingTarget;
    }

    protected virtual void OnDrawGizmos()
    {
        if (HasGuidance())
        {
            Color homingColor = Color.red;
            homingColor.b = 0.5F;
            Gizmos.color = homingColor;
            Gizmos.DrawLine(transform.position, homingTarget.transform.position);
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

        Character character = collision.gameObject.GetComponent<Character>();
        effect.ExecuteEffect(owner, point, character);
        //_rigidbody.velocity = Vector3.zero;
        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Character character = other.GetComponent<Character>();
    //    if (character == spawner) return;

    //    Vector3 point = other.;
    //    //Debug.Log("Projectile hit at: " + point);

    //    effect.ExecuteEffect(spawner, point, character);
    //    //_rigidbody.velocity = Vector3.zero;
    //    Destroy(gameObject);
    //}
    
    private bool HasGuidance()
    {
        return homingSpeed > 0 && homingTarget != null;
    }
    
    private void Guidance()
    {
        if (HasGuidance())
        {
            Vector3 targetPos = homingTarget.transform.position + homingTarget.GetCenterOfMass();
            float step = homingSpeed * Time.deltaTime;

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
