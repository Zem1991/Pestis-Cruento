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
    [SerializeField] protected GameObject spawner;
    [SerializeField] protected ITargetable homingTarget;
    [SerializeField] protected GameObject homingTargetObj;

    [Header("Duration")]
    [SerializeField] protected float durationCurrent;

    public void Initialize(GameObject spawner, GameObject homingTargetObj)
    {
        this.spawner = spawner;
        bool validHoming = CheckValidHomingTarget(homingTargetObj);
        if (validHoming)
        {
            homingTarget = homingTargetObj?.GetComponent<ITargetable>();
            this.homingTargetObj = homingTargetObj;

            transform.LookAt(homingTarget.GetTargetablePosition());
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (homingTarget != null)
        {
            Color homingColor = HasGuidance() ? Color.cyan : Color.blue;
            homingColor.b = 0.5F;
            Vector3 position = homingTarget.GetTargetablePosition();
            Gizmos.color = homingColor;
            Gizmos.DrawLine(transform.position, position);
        }
    }

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(_collider, spawner.GetComponent<Collider>());
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

        Character spanwerChar = spawner.GetComponent<Character>();
        GameObject targetObj = collision.gameObject;
        Vector3 hitNormal = collision.contacts[0].normal;

        if (CheckDeflection(targetObj, hitNormal))
        {
            hitNormal.y = 0;
            hitNormal.Normalize();
            //Debug.Log("hitNormal: " + hitNormal);

            float angle = Mathf.Atan2(hitNormal.z, hitNormal.x);
            angle *= Mathf.Rad2Deg;
            angle = Helper.RoundToMultiple(angle, 45F);
            Debug.Log("angle: " + angle);

            Vector3 reflect = Vector3.Reflect(transform.forward, hitNormal);
            transform.rotation = Quaternion.LookRotation(reflect);

            //transform.Rotate(Vector3.up, angle);

            Vector3 eulerMulitple = transform.eulerAngles;
            eulerMulitple.y = Helper.RoundToMultiple(eulerMulitple.y, 45F);
            //eulerMulitple.y = Helper.RoundToMultiple(eulerMulitple.y, 22.5F);
            transform.eulerAngles = eulerMulitple;
            return;
        }

        effect.ExecuteEffect(spanwerChar, point, targetObj);
        Destroy(gameObject);
    }

    protected bool CheckValidHomingTarget(GameObject obj)
    {
        ITargetable targetable = obj?.GetComponent<ITargetable>();
        if (targetable == null) return false;

        bool isCharacter = false;
        Character spanwerChar = spawner.GetComponent<Character>();
        if (spanwerChar)
        {
            Character targetChar = obj.GetComponent<Character>();
            bool charaIsNotOwner = targetChar != spawner;
            bool charaIsOpponent = spanwerChar.CheckOpponent(targetChar);
            isCharacter = targetChar && charaIsNotOwner && charaIsOpponent;
        }

        ITelekinesisTarget tkTarget = obj.GetComponent<ITelekinesisTarget>();
        TelekinesisEffect tkEffect = effect as TelekinesisEffect;
        bool isTkTarget = tkTarget != null && tkEffect;

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
            Vector3 targetPos = homingTarget.GetTargetablePosition();
            //Vector3 targetPos = this.homingTarget.transform.position;
            float step = homingSpeed * Time.deltaTime;

            //Character homingTarget = this.homingTarget.GetComponent<Character>();
            //if (homingTarget) targetPos += homingTarget.GetTargetablePosition();

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
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 position = transform.position + movement;
        _rigidbody.MovePosition(position);
    }

    private bool CheckDeflection(GameObject collidedObject, Vector3 hitNormal)
    {
        Character collidedChara = collidedObject.GetComponent<Character>();
        if (!collidedChara) return false;
        float deflectionAngle = collidedChara.GetDeflectionArc();
        if (deflectionAngle <= 0) return false;

        deflectionAngle /= 2F;
        Vector3 forward = collidedObject.transform.forward;
        float hitAngle = Vector3.Angle(forward, hitNormal);
        return deflectionAngle >= hitAngle;
    }
}
