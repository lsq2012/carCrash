using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject PlayerPrefab;
    CarController CarCtrler;
    Vector3 StartPos = new Vector3 (0,0,0);
    int PlayerCode;
    private void Awake()
    {
        if(PlayerPrefab!= null) 
        {
            CarCtrler = GameObject.Instantiate(PlayerPrefab, GameRules.instance.spawnPoint.transform.position, GameRules.instance.spawnPoint.transform.rotation).GetComponent<CarController>();
            CarCtrler.playerInput = this;
            PlayerCode = GameRules.instance.spawnPointI;
            CarCtrler.changeMat(PlayerCode);
            GameRules.instance.players[PlayerCode] = CarCtrler.gameObject;
            GameRules.instance.spawnIplus();
               // CarCtrler = PlayerPrefab.GetComponent<CarController>();
            //transform.parent = CarCtrler.transform;
            
           
        }
        
    }
    public void RegenerateCar()
    {
        CarCtrler.ShutDown();
        //Destroy(CarCtrler.gameObject);
        CarCtrler = GameObject.Instantiate(PlayerPrefab, GameRules.instance.spawnPoint.transform.position, transform.rotation).GetComponent<CarController>();
        GameRules.instance.players[PlayerCode] = CarCtrler.gameObject;
        CarCtrler.playerInput = this;
        CarCtrler.changeMat(PlayerCode);
        GameRules.instance.AddPoint(1 - PlayerCode);
        //¸æËßui¼Ó·Ö
    }
   public void onmove(InputAction.CallbackContext context)
    {
        
        CarCtrler.onMove(context);
    }
    public void OnNitro(InputAction.CallbackContext context)
    {
        CarCtrler.onNitro(context);
    }
    public void OnBrake(InputAction.CallbackContext context)
    {
        CarCtrler.onBrake(context);
    }
}
