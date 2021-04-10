using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private Character character;
    [SerializeField] private NavMeshAgent _nmAgent;

    [Header("Detection settings")]
    [SerializeField] private float sightRange = 15F;
    [SerializeField] private float sightRadius = 120F;

    [Header("Detection findings")]
    [SerializeField] private Ray foundCharacterRay;
    [SerializeField] private Character foundCharacter;

    [Header("Pathfinding")]
    [SerializeField] private bool hasNavPath;
    [SerializeField] private Vector3 navTargetPos;
    [SerializeField] private NavMeshPath navPath;

    private void OnDrawGizmos()
    {
        Vector3 myPos = transform.position;
        
        Gizmos.color = GizmoColors.detectionSightRange;
        Gizmos.DrawWireSphere(myPos, sightRange);

        float halfSightRadius = sightRadius / 2F;
        Vector3 fovMidPos = transform.forward * sightRange;
        Vector3 fovLeftPos = Quaternion.Euler(0, -halfSightRadius, 0) * fovMidPos;
        Vector3 fovRightPos = Quaternion.Euler(0, halfSightRadius, 0) * fovMidPos;
        
        Gizmos.color = GizmoColors.detectionSightArc;
        //Gizmos.DrawLine(myPos, myPos + fovMidPos);
        Gizmos.DrawLine(myPos, myPos + fovLeftPos);
        Gizmos.DrawLine(myPos, myPos + fovRightPos);

        if (foundCharacter)
        {
            Gizmos.color = GizmoColors.detectionTarget;
            Gizmos.DrawRay(foundCharacterRay);
            //Gizmos.DrawLine(myPos, foundCharacter.transform.position);
        }

        if (hasNavPath)
        {
            Gizmos.color = GizmoColors.movementPath;
            Gizmos.DrawLine(myPos, navTargetPos);
        }
    }

    private void Awake()
    {
        navPath = new NavMeshPath();
    }

    private void Update()
    {
        bool hasTarget = CheckDetection(foundCharacter);
        if (!hasTarget)
        {
            PerformDetection();
        }
        else if (_nmAgent)
        {
            Vector3 foundCharacterPos = foundCharacter.transform.position;
            hasNavPath = _nmAgent.CalculatePath(foundCharacterPos, navPath);

            if (hasNavPath)
            {
                Debug.Log("hasNavPath");
                navTargetPos = navPath.corners[1];

                Vector3 navDirection = navTargetPos - transform.position;
                navDirection.Normalize();
                character.Rotation(navTargetPos);
                character.Movement(navDirection);
            }
        }
    }

    private bool CheckDetection(Character target)
    {
        if (!target) return false;

        Vector3 rayFrom = character.GetTargetablePosition();
        Vector3 rayTo = target.GetTargetablePosition();
        Vector3 rayDir = (rayTo - rayFrom).normalized;
        Ray ray = new Ray(rayFrom, rayDir);

        float angle = Vector3.Angle(transform.forward, rayDir);
        angle = Mathf.Abs(angle);
        float halfSightRadius = sightRadius / 2F;

        bool withinSightArc = angle < halfSightRadius;
        if (!withinSightArc) return false;

        bool withinSightRange = Physics.Raycast(ray, out RaycastHit hitInfo, sightRange);
        if (!withinSightRange) return false;

        bool sightNotBlocked = hitInfo.collider.gameObject == target.gameObject;
        if (!sightNotBlocked) return false;

        foundCharacterRay = ray;
        return true;
    }

    private void PerformDetection()
    {
        foundCharacter = null;

        Collider[] candidates = Physics.OverlapSphere(transform.position, sightRange);
        foreach (Collider item in candidates)
        {
            GameObject gObj = item.gameObject;
            if (gObj == gameObject) continue;   //Ignore self

            Character possibleTarget = item.GetComponent<Character>();
            if (!possibleTarget) continue;      //Must be an valid target

            bool detected = CheckDetection(possibleTarget);
            if (detected)
            {
                foundCharacter = possibleTarget;
                break;
            }
        }
    }
}
