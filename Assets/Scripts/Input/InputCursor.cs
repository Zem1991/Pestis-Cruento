using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCursor : MonoBehaviour
{
    [Header("Screen cursor")]
    [SerializeField] private Vector2 currentPosScreen;
    [SerializeField] private Vector2 edgeCheck;

    [Header("Scene cursor")]
    [SerializeField] private bool hasHitSomething;
    [SerializeField] private Vector3 currentPosScene;

    [Header("Found objects")]
    [SerializeField] private GameObject foundTargetable;
    //[SerializeField] private Character foundCharacter;

    #region Screen cursor
    public Vector2 GetCurrentPosScreen() { return currentPosScreen; }
    public Vector2 GetEdgeCheck() { return edgeCheck; }
    #endregion

    #region Scene cursor
    public bool HasHitSomething() { return hasHitSomething; }
    public Vector3 GetCurrentPosScene() { return currentPosScene; }
    #endregion

    #region Found objects
    public GameObject GetFoundTargetable() { return foundTargetable; }
    //public Character GetFoundCharacter() { return foundCharacter; }
    #endregion

    public void ReadCursor(Camera camera)
    {
        currentPosScreen = Input.mousePosition;

        edgeCheck = new Vector2();
        if (currentPosScreen.x <= 0) edgeCheck.x--;
        if (currentPosScreen.x >= Screen.width - 1) edgeCheck.x++;
        if (currentPosScreen.y <= 0) edgeCheck.y--;
        if (currentPosScreen.y >= Screen.height - 1) edgeCheck.y++;
        edgeCheck.Normalize();

        Ray ray = camera.ScreenPointToRay(currentPosScreen);
        hasHitSomething = Physics.Raycast(ray, out RaycastHit raycastHitSingle, Mathf.Infinity);
        currentPosScene = raycastHitSingle.point;

        RaycastHit[] raycastHitMultiple = Physics.RaycastAll(ray, Mathf.Infinity);
        foundTargetable = null;

        foreach (RaycastHit forRH in raycastHitMultiple)
        {
            ITargetable forTargetable = forRH.collider.GetComponent<ITargetable>();
            if (forTargetable == null) continue;
            foundTargetable = forRH.collider.gameObject;
            break;
        }
    }
}
