using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    //������Ч���ȼ���͡� û�������� ���Էż��١��ڼ��ٲ��ŵ�ʱ�� Ư�ƿ������� ������������ Ư�Ʋ��ŵ�ʱ�򣬵������������ڵ������ŵ�ʱ�� ˭�������������ǲ������ ���ó�λ�á�
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
