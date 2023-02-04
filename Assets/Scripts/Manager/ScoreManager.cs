using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtilities;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _currentScore = 0;
    private int _totalScore = 0;

    [Header("Adding Score")] 
    [SerializeField, Range(0f,1f)]private float rateOfGrowth ;
    
    public void AddScore(int scoreAdding)
    {
        _totalScore += scoreAdding;
        
        
    }

    private void Update()
    {
        
        
    }
    
    
    

}
