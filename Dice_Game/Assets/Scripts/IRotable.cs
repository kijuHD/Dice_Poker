using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotable 
{
    void RandomRotate();
    void RotateToShowResult(int result,Quaternion spawnerQuaternion);
}
