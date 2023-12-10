using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    //还是要写一个状态机。翻车的时候应该结束漂移状态的。不然视觉效果上漂移时候翻车会一直漂移。现在的结构不方便结束
    public class CarController : MonoBehaviour
{public LayerMask GroundLayerMask;
    public enum FSM
    {
        None,
        NormalDriving,
        Drifting,
        RollOver,
        Hitted,
        Air,
        Respawning
    }
    FSM State;
    FSM shouldState;
    public Material[] Mat;
    public Color[] buttonSpriteColor = new Color[2];

    bool canbeHitten = true;
   // GameObject CarBody;
    
    [Header("地面控制")]
    public bool isOnGround;//可能需要改进 基于轮胎是
    public float OnGroundForce;
    public float speed;
    public float steerSpeed;
    public float isOnGroundRayDis;
    public float isRollOverRayDis;
  
    public float GroundDiff;
    public float GravityForce;
   public bool rollOver;
    public float FlipSpeed;
   public float brakeMultifyper = 0.9f;
    public float brakeForce = 10f;
    bool ACCorNot = true; 

    //public float brakeTime = 1f;
    bool wantBrake;
    [Header("生命")]
    public float MaxHealth = 100f;
    float Health = 100;
    bool Hitted = false;
    bool spawning = false;
    float spawnTimer = 2;
   public PlayerInputHandler playerInput;
    
    //因为空中应该也可以氮气推进 所以要调一下（变成模块化？）
    [Header("道具")]   
    public float MaxPower = 3;//最大能量
    public float PowerDashSpeed=1;//氮气冲刺的力量
    public float TimetoGainOnePower = 1;//加一格能量需要的漂移时间   
    float Power =1;//当前能量值
    bool wantNitro;//是否想放氮气
    bool startNitro;//开始放氮气


    Vector3 GroundNormal;
    [Header("漂移")]
    public float stopDriftSpeed = 600f;
    public float StopDriftAngle;
    public GameObject DriftSmoke;
   public bool Drifting = false;
    public float DriftAccForce = 50f;


    [Header("车轮")]
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxSteeringAngle;
    float velocitySpeed;
 
    [Header("ui")]
    public GameObject EnergyBarUIPrefab;
    EnergyBar energyBar;
    public GameObject healthBarUIPrefab;
    HealthBar healthBar;
    public SoundManger SM;
    bool canPlayAccSound;
  public  GameObject RollOverNotifyPrefab;

    Rigidbody rb;

    Transform RespawnTrans;
    GameRules GM;
    UIMangement UM;
    Vector2 Mov;
   public GameObject EMprefab;
   EffectManger EM;

    bool canRespawn = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameRules").GetComponent<GameRules>();
        //UM = GameObject.Find("GameRules").GetComponent<UIMangement>();
        EM = GameObject.Instantiate(EMprefab, transform.position, transform.rotation).GetComponent<EffectManger>();
        EM.gameObject.transform.parent = this.gameObject.transform;
        energyBar = GameObject.Instantiate(EnergyBarUIPrefab, transform.position, transform.rotation).GetComponent<EnergyBar>();
        energyBar.TransToFollow = transform;
        healthBar = GameObject.Instantiate(healthBarUIPrefab, transform.position, transform.rotation).GetComponent<HealthBar>();
        healthBar.TransToFollow = transform;
        RollOverNotifyPrefab = GameObject.Instantiate(RollOverNotifyPrefab, transform.position, Quaternion.identity);
        RollOverNotifyPrefab.GetComponent<RollOverNotify>().TransToFollow = transform;
        RollOverNotifyPrefab.SetActive(false);
        SM = transform.Find("SoundManger").GetComponent<SoundManger>();
        State = FSM.NormalDriving;
        RespawnTrans = GameObject.Find("RespawnPoint").transform;
        //CarBodyMat = transform.Find("Mesh_Car_Body").GetComponent<MeshRenderer>().material;

        // transform.Find("Mesh_Car_Body").EnsureComponent<MaterialInstance>()
        // Mat[0] = new Material Mat[0];

        //Invoke("ShutDown", 3f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(rb.angularVelocity);
        //CarBodyMat = Mat[0];
        isOnGround = onTheGround();
        rollOver = RollOver();
        Drifting = IsDrift();
        velocitySpeed = rb.velocity.sqrMagnitude;

