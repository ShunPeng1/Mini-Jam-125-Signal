using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    
    [Header("Attractor")] 
    [SerializeField] private GameObject yellowAttractor;
    [SerializeField] private GameObject blueCluster;
    [SerializeField] private GameObject blue0Attractor;
    [SerializeField] private GameObject blue1Attractor;
    [SerializeField] private GameObject blue2Attractor;
    

    [Header("Collider Radius")]
    [SerializeField] private float yellowRadius;
    [SerializeField] private float blueRadius;
    
    
    [Header("Rigidbody")]
    private Rigidbody2D yellowRigidbody;
    private Rigidbody2D blueClusterRigidbody;
    
    [Header("Checking Layer")] 
    [SerializeField] private LayerMask yellowNoteLayerMask;
    [SerializeField] private LayerMask blueNoteLayerMask;



    [Header("Distance From Center")] 
    private Vector3 centerPosition;
    [SerializeField] private float currentZAngleDegree;
    
    // Start is called before the first frame update
    void Start()
    {
        centerPosition = transform.position;
        currentZAngleDegree = 0;
        InitGetAttractorsComponent();
    }

    void InitGetAttractorsComponent()
    {
        yellowRigidbody = yellowAttractor.GetComponent<Rigidbody2D>();
        blueClusterRigidbody = blueCluster.GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = (transform.position - centerPosition).z;

        MousePointerMovement(mousePosition);
        
        
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) )
        {
            OnYellowClick();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C))
        {
            OnBlueClick();
        }
    }

    private void MousePointerMovement(Vector3 mousePosition)
    {
       
        float deltaAngle = Vector3.SignedAngle((yellowAttractor.transform.position - centerPosition).normalized , (mousePosition - centerPosition).normalized  , Vector3.forward);
        
        
        currentZAngleDegree += deltaAngle;
        currentZAngleDegree = currentZAngleDegree > 180 ? currentZAngleDegree - 360 : currentZAngleDegree < 180 ? currentZAngleDegree + 360 : currentZAngleDegree;
        
        float x = mousePosition.x;
        float y = mousePosition.y;
        
        
        //transform.rotation = Quaternion.Euler(0,0, currentZAngleDegree);

        //yellowAttractor.transform.position = new Vector3(x, y, 0);
        //blueCluster.transform.position = new Vector3(-x, -y, 0);

        yellowRigidbody.MovePosition(new Vector3(x, y, 0));
        yellowRigidbody.MoveRotation(currentZAngleDegree);
        
        blueClusterRigidbody.MovePosition(new Vector3(-x, -y, 0));
        blueClusterRigidbody.MoveRotation(currentZAngleDegree + 180f);
        
    }

    private void OnYellowClick()
    {
        var position = yellowAttractor.transform.position;
        Collider2D [] hits= Physics2D.OverlapCircleAll(position, yellowRadius, yellowNoteLayerMask);
        
        MusicNoteManager.Instance.CheckHitYellowMusicNote(hits, position);
        
    }

    private void OnBlueClick()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(yellowAttractor.transform.position, yellowRadius);
        Gizmos.DrawWireSphere(blue0Attractor.transform.position, blueRadius);
        Gizmos.DrawWireSphere(blue1Attractor.transform.position, blueRadius);
        Gizmos.DrawWireSphere(blue2Attractor.transform.position, blueRadius);
    }

}
