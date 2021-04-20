using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private Character character;
    //[SerializeField] private NavMeshAgent _nmAgent;

    [Header("Detection settings")]
    [SerializeField] private float sightRange = 15F;
    [SerializeField] private float sightRadius = 120F;

    [Header("Detection results")]
    [SerializeField] private Character detectedCharacter;
    [SerializeField] private Vector3 detectedCharacterPos;

    [Header("Pathfinding")]
    [SerializeField] private bool hasNavPath;
    [SerializeField] private NavMeshPath navPath;
    [SerializeField] private Vector3 navPathFirstPos;
    [SerializeField] private Vector3 navPathFinalPos;

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

        if (detectedCharacter)
        {
            Gizmos.color = GizmoColors.detectionTarget;
            Gizmos.DrawLine(myPos, detectedCharacterPos);
        }

        if (hasNavPath)
        {
            Gizmos.color = GizmoColors.movementPath;
            Vector3[] corners = navPath.corners;
            Vector3 fromPos = myPos;
            Vector3 toPos;
            for (int index = 0; index < corners.Length; index++)
            {
                toPos = corners[index];
                Gizmos.DrawLine(fromPos, toPos);
                fromPos = toPos;
            }
        }
    }

    private void Awake()
    {
        navPath = new NavMeshPath();

        Vector3 myPos = transform.position;
        navPathFirstPos = myPos;
        navPathFinalPos = myPos;
    }

    private void Update()
    {
        Vector3 myPos = transform.position;

        bool hasTarget = PerformDetection();
        if (hasTarget) navPathFinalPos = detectedCharacterPos;

        Vector3 targetPos = navPathFinalPos;
        hasNavPath = NavMesh.CalculatePath(myPos, targetPos, NavMesh.AllAreas, navPath);

        if (!hasNavPath || navPathFinalPos == myPos)
        {
            hasNavPath = false;
            navPath.ClearCorners();
            navPathFirstPos = myPos;
            navPathFinalPos = myPos;
        }
        else
        {
            if (navPath.corners.Length > 1) navPathFirstPos = navPath.corners[1];

            Vector3 navDirection = navPathFirstPos - myPos;
            navDirection.Normalize();

            character.Rotation(navPathFirstPos);
            character.Movement(navDirection);
        }
    }

    #region Detection
    public bool CheckDetection(Character target)
    {
        if (!target) return false;

        Allegiance myAllegiance = character.GetAllegiance();
        Allegiance targetAllegiance = target.GetAllegiance();

        bool isEnemy = myAllegiance.CheckOpponent(targetAllegiance);
        if (!isEnemy) return false;

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

        return true;
    }

    private bool PerformDetection()
    {
        if (detectedCharacter)
        {
            bool keepSame = CheckDetection(detectedCharacter);
            if (keepSame)
            {
                SetDetectedTarget(detectedCharacter);
                return true;
            }
        }

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
                SetDetectedTarget(possibleTarget);
                return true;
            }
        }

        SetDetectedTarget(null);
        return false;
    }

    private void SetDetectedTarget(Character target)
    {
        if (target)
        {
            detectedCharacter = target;
            detectedCharacterPos = target.transform.position;
        }
        else
        {
            detectedCharacter = null;
            detectedCharacterPos = transform.position;
        }
    }
    #endregion
}
