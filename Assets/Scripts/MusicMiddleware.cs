using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicMiddleware : MonoBehaviour {


    [System.Serializable]
    public class SoundEntry {
        public AudioClip sound;
        [HideInInspector]
        public AudioSource source;
        [HideInInspector]
        public bool looping = false;
        public float volume = 1;
        public float loopStartTime;
        public float loopEndTime;
    }
    private SoundEntry playEntry;
    public List<SoundEntry> sounds; 

    private static List<SoundEntry> staticSounds;

    public static void playSound(string name){
        SoundEntry playEntry = staticSounds.Find(item => item.sound.name == name);
        playEntry.source.volume = playEntry.volume;
        playEntry.source.Play();
    }

    public static void loopSound(string name, bool startAtBeginning){
        SoundEntry playEntry = staticSounds.Find(item => item.sound.name == name);
        if(!startAtBeginning)
            playEntry.source.time = playEntry.loopStartTime;
        playEntry.source.Play();
        playEntry.source.volume = playEntry.volume;
        playEntry.looping = true;
    }

    public void loopFromTime(string name, float startTime, bool startAtBeginning){
        SoundEntry playEntry = sounds.Find(item => item.sound.name == name);
        if(startAtBeginning)
            playEntry.source.time = startTime;
        playEntry.source.Play();
        playEntry.loopStartTime = startTime;
        playEntry.looping = true;
    }

    public void loopBetweenTimes(string name, float startTime, float endTime, bool startAtBeginning){
        SoundEntry playEntry = sounds.Find(item => item.sound.name == name);
        if(startAtBeginning)
            playEntry.source.time = startTime;
        playEntry.source.Play();
        playEntry.loopStartTime = startTime;
        playEntry.loopEndTime = endTime;
        playEntry.looping = true;
    }

    public static void pauseSound(string name){
        SoundEntry pauseEntry = staticSounds.Find(item => item.sound.name == name);
        pauseEntry.source.Pause();
    }

    public void pauseAllSounds(){
        foreach(SoundEntry sound in sounds){
            sound.source.Pause();
        }
    }

    void Awake() {
        foreach(SoundEntry sound in sounds){
            sound.source = gameObject.AddComponent<AudioSource>() as AudioSource;
            sound.source.clip = sound.sound;
            if(sound.loopEndTime <= 0.0f)
                sound.loopEndTime = sound.source.clip.length;
            sound.looping = false;
        }

        staticSounds = sounds;
    }

    void Update(){
        foreach(SoundEntry sound in sounds){
            if(sound.looping){
                if(!sound.source.isPlaying || sound.source.time >= sound.loopEndTime - 0.1f){
                    sound.source.time = sound.loopStartTime;
                }
                /*if(sound.source.time >= sound.loopStartTime + 5.0f && sound.source.time <= sound.loopEndTime - 5.0f){
                    sound.source.time = sound.loopEndTime - 5.0f;
                }*/
            }
        }
    }
}