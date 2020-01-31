using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D6 : Dice
{
    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        transform = gameObject.GetComponent<Transform>();

        currentMaterial = gameObject.GetComponent<Renderer>().material;

        AddDirections();
    }

    private void AddDirections()
    {
        directions.Add(Vector3.up);
        values.Add(6);

        directions.Add(Vector3.down);
        values.Add(1);

        directions.Add(Vector3.left);
        values.Add(2);

        directions.Add(Vector3.right);
        values.Add(5);

        directions.Add(Vector3.forward);
        values.Add(4);

        directions.Add(Vector3.back);
        values.Add(3);
    }

    public override void RotateToShowResult(int result, Quaternion spawnerQuaternion)
    {        

        transform.Rotate(0, 90, 0);

        switch (result)
        {
            case 1:
                transform.Rotate(180, 0, 0);

                Move(0f, 0.1f);
                break;
            case 2:
                transform.Rotate(0, 0, -90);

                Move(0f, 0.05f, 0.05f);
                break;
            case 3:
                transform.Rotate(90, 0, 0);

                Move(-0.05f, 0.05f);
                break;
            case 4:
                transform.Rotate(-90, 0, 0);

                Move(0.05f, 0.05f);
                break;
            case 5:
                transform.Rotate(0, 0, 90);

                Move(0f, 0.05f, -0.05f);
                break;
            case 6:
                break;
            default:
                break;
        }
    }
}
