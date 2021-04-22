using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterAI : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private Character character;

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

        if (currentDecisionTarget)
        {
            Gizmos.color = GizmoColors.detectionTarget;
            Gizmos.DrawLine(myPos, currentDecisionPos);
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
        navPath = new UnityEngine.AI.NavMeshPath();
        NavClear();
    }

    private void Update()
    {
        //These two should be overriden by somehting like orders from an officer
        ReadContext();
        MakeCalculations();

        //And this one should be called always.
        TakeDecision();
    }

    private void ReadContext()
    {
        //Do detection - sight, hearing, etc
        DetPerform();
    }

    private void MakeCalculations()
    {
        //Filter known enemies that can be attacked
        //TODO: ...

        //Filter known enemies that can be reached
        CalcReachableEnemy();
    }

    private void TakeDecision()
    {
        //select best action
        //  - check what to attack
        //  - check where to move
        DecideDecision();

        //perform said action
        ExecuteDecision();
    }
}
