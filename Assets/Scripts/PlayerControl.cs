using System;
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
    
    [Header("Circle Collider")] 
    private CircleCollider2D yellowCollider;
    private CircleCollider2D blue0Collider;
    private CircleCollider2D blue1Collider;
    private CircleCollider2D blue2Collider;

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
        
        yellowCollider = yellowAttractor.GetComponent<CircleCollider2D>();
        blue0Collider = blue0Attractor.GetComponent<CircleCollider2D>();
        blue1Collider = blue1Attractor.GetComponent<CircleCollider2D>();
        blue2Collider = blue2Attractor.GetComponent<CircleCollider2D>();

        
        yellowRigidbody = yellowAttractor.GetComponent<Rigidbody2D>();
        blueClusterRigidbody = blueCluster.GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = (transform.position - centerPosition).z;

        MousePointerMovement(mousePosition);

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C) )
        {
            OnYellowClick();
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.X))
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
        //Debug.Log("right Click ");
        Collider2D [] hits= Physics2D.OverlapCircleAll(yellowAttractor.transform.position, yellowCollider.radius, yellowNoteLayerMask);
        Debug.Log(hits.ToString());
        //MusicNoteManager.Instance.CheckHitMusicNote(hits);
        
    }

    private void OnBlueClick()
    {
        
    }
    
}
