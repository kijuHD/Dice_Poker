using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{

    void Move(float x = 0f, float y = 0f, float z = 0f);
    void Move(Vector3 vector3);

    void MoveTo(Vector3 vector3);

    void MoveFromObject(Vector3 vector3, Vector3 objectPosition);

}
