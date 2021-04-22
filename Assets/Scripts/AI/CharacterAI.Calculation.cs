using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterAI : MonoBehaviour
{
    [Header("Calculation")]
    [SerializeField] private Character calcReachableEnemy;
    [SerializeField] private Vector3 calcReachableEnemyPos;

    private void CalcReachableEnemy()
    {
        calcReachableEnemy = null;
        calcReachableEnemyPos = Vector3.zero;

        foreach (Character forChar in detectedCharacterList)
        {
            bool hasNavigation = NavCan(forChar.transform.position, out UnityEngine.AI.NavMeshHit nmHit);
            if (hasNavigation)
            {
                calcReachableEnemy = forChar;
                calcReachableEnemyPos = nmHit.position;
                return;
            }
        }
    }
}
