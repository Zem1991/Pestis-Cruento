using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statuses : MonoBehaviour
{
    private Character _character;

    [Header("Settings")]
    [SerializeField] protected List<AbstractStatus> statusList = new List<AbstractStatus>();

    void Start()
    {
        _character = GetComponentInParent<Character>();
    }

    void Update()
    {
        List<AbstractStatus> copyStatusList = new List<AbstractStatus>(statusList);
        foreach (AbstractStatus forStatus in copyStatusList)
        {
            bool isDone = forStatus.UpdateStatus(Time.deltaTime);
            if (isDone)
            {
                //forStatus.RemoveStatus(_character);
                RemoveStatus(forStatus);
            }
            else
            {
                //forStatus.CauseStatus(_character);
            }
        }
    }

    #region Add status
    public bool AddStatus(AbstractStatus status)
    {
        DeflectionStatus deflectionStatus = status as DeflectionStatus;
        if (deflectionStatus) return AddDeflectionStatus(deflectionStatus);
        return false;
    }
    private bool AddDeflectionStatus(DeflectionStatus deflectionStatus)
    {
        DeflectionStatus newStatus = Instantiate(deflectionStatus, transform);
        statusList.Add(newStatus);
        float charaDeflectionArc = _character.GetDeflectionArc();
        float statusDeflectionArc = newStatus.GetDeflectionArc();
        if (charaDeflectionArc < statusDeflectionArc) _character.SetDeflectionArc(statusDeflectionArc);
        return true;
    }
    #endregion

    #region Remove status
    private bool RemoveStatus(AbstractStatus status)
    {
        DeflectionStatus deflectionStatus = status as DeflectionStatus;
        if (deflectionStatus) RemoveDeflectionStatus(deflectionStatus);
        statusList.Remove(deflectionStatus);
        Destroy(deflectionStatus.gameObject);
        return false;
    }
    private void RemoveDeflectionStatus(DeflectionStatus deflectionStatus)
    {
        DeflectionStatus otherStatus = null;
        float newDeflectionArc = 0F;
        foreach (DeflectionStatus forStatus in statusList)
        {
            if (forStatus == deflectionStatus) continue;
            if (!otherStatus || otherStatus.GetDeflectionArc() < forStatus.GetDeflectionArc())
            {
                otherStatus = forStatus;
                newDeflectionArc = otherStatus.GetDeflectionArc();
            }
        }
        _character.SetDeflectionArc(newDeflectionArc);
    }
    #endregion
}
