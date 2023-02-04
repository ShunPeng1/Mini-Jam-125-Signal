using System;
using System.Collections;
using System.Collections.Generic;
using TurnTheGameOn.Timer;
using UnityEngine;
using UnityUtilities;


public enum NoteType
{
    Yellow,
    Blue,
    Switch
}

public enum NoteDuration
{
    WholeNote,
    HalfNote,
    QuarterNote,
    EightNote,
    
    
}

public class MusicNoteManager : SingletonMonoBehaviour<MusicNoteManager>
{
    [Header("Require Object")]
    [SerializeField] private Timer timer;
    [SerializeField] private PlayerControl player;
    [SerializeField] private TeslaCoilManager teslaCoilManager;
    
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
        public NoteDuration noteDuration;
        public NoteType noteType;
        
        [HideInInspector] public Vector3 position;
        [HideInInspector] public float correctTime;
    }

    [Serializable]
    private class ScoreHitValue
    {
        public GameObject prefabs;
        [Range(0f,1f)] public float timeOffsetPercentage;
        public float value;
    }

    [Header("Core Map")]
    [SerializeField] private List<MusicNote> musicNotes;
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
        foreach (var musicNote in musicNotes)
        {
            if (gameObject)
            {
                musicNote.position = musicNote.gameObject.transform.position;
            }
            
            float duration = DurationFromNoteType(musicNote.noteDuration);

            musicNote.correctTime = nextBeatCorrectTime;
            nextBeatCorrectTime += duration;
            
        }
        
    }

    private float DurationFromNoteType(NoteDuration noteDuration)
    {
        float beatSecond = (float) bpm /  60f;
        
        switch (noteDuration)
        {
            case NoteDuration.WholeNote:
                return beatSecond / (float)tempo;
                
            case NoteDuration.HalfNote:
                return 0.5f * beatSecond / (float)tempo;

            case NoteDuration.QuarterNote:
                return 0.25f * beatSecond / (float)tempo;
            
            case NoteDuration.EightNote:
                return 0.125f *beatSecond / (float)tempo;
            
            default:
                throw new ArgumentOutOfRangeException(nameof(noteDuration), noteDuration, null);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        while (currentDestroyIndex < musicNotes.Count &&  musicNotes[currentDestroyIndex].correctTime + destroySpawnTime < timer.GetTimerValue())
        {
            //Debug.Log("DESPAWN");
            MusicNote currentDestroyMusicNote = musicNotes[currentDestroyIndex];

            if(currentDestroyMusicNote.gameObject != null) currentDestroyMusicNote.gameObject.SetActive(false);
            //Destroy(currentDestroyMusicNote.gameObject);
            currentDestroyIndex++;
            
        }
        
        while (currentSpawnIndex < musicNotes.Count &&  musicNotes[currentSpawnIndex].correctTime - preSpawnTime < timer.GetTimerValue())
        {
            //Debug.Log("SPAWN");
            MusicNote currentSpawnMusicNote = musicNotes[currentSpawnIndex];

            if(currentSpawnMusicNote.gameObject != null) currentSpawnMusicNote.gameObject.SetActive(true);
            //Destroy(currentDestroyMusicNote.gameObject);
            currentSpawnIndex++;
        }
        
        
    }


    public void CheckHitYellowMusicNote(Collider2D [] hits , GameObject attractor)
    {
        if (currentDestroyIndex >= musicNotes.Count) return;
        
        GameObject tobeCheckNoteTimer = musicNotes[currentDestroyIndex].gameObject;
        while ( currentDestroyIndex < musicNotes.Count &&  tobeCheckNoteTimer.gameObject == null)
        {
            currentDestroyIndex++;
            tobeCheckNoteTimer = musicNotes[currentDestroyIndex].gameObject;
        }
        
        if (currentDestroyIndex >= musicNotes.Count) return;
        
        
        foreach (var hit in hits)
        {
            if (tobeCheckNoteTimer != hit.gameObject) continue;
            
            OnHitMusicNote(musicNotes[currentDestroyIndex]);
            return;
        }
        
        OnMissMusicNote(attractor);
        
    }

    private void OnHitMusicNote(MusicNote hit)
    {
        
        musicNotes[currentDestroyIndex].gameObject.SetActive(false);

        if (hit.noteType == NoteType.Switch)
        {
            teslaCoilManager.OnSwitchAttractor();
            player.OnSwitchAttractor();
        }
        
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

    private void OnMissMusicNote(GameObject attractor)
    {
        Instantiate(scoreHitValues[^1].prefabs, attractor.transform.position, Quaternion.identity, transform);
    }    
    
    
}