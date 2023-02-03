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
    private class MusicNote
    {
        // [HideInInspector]
        public GameObject gameObject;
        public Vector3 position;
        public float correctTime;
        public float beat;
    }

    [Header("Core Map")]
    [SerializeField] private List<MusicNote> musicNotes;
    
    private int currentSpawnIndex = 0, currentDestroyIndex = 0;
    
    void Start()
    {
        timer.SetTimerValue(-initWaitTime);   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (currentDestroyIndex >= musicNotes.Count)
        {
            return;
        }
        
        MusicNote currentDestroyMusicNote = musicNotes[currentDestroyIndex];
        if (currentDestroyMusicNote.correctTime + destroySpawnTime < timer.GetTimerValue())
        {
            
            Destroy(currentDestroyMusicNote.gameObject);
            currentDestroyIndex++;
        }
     
        
        if (currentSpawnIndex >= musicNotes.Count)
        {
            return;
        }
        
        MusicNote currentSpawnMusicNote = musicNotes[currentSpawnIndex];
        if (currentSpawnMusicNote.correctTime - preSpawnTime < timer.GetTimerValue())
        {
            musicNotes[currentSpawnIndex].gameObject = Instantiate(AssetManager.Instance.hitNodePrefab, currentSpawnMusicNote.position, Quaternion.identity, transform);
            currentSpawnIndex++;
        }
        
    }


    public bool CheckHitMusicNote(Collider2D [] hits)
    {
        if (hits == null || currentDestroyIndex >= musicNotes.Count) return false;

        GameObject tobeCheckNoteTimer = musicNotes[currentDestroyIndex].gameObject;
        foreach (var hit in hits)
        {
            if (tobeCheckNoteTimer == hit.gameObject)
            {
                OnHitMusicNote();
                return true;
            }
        }

        return false;
    }

    private void OnHitMusicNote()
    {
        Destroy(musicNotes[currentDestroyIndex].gameObject);
        Debug.Log("Hit music note");
    }
        
}