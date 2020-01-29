using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{
    void MoveVector(Vector3 vector3);

    void MoveTo(Vector3 vector3);

    void MoveAboveObject(Vector3 vector3, Vector3 objectPosition);

}
