using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    
    
    [Header("Attractor")] 
    [SerializeField] private GameObject yellowAttractor;
    [SerializeField] private GameObject blueCluster;
    [SerializeField] private GameObject blue0Attractor;
    [SerializeField] private GameObject blue1Attractor;
    [SerializeField] private GameObject blue2Attractor;

    [Header("Distance From Center")] 
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 yellowInitPosition;
    [SerializeField] private Vector3 blue0InitPosition;
    [SerializeField] private Vector3 blue1InitPosition;
    [SerializeField] private Vector3 blue2InitPosition;
    [SerializeField] private float currentZAngleDegree;
    
    // Start is called before the first frame update
    void Start()
    {
        centerPosition = transform.position;
        yellowInitPosition = yellowAttractor.transform.localPosition;
        blue0InitPosition  = blue0Attractor.transform.localPosition;
        blue1InitPosition  = blue1Attractor.transform.localPosition;
        blue2InitPosition  = blue2Attractor.transform.localPosition;
        currentZAngleDegree = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        mousePosition.z = (transform.position - centerPosition).z;
        float deltaAngle = Vector3.SignedAngle((yellowAttractor.transform.position - centerPosition).normalized , (mousePosition - centerPosition).normalized  , Vector3.forward);
        
        Debug.Log(mousePosition);
        currentZAngleDegree += deltaAngle;
        currentZAngleDegree = currentZAngleDegree > 180 ? currentZAngleDegree - 360 : currentZAngleDegree < 180 ? currentZAngleDegree + 360 : currentZAngleDegree;

        float rangeFaction = (yellowAttractor.transform.position - centerPosition).magnitude /
                             (yellowInitPosition - centerPosition).magnitude;
        
        
        float yellowRange = (yellowAttractor.transform.position - centerPosition).magnitude;
        float xYellow = yellowRange * Mathf.Cos(currentZAngleDegree * Mathf.Deg2Rad) + centerPosition.x;
        float yYellow = yellowRange * Mathf.Sin(currentZAngleDegree * Mathf.Deg2Rad) + centerPosition.y;

        transform.rotation = Quaternion.Euler(0,0, currentZAngleDegree);
        yellowAttractor.transform.position = new Vector3(xYellow, yYellow, 0);

        blueCluster.transform.position = new Vector3(-xYellow, -yYellow, 0);
        

    }
}
