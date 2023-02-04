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

    [Header("Core Map")]
    [SerializeField] private List<MusicNote> yellowMusicNotes;
    
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
        
        if (currentDestroyIndex >= yellowMusicNotes.Count)
        {
            return;
        }
        
        MusicNote currentDestroyMusicNote = yellowMusicNotes[currentDestroyIndex];
        if (currentDestroyMusicNote.correctTime + destroySpawnTime < timer.GetTimerValue())
        {
            yellowMusicNotes[currentDestroyIndex].gameObject.SetActive(true);
            //Destroy(currentDestroyMusicNote.gameObject);
            currentDestroyIndex++;
        }
     
        
        if (currentSpawnIndex >= yellowMusicNotes.Count)
        {
            return;
        }
        
        MusicNote currentSpawnMusicNote = yellowMusicNotes[currentSpawnIndex];
        if (currentSpawnMusicNote.correctTime - preSpawnTime < timer.GetTimerValue())
        {
            //yellowMusicNotes[currentSpawnIndex].gameObject = Instantiate(AssetManager.Instance.hitNodePrefab, currentSpawnMusicNote.position, Quaternion.identity, transform);
            yellowMusicNotes[currentSpawnIndex].gameObject.SetActive(true);
            
            currentSpawnIndex++;
        }
        
    }


    public bool CheckHitYellowMusicNote(Collider2D [] hits)
    {
        if (hits == null || currentDestroyIndex >= yellowMusicNotes.Count) return false;

        GameObject tobeCheckNoteTimer = yellowMusicNotes[currentDestroyIndex].gameObject;
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
        yellowMusicNotes[currentDestroyIndex].gameObject.SetActive(false);
        //Destroy(yellowMusicNotes[currentDestroyIndex].gameObject);
        currentDestroyIndex++;
        Debug.Log("Hit music note");
    }
        
}