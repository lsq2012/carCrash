using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    //加速音效优先级最低。 没声音播放 可以放加速。在加速播放的时候， 漂移可以抢， 氮气可以抢。 漂移播放的时候，氮气可以抢。在氮气播放的时候， 谁都不能抢。但是播放完后 会让出位置。
    public static SoundManger soundInstance;
    public AudioSource Playersource;
    public AudioSource DashSource;
    public AudioSource OtherStuffAudioSource;
   // int currentPriority;
   // bool canPlay;
   // bool AccWaiting;
  
    [SerializeField]
    private AudioClip S_drift,S_acc,S_Falling,S_Crash,S_Wining,S_bigCrash;
    // Start is called before the first frame update
    void Start()
    {
        soundInstance = this;
    }
    public void DriftSound(bool stat)
    {
      
            loopPlay(S_drift, stat);

        
        

    }
    public void AccSound(bool stat)
    {
        
            loopPlay(S_acc, stat);

        
        
        //canPlay = canbePlay(1);
        /*if (canPlay)
        {
            loopPlay(S_acc, stat);
            currentPriority = 1;
            AccWaiting = false;
        }
        else
        {
            AccWaiting = true;
        }
        if (!stat)
        {
            AccWaiting = false;
        }*/
        
        

    }
    public void DashSound()
    {
     
            DashSource.Play();
            
    }
 


    void loopPlay(AudioClip audioClip,bool stat)
    {
        Playersource.clip = audioClip;
        if (stat)
        {
            Playersource.Play();
        }
        else
        {
            Playersource.Stop();
          
        }
       
    }
 
    
    public void CrashSound()
    {
        OtherStuffAudioSource.clip = S_Crash;
        OtherStuffAudioSource.Play();
    }

    public void FallingSound()
    {
        OtherStuffAudioSource.clip = S_Falling;
        OtherStuffAudioSource.Play();
    }
    public void WiningSound()
    {
        OtherStuffAudioSource.clip = S_Wining;
        OtherStuffAudioSource.Play();
    }
    public void BigCrashSound()
    {
        OtherStuffAudioSource.clip = S_bigCrash;
        OtherStuffAudioSource.Play();
    }
    // Update is called once per frame

}
