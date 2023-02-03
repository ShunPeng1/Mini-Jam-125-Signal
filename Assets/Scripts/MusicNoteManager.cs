using System;
using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.Timer;
using UnityEngine;
using UnityUtilities;

public class MusicNoteManager : SingletonMonoBehaviour<MusicNoteManager>
{
    
    [SerializeField] private Timer timer;

    [Header("Timer")] 
    [SerializeField] private float initWaitTime;
    [SerializeField] private float endWaitTime;
    
    [Header("Note Timer")]
    [SerializeField] private float deltaAccuracyTime;
    [SerializeField] private float preSpawnTime;
    [SerializeField] private float destroySpawnTime;
    
    
    [Serializable]
    private class NoteTimer
    {
        [HideInInspector] public GameObject gameObject;
        public Vector3 position;
        public float correctTime;
        public float beat;
    }

    [Header("Core Map")]
    [SerializeField] private List<NoteTimer> noteTimers;
    
    private int currentSpawnIndex, currentDestroyIndex;
    
    void Start()
    {
        timer.SetTimerValue(-initWaitTime);   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentSpawnIndex >= noteTimers.Count)
        {
            return;
        }

        NoteTimer currentSpawnNote = noteTimers[currentDestroyIndex], currentDestroyNote = noteTimers[currentDestroyIndex];
        if (currentSpawnNote.correctTime - preSpawnTime < timer.GetTimerValue())
        {
            noteTimers[currentDestroyIndex].gameObject= Instantiate(AssetManager.Instance.hitNodePrefab, currentSpawnNote.position, Quaternion.identity, transform);
            currentSpawnIndex++;
        }
        
        if (currentDestroyNote.correctTime - destroySpawnTime < timer.GetTimerValue())
        {
            Destroy(currentDestroyNote.gameObject);
            currentDestroyIndex++;
        }


    }

    
}