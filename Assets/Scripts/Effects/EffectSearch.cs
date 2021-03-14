using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSearch : MonoBehaviour
{
    //[Header("Settings")]
    //TODO: add MANY stuff here

    [Header("Results")]
    [SerializeField] private List<GameObject> resultList = new List<GameObject>();

    public List<GameObject> GetResultList() { return resultList; }

    public bool Search(MainCharacter user, Vector3 targetPos, GameObject targetObj)
    {
        //TODO: add MANY stuff here
        if (targetObj)
        {
            resultList.Add(targetObj);
        }
        return true;
    }
}
