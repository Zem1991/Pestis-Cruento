using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterAI : MonoBehaviour
{
    [Header("Detection: settings")]
    [SerializeField] private float sightRange = 15F;
    [SerializeField] private float sightRadius = 120F;

    [Header("Detection: results")]
    [SerializeField] private List<Character> detectedCharacterList = new List<Character>();

    public bool DetCheck(Character target)
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

    private void DetPerform()
    {
        detectedCharacterList.Clear();
        Collider[] candidates = Physics.OverlapSphere(transform.position, sightRange);

        foreach (Collider item in candidates)
        {
            //Ignore self
            GameObject gObj = item.gameObject;
            if (gObj == gameObject) continue;

            //Must be an valid target
            Character possibleTarget = item.GetComponent<Character>();
            if (!possibleTarget) continue;

            bool detected = DetCheck(possibleTarget);
            if (detected) detectedCharacterList.Add(possibleTarget);
        }
    }
}
