using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [Header("Initial settings")]
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float durationMax;
    [SerializeField] private AbstractEffect effect;

    [Header("Dynamic settings")]
    [SerializeField] private Character spawner;
    [SerializeField] private Character target;
    [SerializeField] private float durationCurrent;

    public void Initialize(Character spawner, Character target)
    {
        this.spawner = spawner;
        this.target = target;
    }

    void Start()
    {
        Vector3 force = transform.forward * speed;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    void Update()
    {
        Vector3 forward = transform.forward;
        float deltaTime = Time.deltaTime;

        //Vector3 movement = forward * speed * deltaTime;
        //_rigidbody.MovePosition(movement);

        //if (angularSpeed > 0 && target != null)
        //{
        //    float step = angularSpeed * deltaTime;

        //    Vector3 direction = target.transform.position - transform.position;
        //    Vector3 rotation = Vector3.RotateTowards(forward, direction, step, 0F);
        //    Quaternion rot = Quaternion.Euler(rotation);
        //    _rigidbody.MoveRotation(rot);
        //}

        durationCurrent += deltaTime;
        if (durationCurrent >= durationMax) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();
        if (character == spawner) return;

        ContactPoint contact = collision.contacts[0];
        Vector3 point = contact.point;
        //Debug.Log("Projectile hit at: " + point);

        effect.ExecuteEffect(spawner, point, character);
        //_rigidbody.velocity = Vector3.zero;
        Destroy(gameObject);
    }
}
