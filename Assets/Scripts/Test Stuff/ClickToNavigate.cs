using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToNavigate : MonoBehaviour
{
    public Camera navCam;
    public NavMeshAgent nmAgent;
    public NavMeshPath path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 currentPosScreen = Input.mousePosition;
            Ray screenCursorRay = navCam.ScreenPointToRay(currentPosScreen);

            Plane xzPlane = new Plane(Vector3.up, 0);
            bool hasHitSomething = xzPlane.Raycast(screenCursorRay, out float enter);

            //bool hasHitSomething = Physics.Raycast(screenCursorRay, out RaycastHit raycastHitSingle, Mathf.Infinity);
            Vector3 point = screenCursorRay.GetPoint(enter);

            Debug.Log(point);
            nmAgent.SetDestination(point);
            //if (nmAgent.CalculatePath(point, path))
            //{
                
            //}
        }
    }
}
