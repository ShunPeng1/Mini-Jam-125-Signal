using System;
using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.Timer;
using UnityEngine;
using UnityUtilities;


public enum NoteColor
{
    Yellow,
    Blue,
    Silence
}

public enum NoteType
{
    WholeNote,
    HalfNote,
    QuarterNote,
    EightNote,
    
    
}

public class MusicNoteManager : SingletonMonoBehaviour<MusicNoteManager>
{
    
    [SerializeField] private Timer timer;

    [Header("Timer")] 
    [SerializeField] private int bpm = 100;
    [SerializeField] private int tempo = 4;
    [SerializeField] private float initWaitTime;
    [SerializeField] private float endWaitTime;
    
    [Header("Note Timer")]
    [SerializeField] private float deltaAccuracyTime;
    [SerializeField] private float preSpawnTime;
    [SerializeField] private float destroySpawnTime;
    
    
    [Serializable]
    private class MusicNote
    {
        public GameObject gameObject;
        public NoteType noteType;   
        
        [HideInInspector] public Vector3 position;
        [HideInInspector] public float correctTime;
    }

    [Serializable]
    private class ScoreHitValue
    {
        public GameObject prefabs;
        public float timeOffsetPercentage;
        public float value;
    }

    [Header("Core Map")]
    [SerializeField] private List<MusicNote> yellowMusicNotes;
    [SerializeField] private List<ScoreHitValue> scoreHitValues;

    private int currentSpawnIndex = 0, currentDestroyIndex = 0;
    
    void Start()
    {
        timer.SetTimerValue(-initWaitTime);   
        InitDecodeMusicNote();
    }


    
    private void InitDecodeMusicNote()
    {
        float nextBeatCorrectTime = 0f;
        foreach (var musicNote in yellowMusicNotes)
        {
            if (gameObject)
            {
                musicNote.position = musicNote.gameObject.transform.position;
            }
            
            float duration = DurationFromNoteType(musicNote.noteType);

            musicNote.correctTime = nextBeatCorrectTime;
            nextBeatCorrectTime += duration;
            
        }
        
    }

    private float DurationFromNoteType(NoteType noteType)
    {
        float beatSecond = (float) bpm /  60f;
        
        switch (noteType)
        {
            case NoteType.WholeNote:
                return beatSecond / (float)tempo;
                
            case NoteType.HalfNote:
                return 0.5f * beatSecond / (float)tempo;

            case NoteType.QuarterNote:
                return 0.25f * beatSecond / (float)tempo;
            
            case NoteType.EightNote:
                return 0.125f *beatSecond / (float)tempo;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(noteType), noteType, null);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        while (currentDestroyIndex < yellowMusicNotes.Count &&  yellowMusicNotes[currentDestroyIndex].correctTime + destroySpawnTime < timer.GetTimerValue())
        {
            //Debug.Log("DESPAWN");
            MusicNote currentDestroyMusicNote = yellowMusicNotes[currentDestroyIndex];

            if(currentDestroyMusicNote.gameObject != null) currentDestroyMusicNote.gameObject.SetActive(false);
            //Destroy(currentDestroyMusicNote.gameObject);
            currentDestroyIndex++;
            
        }
        
        while (currentSpawnIndex < yellowMusicNotes.Count &&  yellowMusicNotes[currentSpawnIndex].correctTime - preSpawnTime < timer.GetTimerValue())
        {
            //Debug.Log("SPAWN");
            MusicNote currentSpawnMusicNote = yellowMusicNotes[currentSpawnIndex];

            if(currentSpawnMusicNote.gameObject != null) currentSpawnMusicNote.gameObject.SetActive(true);
            //Destroy(currentDestroyMusicNote.gameObject);
            currentSpawnIndex++;
        }
        
        
    }


    public void CheckHitYellowMusicNote(Collider2D [] hits , Vector3 hitPosition)
    {
        if (currentDestroyIndex >= yellowMusicNotes.Count) return;
        
        GameObject tobeCheckNoteTimer = yellowMusicNotes[currentDestroyIndex].gameObject;
        while ( currentDestroyIndex < yellowMusicNotes.Count &&  tobeCheckNoteTimer.gameObject == null)
        {
            currentDestroyIndex++;
            tobeCheckNoteTimer = yellowMusicNotes[currentDestroyIndex].gameObject;
        }
        
        if (currentDestroyIndex >= yellowMusicNotes.Count) return;
        
        
        foreach (var hit in hits)
        {
            if (tobeCheckNoteTimer != hit.gameObject) continue;
            
            OnHitMusicNote(yellowMusicNotes[currentDestroyIndex]);
            return;
        }
        
        OnMissMusicNote(hitPosition);
        
    }

    private void OnHitMusicNote(MusicNote hit)
    {
        
        if(yellowMusicNotes[currentDestroyIndex].gameObject != null) yellowMusicNotes[currentDestroyIndex].gameObject.SetActive(false);
        
        // Score Popup
        float hitOffsetPercentage = Mathf.Abs( (float)(hit.correctTime - timer.GetTimerValue()) ) / (preSpawnTime + destroySpawnTime); 

        foreach (var scoreHitValue in scoreHitValues)
        {
            if ( hitOffsetPercentage < scoreHitValue.timeOffsetPercentage)
            {
                Instantiate(scoreHitValue.prefabs, hit.position, Quaternion.identity, transform);
                break;
            }
        }
        
        currentDestroyIndex++;
    }

    private void OnMissMusicNote(Vector3 hitPosition)
    {
        Instantiate(scoreHitValues[^1].prefabs, hitPosition, Quaternion.identity, transform);
    }    
    
    
}