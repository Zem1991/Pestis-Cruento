using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCursor : MonoBehaviour
{
    [Header("Self references")]
    [SerializeField] private Transform cursorTelepointer;
    [SerializeField] private Transform levelTelepointer;
    [SerializeField] private Transform targetTelepointer;

    [Header("Screen cursor")]
    [SerializeField] private Vector2 currentPosScreen;
    [SerializeField] private Vector2 edgeCheck;
    [SerializeField] private Ray screenCursorRay;

    [Header("Scene cursor")]
    [SerializeField] private bool hasHitSomething;
    [SerializeField] private Vector3 currentPosScene;

    [Header("Found objects")]
    [SerializeField] private GameObject foundLevelPiece;
    [SerializeField] private GameObject foundTargetable;
    //[SerializeField] private Character foundCharacter;

    #region Screen cursor
    public Vector2 GetCurrentPosScreen() { return currentPosScreen; }
    public Vector2 GetEdgeCheck() { return edgeCheck; }
    public Ray GetScreenCursorRay() { return screenCursorRay; }
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

        screenCursorRay = camera.ScreenPointToRay(currentPosScreen);
        hasHitSomething = Physics.Raycast(screenCursorRay, out RaycastHit raycastHitSingle, Mathf.Infinity);
        currentPosScene = raycastHitSingle.point;

        cursorTelepointer.gameObject.SetActive(hasHitSomething);
        levelTelepointer.gameObject.SetActive(hasHitSomething);
        targetTelepointer.gameObject.SetActive(hasHitSomething);
        cursorTelepointer.transform.position = currentPosScene;
        levelTelepointer.transform.position = currentPosScene;
        targetTelepointer.transform.position = currentPosScene;
        
        if (!hasHitSomething) return;

        RaycastHit[] raycastHitMultiple = Physics.RaycastAll(screenCursorRay, Mathf.Infinity);
        foundTargetable = null;

        foreach (RaycastHit forRH in raycastHitMultiple)
        {
            if (foundLevelPiece && foundTargetable) break;

            //TODO: "foundLevelPiece"

            ITargetable forTargetable = forRH.collider.GetComponent<ITargetable>();
            if (forTargetable != null)
            {
                foundTargetable = forRH.collider.gameObject;
                targetTelepointer.transform.position = forTargetable.GetTargetablePosition();
            }
        }
    }
}
