using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PlayerControl : MonoBehaviour
{
    
    [Header("Attractor")] 
    [SerializeField] private GameObject currentAttractor;
    [SerializeField] private GameObject waitAttractor;


    [Header("Collider Radius")]
    [SerializeField] private float yellowRadius;
    [SerializeField] private float blueRadius;
    
    
    [Header("Rigidbody")]
    private Rigidbody2D yellowRigidbody;
    private Rigidbody2D blueAttractorRigidbody;
    
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
        yellowRigidbody = currentAttractor.GetComponent<Rigidbody2D>();
        blueAttractorRigidbody = waitAttractor.GetComponent<Rigidbody2D>();
    }
    

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = (transform.position - centerPosition).z;

        MousePointerMovement(mousePosition);
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) )
        {
            OnClickForHit();
        }

        
    }

    
    private void MousePointerMovement(Vector3 mousePosition)
    {
       
        float deltaAngle = Vector3.SignedAngle((currentAttractor.transform.position - centerPosition).normalized , (mousePosition - centerPosition).normalized  , Vector3.forward);
        
        
        currentZAngleDegree += deltaAngle;
        currentZAngleDegree = currentZAngleDegree > 180 ? currentZAngleDegree - 360 : currentZAngleDegree < 180 ? currentZAngleDegree + 360 : currentZAngleDegree;
        
        float x = mousePosition.x;
        float y = mousePosition.y;


        currentAttractor.transform.position = new Vector3(x, y, 0);
        currentAttractor.transform.rotation = Quaternion.Euler(0, 0, currentZAngleDegree);
        
        waitAttractor.transform.position = new Vector3(-x, -y, 0);
        waitAttractor.transform.rotation = Quaternion.Euler(0, 0,180f+ currentZAngleDegree);

        //yellowRigidbody.MovePosition(new Vector3(x, y, 0));
        //yellowRigidbody.MoveRotation(currentZAngleDegree);
        
        //blueClusterRigidbody.MovePosition(new Vector3(-x, -y, 0));
        //blueClusterRigidbody.MoveRotation(currentZAngleDegree + 180f);
        
    }

    private void OnClickForHit()
    {
        var position = currentAttractor.transform.position;
        Collider2D [] hits= Physics2D.OverlapCircleAll(position, yellowRadius, yellowNoteLayerMask);
        
        MusicNoteManager.Instance.CheckHitYellowMusicNote(hits, currentAttractor);
        
    }

    public void OnSwitchAttractor()
    {
        (currentAttractor, waitAttractor) = (waitAttractor, currentAttractor);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentAttractor.transform.position, yellowRadius);
        Gizmos.DrawWireSphere(waitAttractor.transform.position, blueRadius);
    }

}
