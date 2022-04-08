using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public List< List<Sound> > timeline;

    public Sound[] sounds;

    public static AudioManager instance;

    public enum phases {intro, peaceA, peaceB, fightA, fightB};

    private phases phase;
    private float currentTime;
    private int bpm = 96;
    private float barDuration;
    private bool rifle = false;
    private bool pistol = false;
    private bool shotgun = false;
    private bool minigun = false;

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        phase = phases.intro;
        //DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        timeline = new List< List<Sound> >();
        currentTime = 0f;
        barDuration = 60f / bpm * 4f;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(timeline.Count <= getCurrentBar() || timeline.Count == 0)
        {
            List<Sound> currentBar = new List<Sound>(7);
            
            //start new 4 bar cycle
            if(getCurrentBar() % 4 == 0)
            {
                if(getCurrentBar() == 0)
                {
                    SetPhase(phases.intro);
                }
                if(getCurrentBar() > 0 && phase != phases.fightA)
                {
                    SetPhase(phases.peaceA);
                }
                switch(phase)
                {
                    case phases.intro:    
                        currentBar.Insert(0, findSound("drumsIntro"));
                        break;
                    case phases.peaceA:          
                        currentBar.Insert(0, findSound("drumsPeaceA"));
                        currentBar.Insert(1, findSound("peaceA"));
                        if(pistol)
                            currentBar.Insert(3, findSound("pistolA"));
                        if(shotgun)
                            currentBar.Insert(4, findSound("shotgunA"));
                        if(rifle)
                            currentBar.Insert(5, findSound("rifleA"));
                        if(minigun)
                            currentBar.Insert(6, findSound("minigunA"));
                        break;
                    //end phase
                    case phases.peaceB:          
                        currentBar.Insert(0, findSound("drumsPeaceB"));
                        currentBar.Insert(1, findSound("PeaceB"));
                        break;

                    case phases.fightA:          
                        currentBar.Insert(0, findSound("drumsFightA"));
                        currentBar.Insert(1, findSound("fightA"));
                        if(pistol)
                            currentBar.Insert(3, findSound("pistolA"));
                        if(shotgun)
                            currentBar.Insert(4, findSound("shotgunA"));
                        if(rifle)
                            currentBar.Insert(5, findSound("rifleA"));
                        if(minigun)
                            currentBar.Insert(6, findSound("minigunA"));
                        break;
                    case phases.fightB:          
                        currentBar.Insert(0, findSound("drumsFightB"));
                        currentBar.Insert(1, findSound("fightB"));
                        if(pistol)
                            currentBar.Insert(3, findSound("pistolB"));
                        if(shotgun)
                            currentBar.Insert(4, findSound("shotgunB"));
                        if(rifle)
                            currentBar.Insert(5, findSound("rifleB"));
                        if(minigun)
                            currentBar.Insert(6, findSound("minigunB"));
                        break;  
                }
            }
            timeline.Add(currentBar);
            PlayBar();
        }
    }

    public void SetPistol(bool set)
    {
        pistol = set;
    }
    public void SetRifle(bool set)
    {
        rifle = set;
    }
    public void SetShotgun(bool set)
    {
        shotgun = set;
    }
    public void SetMinigun(bool set)
    {
        minigun = set;
    }
    private int getCurrentBar()
    {
        return (int)Mathf.Floor(currentTime / barDuration);
    }

    public void PlayBar()
    {
        foreach(Sound it in timeline[getCurrentBar()])
        {
            if(it != null)
            {
                it.source.Play();
                Debug.Log("Sound: " + it.name + "is playing");
            }
        }
    }

    public void Play(string name)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        Debug.LogWarning(s.name);
        Debug.Log("Sound: " + s.name + "is playing");
        s.source.Play();
    }
    private Sound findSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return null;
        }
        Debug.LogWarning(s.name);
        return s;
    }

    public void SetPhase(phases phase)
    {
        this.phase = phase;
        Debug.Log("Phase is set to " + phase);

    }

    public void setVolume(string name, float volume)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return;
        }
        s.source.volume = volume;
        Debug.Log("Volume of " + s.name + "is set to" + volume);
    }

/*
    public void Schedule(string name, int bar)
    {
        int currentBar = getCurrentBar();
        int targetBar = currentBar  - currentBar % 4 + 4;
        while(timeline.Count < targetBar)
        {
            timeline.Add(new List<Sound>);
        }
        timeline.Add
    }
*/


    public AudioSource getSoundSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found");
            return null;
        }
        Debug.LogWarning(s.name);
        Debug.Log("Sound: " + s.name + "is playing");
        return s.source;
    }
}
