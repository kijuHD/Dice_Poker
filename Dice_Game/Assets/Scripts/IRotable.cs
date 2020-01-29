using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRotable 
{
    void RandomRotate();
    void Rotate(float x,float y,float z,float w);

    void Rotate(Quaternion quaternion);
    void RotateToShowResult(int result,Quaternion spawnerQuaternion);
}
