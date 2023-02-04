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
    
    
    [Header("Checking Layer")] 
    [SerializeField] private LayerMask yellowNoteLayerMask;
    [SerializeField] private LayerMask blueNoteLayerMask;
    
    
    [Header("Mouse")] 
    [SerializeField, Range(0.01f, 2f)]private float mouseSensitivity = 1f;

    [Header("Distance From Center")]
    private Vector3 centerPosition;
    private float currentZAngleDegree;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        centerPosition = transform.position;
        currentZAngleDegree = 0;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        InitGetAttractorsComponent();
    }

    void InitGetAttractorsComponent()
    {
    }
    

    private void Update()
    {
       

        MousePointerMovement();
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) )
        {
            OnClickForHit();
        }

        
    }
    
    private void MousePointerMovement()
    {
        
       
        Debug.Log(Input.GetAxis("Mouse X").ToString()+" " + Input.GetAxis("Mouse Y").ToString());
        
        //mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * mouseSensitivity;
        
        mousePosition.z = (transform.position - centerPosition).z;
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
        mousePosition = currentAttractor.transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(currentAttractor.transform.position, yellowRadius);
        Gizmos.DrawWireSphere(waitAttractor.transform.position, blueRadius);
    }

}
