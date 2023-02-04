using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextPopup : MonoBehaviour
{
    [SerializeField] private float existingTime = 1f;
    private IEnumerator Start()
    {
        
        yield return new WaitForSeconds(existingTime);
        Destroy(gameObject);
    }
    
    
}
