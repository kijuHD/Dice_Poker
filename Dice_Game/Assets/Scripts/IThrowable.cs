﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable 
{
     void AddForce(float force);
     void AddRandomTorque();
}
