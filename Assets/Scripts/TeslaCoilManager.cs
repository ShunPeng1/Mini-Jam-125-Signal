using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class TeslaCoilManager : MonoBehaviour
{
    
    [SerializeField] private LightningBoltScript lightningBolt;

    [Header("Transform position")] 
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject yellowCoil, blueCoil;
    [SerializeField] private GameObject yellowAttractor, blueAttractor;

    [SerializeField] private bool isYellowNorBlue = true;
    void Start()
    {
        
    }

    
    public void OnSwitchAttractor()
    {
        if (isYellowNorBlue)
        {
            lightningBolt.StartPosition = blueCoil.transform.position;
            lightningBolt.EndPosition = blueAttractor.transform.position;
        }
        else
        {
            lightningBolt.StartPosition = yellowCoil.transform.position;
            lightningBolt.EndPosition = yellowAttractor.transform.position;
        }
    }
    
    
}
