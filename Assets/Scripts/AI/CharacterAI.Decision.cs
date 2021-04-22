using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterAI : MonoBehaviour
{
    //[Header("Decision: settings")]
    //[SerializeField] private Character decCharacter;
    //[SerializeField] private Vector3 decCharacterLastPos;

    [Header("Decision: current")]
    [SerializeField] AIDecision currentDecision;
    [SerializeField] private Vector3 currentDecisionPos;
    [SerializeField] private Character currentDecisionTarget;
    [SerializeField] private float currentDecisionStoppingDistance;

    private void DecideDecision()
    {
        AIDecision newDecision = AIDecision.NONE;
        Vector3 newDecisionPos = transform.position;
        Character newDecisionTarget = null;
        float newDecisionStoppingDistance = 0.1F;

        if (calcReachableEnemy)
        {
            newDecision = AIDecision.REACH_ENEMY;
            newDecisionPos = calcReachableEnemyPos;
            newDecisionTarget = calcReachableEnemy;
        }

        currentDecision = newDecision;
        currentDecisionPos = newDecisionPos;
        currentDecisionTarget = newDecisionTarget;
        currentDecisionStoppingDistance = newDecisionStoppingDistance;
    }

    private void ExecuteDecision()
    {
        switch (currentDecision)
        {
            case AIDecision.NONE:
                //NavClear();
                break;
            //case AIDecision.MOVE_TO:
            //    break;
            case AIDecision.REACH_ENEMY:
                //NavMove(currentDecisionPos);
                break;
            //case AIDecision.ATTACK_ENEMY:
            //    break;
            //default:
            //    break;
        }

        NavMove(currentDecisionPos);

        //bool hasDecisionTarget = DetPerform();
        //if (hasDecisionTarget)
        //{
        //    Character decisionTarget = detectedCharacter;
        //    bool hasNavigation = NavCan(decisionTarget.transform.position, out UnityEngine.AI.NavMeshHit nmHit);
        //    if (hasNavigation) decisionMovePos = nmHit.position;
        //}

        //NavMove(decisionMovePos);
    }
}
