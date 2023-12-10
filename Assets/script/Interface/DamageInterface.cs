using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DamageInterface 
{
   // Vector3 impluseDir { get; }
   // float CurrentSpeed { get; }
    void CalculateDamage(float damage,Vector3 impluseDir, float Force,GameObject whoDidThis,Vector3 HappenPos);

}
