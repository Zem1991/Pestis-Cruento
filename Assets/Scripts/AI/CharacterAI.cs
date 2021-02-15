using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Character character;

    [Header("Detection settings")]
    [SerializeField] private float sightRange = 15F;
    [SerializeField] private float sightRadius = 120F;

    [Header("Detection findings")]
    [SerializeField] private Character foundCharacter;

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        
        Color color = new Color(1, .5F, 0, 1);
        Gizmos.color = color;
        Gizmos.DrawWireSphere(position, sightRange);

        float halfSightRadius = sightRadius / 2F;
        Vector3 fovMidPos = transform.forward * sightRange;
        Vector3 fovLeftPos = Quaternion.Euler(0, -halfSightRadius, 0) * fovMidPos;
        Vector3 fovRightPos = Quaternion.Euler(0, halfSightRadius, 0) * fovMidPos;
        
        color.g = 1;
        Gizmos.color = color;
        Gizmos.DrawLine(position, position + fovMidPos);
        Gizmos.DrawLine(position, position + fovLeftPos);
        Gizmos.DrawLine(position, position + fovRightPos);

        if (foundCharacter)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, foundCharacter.transform.position);
        }
    }

    private void Update()
    {
        bool hasTarget = CheckDetection(foundCharacter);
        if (!hasTarget) PerformDetection();
    }

    private bool CheckDetection(Character character)
    {
        if (!character) return false;

        Vector3 from = transform.position;
        Vector3 to = character.transform.position;
        float distance = Vector3.Distance(from, to);
        if (distance > sightRange) return false;

        Vector3 direction = to - from;
        float angle = Vector3.Angle(transform.forward, direction);
        angle = Mathf.Abs(angle);
        float halfSightRadius = sightRadius / 2F;
        return angle < halfSightRadius;
    }

    private void PerformDetection()
    {
        foundCharacter = null;

        Collider[] candidates = Physics.OverlapSphere(transform.position, sightRange);
        foreach (Collider item in candidates)
        {
            GameObject gObj = item.gameObject;
            if (gObj == gameObject) continue;

            Character possibleTarget = item.GetComponent<Character>();
            if (!possibleTarget) continue;

            bool detected = CheckDetection(possibleTarget);
            if (detected)
            {
                foundCharacter = possibleTarget;
                break;
            }
        }
    }
}