        stateJudge();
        switch (State)
        {
            case FSM.NormalDriving:
                State_Normal();
                break;
            case FSM.Drifting:
                State_Drifting();
                break;
            case FSM.RollOver:
                State_RollOver();
                break;
            case FSM.Air:
                State_Air();
                break;
            case FSM.Hitted:
                State_Hitten();
                break;
            case FSM.None:
                State = FSM.NormalDriving;
                break;
            case FSM.Respawning:
                State_Respaw();
                break;
                
        }
        
        Gravity();
        UpdateUI();
        DeBugDrawLine();
       
    }
    void stateJudge()
    {
        if (spawning)
        {
            shouldState = FSM.Respawning;
        }
        if (Hitted)
        {
            shouldState = FSM.Hitted;
        }
        else if (rollOver &&velocitySpeed<100)
        {
            shouldState = FSM.RollOver;
        }
        else if (!isOnGround)
        {
            shouldState = FSM.Air;
        }
        else
       if (Drifting)
        {
            shouldState = FSM.Drifting;
        }
        else
        {
            shouldState = FSM.NormalDriving;
        }
    }
    void State_Normal()
    {
        
            RotateSteering();
            ACC();            
            NitroCtrl();
            Brake();

        
    
        if (State != shouldState)
        {
            State = shouldState;
            SM.AccSound(false);
        }
        if(State == FSM.Drifting)
        {
            EM.Drift(true);
            SM.DriftSound(true);
            energyBar.Gaining = true;
        }
        if(State == FSM.RollOver)
        {
            RollOverNotifyPrefab.SetActive(true);
        }
    }
    void State_Drifting()
    {
        RotateSteering();
        ACC();
        NitroCtrl();
        Brake();
        gainNitro();
        DriftACC();
        if (State != shouldState)
        {
            State = shouldState;

            EM.Drift(false);
            energyBar.Gaining = false;
            SM.DriftSound(false);
        }
        if (State == FSM.RollOver)
        {
            RollOverNotifyPrefab.SetActive(true);
        }

    }
    void State_Air()
    {
        FlipControl();
        //RotateSteering();
        NitroCtrl();
        if (State != shouldState)
        {
            State = shouldState;
            
        }
        if (State == FSM.RollOver)
        {
            RollOverNotifyPrefab.SetActive(true);
        }
        //加入引擎空转音效？
    }
    void State_RollOver()
    {
        FlipControl();
        flipNitroCtrl();       //NitroCtrl();
        if (State != shouldState)
        {
            State = shouldState;
            RollOverNotifyPrefab.SetActive(false);
        }
        
    }
    void State_Hitten()
    {
        
    }
    void State_Respaw()
    {
        spawnTimer -= Time.deltaTime;
        {
            if (spawnTimer < 0)
            {
                spawning = false;
                spawnTimer = 2;
                State = FSM.NormalDriving;

            }
        }
        if (State == FSM.RollOver)
        {
            RollOverNotifyPrefab.SetActive(true);
        }
    }
    private void UpdateUI()
    {
        energyBar.FillAmount = Power/MaxPower;
    }
    




    void Gravity()
    {
      
        rb.AddForce(Vector3.up * -1f * GravityForce);
    }
    void ACC()//利用wheel collider进行的控制
    {
        //ACCorNot = AccOrNot();
        float motor = speed * Mov.y;
        float steering = maxSteeringAngle * Mov.x;

       /* if (ACCorNot&& Mov.y < 0)
        {
            rb.AddForce(rb.velocity * brakeForce * -1f);
            print("diverseBrake");
        }
        else if(!ACCorNot && Mov.y > 0)
        {
            rb.AddForce(rb.velocity * brakeForce * -1f);
            print("diverseBrake");
        }*/

        foreach (AxleInfo axleInfo in axleInfos)
        {
            
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
           /* if (Mathf.Abs(Mov.y) < 0.1)
            {
                axleInfo.leftWheel.motorTorque = 0f;
                axleInfo.rightWheel.motorTorque = 0f;
            }*/
        }
       
       
    }

    void DriftACC()
    {
        if (Mov.y > 0.1)
        {
            rb.AddForce(transform.TransformDirection(Vector3.forward) * DriftAccForce * Mov.y);
            
        }
    }
    void RotateSteering()
    {
      
      
        if (Mov.x != 0)//steer
        {
            rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Mov.x * steerSpeed, transform.rotation.eulerAngles.z));
            Debug.DrawRay(transform.position, GroundNormal * 10, Color.green);

        }
      
    }//转弯和成盒子时候的控制
    
    void gainNitro()
    {
       
            Power += Time.deltaTime / TimetoGainOnePower;
            if (Power > MaxPower)
            {
                Power = MaxPower;
            }
        
    }
    void NitroCtrl()//氮气技能
    {
        
        if (wantNitro && Power >= 1)
        {
            EM.ShowDashindicator();
            if (startNitro)
            {
                Power -= 1;
                rb.velocity = transform.TransformDirection(Vector3.forward) * PowerDashSpeed;
               // rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Mov.x * steerSpeed, transform.rotation.eulerAngles.z));
                EM.Nitro();
                wantNitro = false;
                startNitro = false;
                SM.DashSound();

            }


        }
        else if(wantNitro && Power<1&&startNitro)
        {
            Power += 0.04f;
            wantNitro = false;
            startNitro = false;
        }
    }
    
    void flipNitroCtrl()//氮气技能
    {
        if (Power <= 1)
        {
            Power += Time.deltaTime / TimetoGainOnePower / 3;
        }
       
        if (wantNitro && Power >= 1)
        {
            EM.ShowDashindicator();
            if (startNitro)
            {
                Power -= 1;
                rb.velocity = transform.TransformDirection(Vector3.up) * PowerDashSpeed * -1f;
                EM.Nitro();
                wantNitro = false;
                startNitro = false;
                SM.DashSound();

            }


        }/*
        else if (wantNitro && Power < 1)
        {
            Power += 0.08f;
            wantNitro = false;
            startNitro = false;
        }*/
    }
    void Brake()
    {
        if (wantBrake)
        {
            rb.AddForce(rb.velocity *brakeForce*-1f);
            //rb.velocity = rb.velocity *brakeMultifyper*Time.deltaTime;
           // Mathf.SmoothDamp()

        }
        
    }

    void FlipControl()
    {
        rb.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles.x - Mov.y*FlipSpeed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - Mov.x * FlipSpeed));
       
    }

    
  
    bool onTheGround()
    {
        RaycastHit GroundCheck;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up) * -1, out GroundCheck, isOnGroundRayDis, GroundLayerMask))
        {
            GroundNormal = GroundCheck.normal;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * -1 * GroundCheck.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * -1 * isOnGroundRayDis, Color.red);
            return false;
        }

    }//检测是否在地面
    bool RollOver()
    {
        bool lastFrame = rollOver;
        bool CurrentFrame;

        RaycastHit GroundCheck;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up) * 1, out GroundCheck, isRollOverRayDis, GroundLayerMask))
        {
            CurrentFrame = true;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1 * GroundCheck.distance, Color.yellow);
            
        }
        else
        {
            CurrentFrame = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 1 * isRollOverRayDis, Color.red);
           
        }
        if (CurrentFrame & !lastFrame)//现在翻 上一帧没翻车 那就ui启动 反之ui关闭。 保证只调用一次
        {
           // UM.SetRollOverNotifyVisbility(true);
        }else if (!CurrentFrame & lastFrame)
        {
           // UM.SetRollOverNotifyVisbility(false);
        }

        return CurrentFrame;


    }//检测翻车没
    bool IsDrift()
    {
        
        Vector3 project = Vector3.ProjectOnPlane(rb.velocity, transform.TransformDirection(Vector3.up));//试着用这个作为速度指标？

        //sqr 600 mag 30左右


         if (velocitySpeed > stopDriftSpeed&&isOnGround&& Vector3.Angle(project, transform.TransformDirection(Vector3.forward)) > StopDriftAngle)
         {

                 return true;


         }/*else if (isOnGround && rb.angularVelocity.magnitude > 3f)
        {

        }*/
         else
         {
             return false;
             //Drifting = false;
         }
       




    }//检测漂移没

    bool AccOrNot()
    {
        Vector3 project = Vector3.ProjectOnPlane(rb.velocity, transform.TransformDirection(Vector3.up));
        if(project.x * transform.TransformDirection(Vector3.up).x > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
     

        //if(rb.velocity.x * transform.TransformDirection(Vector3.forward)>0)
    }
    private void OnTriggerEnter(Collider other)
    {
       
      
        if(other.tag == "PlayFallSound")
        {
            SM.FallingSound();
        }
        if (other.tag == "DeathArea")
        {
            GetHitten(10, Vector3.zero, 0,transform.position,"fall");
            Invoke("SmallRespawn", 1f);
            //SmallRespawn();
        }
        if (other.tag == "Respawn")
        {
            RespawnTrans = other.transform;
        }
    }//吃道具
  
  
   
   public void onMove(InputAction.CallbackContext context)
    {
        
        Vector2 Movement = context.ReadValue<Vector2>();
        Mov = Movement;

       
       
    }

    public void onNitro(InputAction.CallbackContext context)
    {
     
        switch (context.phase)
        {
            case InputActionPhase.Started:
                wantNitro = true;
                break;
            
            case InputActionPhase.Canceled:
                startNitro = true;
                //wantNitro = false;
                break;
        }
        
        
    }
    public void onBrake(InputAction.CallbackContext context)
    {
        
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                wantBrake = true;
                break;
            case InputActionPhase.Canceled:
                wantBrake = false;
                break;
        }


    }

    void SmallRespawn()//传送到出生点 并且清空惯性和角动量
    {
        spawning = true;
        if (Power > 33)
        {
            Power = 33f;
        }
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        transform.position = RespawnTrans.position;
        transform.rotation = Quaternion.identity;


        rb.constraints = RigidbodyConstraints.FreezeAll;
        Invoke("RecoverRB", 1);

        State = FSM.Respawning;
        //rb.rotation = RespawnTrans.rotation;//还是不行 总之要停止旋转
        //rb.angularVelocity = new Vector3(0, 0, 0);

    }
    void RecoverRB()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
    void BigRespawn()
    {
        playerInput.RegenerateCar();
        //SmallRespawn();
        //开始 记录所有子物体位置 血量 （FixedJoint是个问题）
    }

    public void GetHitten(float damage,Vector3 Hitdir, float Force,Vector3 happenPos,string type)//carhead ground fall
    {
        if (canbeHitten)
        {
            canbeHitten = false;
            Health -= damage / 3;
            healthBar.FillAmount = Health / 100;
            rb.velocity = new Vector3(0, 0, 0);
            rb.AddForce(Hitdir * Force);
            switch (type)
            {
                case "carhead":
                    SM.CrashSound();
                    EM.CarHeadKnockSpark(happenPos);
                    break;
                case "ground":
                    if (damage <= 15)
                    {
                        SM.CrashSound();
                        
                    }
                    else
                    {
                        SM.BigCrashSound();
                    }
                    EM.CarHitGroundSpark(happenPos);
                    break;
                case "fall":
                    break;
            }

            Power += 0.5f;
            if (Health < MaxHealth / 4)
            {
                EM.showWeakEffect();
            }
            if (Health < 0)
            {
                healthBar.gameObject.SetActive(false);
                energyBar.gameObject.SetActive(false);
                RollOverNotifyPrefab.SetActive(false);
                EM.gameObject.SetActive(false);
                // Destroy(healthBar.gameObject);
                // Destroy(energyBar.gameObject);
                BigRespawn();

                //Health = 100;

            }
            else
            {
                Invoke("canBeHittenAgain", 0.2f);
            }
        }
        
    }
    void canBeHittenAgain()
    {
        canbeHitten = true;
    }
    void DeBugDrawLine()
    {
        Debug.DrawLine(transform.position, transform.position + rb.velocity.normalized * 10, Color.blue);
        Vector3 project = Vector3.ProjectOnPlane(rb.velocity, transform.TransformDirection(Vector3.up));//试着用这个作为速度指标？
        Debug.DrawLine(transform.position, transform.position + project * 1, Color.green);
    }
   public void changeMat(int PlayerCode)
    {
        transform.Find("Mesh_Car_Body").GetComponent<MeshRenderer>().material = Mat[PlayerCode];
        //CarBodyMat = Mat[PlayerCode];
        print("CHANGED" + PlayerCode);
        transform.Find("ButtonDisplay").GetComponent<SpriteRenderer>().color = buttonSpriteColor[PlayerCode];
        
    }
    public void ShutDown()
    {
        //遍历子物体 关掉组件？可能还是直接喂比较好？ 弹跳物体组件得关掉。车灯也得关掉
        if (canRespawn)
        {
            canRespawn = false;
            transform.Find("DieEffectManger").GetComponent<DieEffectManger>().Die();
            this.enabled = false;
            //canRespawn = false
        }
       
    }
}


