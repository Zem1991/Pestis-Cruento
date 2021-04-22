using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public partial class CharacterAI : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private bool hasNavPath;
    [SerializeField] private NavMeshPath navPath;
    [SerializeField] private Vector3 navPathFirstDir;
    [SerializeField] private Vector3 navPathFirstPos;
    //[SerializeField] private Vector3 navPathFinalPos;
    //[SerializeField] private Vector3 navPathLookPos;

    public bool NavCan(Vector3 targetPos, out NavMeshHit nmHit)
    {
        return NavMesh.SamplePosition(targetPos, out nmHit, 1, NavMesh.AllAreas);
    }

    private void NavMove(Vector3 targetPos)
    {
        Vector3 myPos = transform.position;

        if (Vector3.Distance(targetPos, myPos) < currentDecisionStoppingDistance)
        {
            NavClear();
        }
        else
        {
            hasNavPath = NavMesh.CalculatePath(myPos, targetPos, NavMesh.AllAreas, navPath);
            if (hasNavPath)
            {
                navPathFirstPos = navPath.corners.Length > 1 ? navPath.corners[1] : myPos;
                //navPathFinalPos = targetPos;
                navPathFirstDir = navPathFirstPos - myPos;
                navPathFirstDir.Normalize();
            }
        }

        character.SetRotation(navPathFirstPos);
        character.SetMovement(navPathFirstDir);
    }

    private void NavClear()
    {
        Vector3 myPos = transform.position;
        hasNavPath = false;
        navPath.ClearCorners();
        navPathFirstDir = Vector3.zero;
        navPathFirstPos = myPos;
        //navPathFinalPos = myPos;
        //navPathLookPos = myPos;
    }
}
