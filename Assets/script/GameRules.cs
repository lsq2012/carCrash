using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameRules : MonoBehaviour
{
    public bool isTutrial = false;
    int[] PlayerRoundWin = { 0,0};
    public Text[] PlayerRoundWinText;
    //public Text Player2RoundWin;
    public Text PlayerxWin;
   public GameObject[] spawnPoints;
   public GameObject spawnPoint;
    public static GameRules instance = null;
   public int spawnPointI = 0;
   public GameObject[] players = new GameObject[2];
    public rgbBar [] LightBars = new rgbBar[4];
    AudioSource SS;
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }else if(instance != null)
        {
            Destroy(gameObject);
        }
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        spawnPoint = spawnPoints[spawnPointI];
        SS = GetComponent<AudioSource>();
    }
    private void Start()
    {
        
        PlayerInputManager.instance.JoinPlayer(0, -1, "KB+M");
     PlayerInputManager.instance.JoinPlayer(1, -1, "GamePad");
        if (!isTutrial)
        {
            PlaySound(7);
        }
           

        
        
       
    }

   public void PlaySound(float duration)
    {
        if (!isTutrial)
        {
            SS.volume = 1;
            SS.Play();
            Invoke("stopSound", duration);

        }
        
    }
    void stopSound()
    {
        StartCoroutine(SoundFade());
    }
    IEnumerator SoundFade()
    {
        for(float alpha = 1f; alpha >= 0; alpha -= 0.02f)
        {
            SS.volume = alpha;
            if (alpha <= 0.01)
            {
                SS.Stop();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void spawnIplus()
    {
        spawnPointI += 1;
        if (spawnPointI > 1)
        {
            spawnPointI = 1;
        }
        spawnPoint = spawnPoints[spawnPointI];
    }

    public void LightBarShine()
    {
        for(int i = 0; i< LightBars.Length; i++)
        {
            LightBars[i].startFlashing();
        }
    }
    public void AddPoint(int PlayerCode)
    {
        if (!isTutrial)
        {
            LightBarShine();
            PlayerRoundWin[PlayerCode] += 1;
            PlayerRoundWinText[PlayerCode].text = PlayerRoundWin[PlayerCode].ToString();
            PlayerRoundWinText[PlayerCode].gameObject.GetComponent<Animation>().Play();
            if (PlayerRoundWin[PlayerCode] >= 2)
            {
                PlayerxWin.gameObject.SetActive(true);
                PlayerxWin.text = ("P " + PlayerCode + 1 + " Win!!!");
                PlaySound(8f);
                Camera.main.transform.gameObject.GetComponent<CameraCtrl>().Wining = true;
                Camera.main.transform.gameObject.GetComponent<CameraCtrl>().WinManTrans = players[PlayerCode].transform;
                Invoke("RestartLevel", 9f);
            }
            else
            {
                PlaySound(3f);
            }
        }
        
    }
     void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /*void OnPlayerJoined(PlayerInput playerInput)
    {

    }
    public void ReturnPos()
    {
        
    }*/
}
