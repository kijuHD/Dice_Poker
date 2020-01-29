using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResult 
{
    int GetResult(Vector3 referenceVectorUp, float epsilonDeg);
}
