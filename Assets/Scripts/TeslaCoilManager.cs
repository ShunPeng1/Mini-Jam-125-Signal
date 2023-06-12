using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class TeslaCoilManager : MonoBehaviour
{
    
    [SerializeField] private LightningBoltScript yellowLightningBolt;
    [SerializeField] private LightningBoltScript blueLightningBolt;

    [Header("Transform position")] 
    [SerializeField] private PlayerControl player;
    [SerializeField] private GameObject yellowCoil, blueCoil;
    [SerializeField] private GameObject yellowAttractor, blueAttractor;

    
    [SerializeField] private bool isYellowNorBlue;
    void Start()
    {
        
        yellowLightningBolt.StartPosition = yellowCoil.transform.position;
        yellowLightningBolt.EndPosition = yellowAttractor.transform.position;
        blueLightningBolt.StartPosition = blueCoil.transform.position;
        blueLightningBolt.EndPosition = blueAttractor.transform.position;
        
        //OnSwitchAttractor();
        yellowLightningBolt.gameObject.SetActive(true);
        blueLightningBolt.gameObject.SetActive(true);
    }

    
    public void OnSwitchAttractor()
    {
        if (isYellowNorBlue)
        {
            yellowLightningBolt.gameObject.SetActive(false);
            blueLightningBolt.gameObject.SetActive(true);
        }
        else
        {
            yellowLightningBolt.gameObject.SetActive(true);
            blueLightningBolt.gameObject.SetActive(false);
        }
    }
    
    
}
