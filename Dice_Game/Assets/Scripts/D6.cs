using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D6 : Dice, IResult, IThrowable, IMoveable, IRotable
{
    public Rigidbody rigidbody;
    public Transform transform;

    public bool interactable;
    private bool selected;

    Material currentMaterial;
    public Material originMaterial;
    public Material outlineMaterial;
    

    void Awake()
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

        currentMaterial = gameObject.GetComponent<Renderer>().material;
        interactable = false;
        selected = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetResult(Vector3 referenceVectorUp, float epsilonDeg = 5f)
    {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);

        // Find smallest difference to object space direction
        float min = float.MaxValue;
        int mostSimilarDirectionIndex = -1;
        for (int i = 0; i < directions.Count; ++i)
        {
            float a = Vector3.Angle(referenceObjectSpace, directions[i]);
            if (a <= epsilonDeg && a < min)
            {
                min = a;
                mostSimilarDirectionIndex = i;
            }
        }
        // -1 as error code for not within bounds
        return (mostSimilarDirectionIndex >= 0) ? values[mostSimilarDirectionIndex] : -1;
    }

    public void AddForce(float force)
    {
        rigidbody.AddForce(transform.right * force);
    }

    public void AddRandomTorque()
    {
        rigidbody.AddTorque(Random.Range(100, 200), Random.Range(100, 200), Random.Range(100, 200));
    }

    public void MoveVector(Vector3 vector3)
    {
        transform.position +=  vector3;
    }

    public void MoveTo(Vector3 vector3)
    {
        transform.position = vector3;
    }

    public void MoveAboveObject(Vector3 vector3, Vector3 objectPosition)
    {
        transform.position = objectPosition + vector3;
    }

    public void RandomRotate()
    {
        transform.rotation = new Quaternion(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160), 0);
    }

    public void Rotate(float x, float y, float z, float w)
    {
        transform.localRotation = new Quaternion(x, y, z, w);
        
    }

    public void Rotate(Quaternion quaternion)
    {
        transform.rotation = quaternion;
    }

    public void RotateToShowResult(int result, Quaternion spawnerQuaternion)
    {
        /*
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
        */

        transform.Rotate(0, 90, 0);

        switch (result)
        {
            case 1:
                transform.Rotate(180,0,0);         
               
                Move(0f,0.1f);
                break;
            case 2:
                transform.Rotate(0, 0, -90);

                Move(0f, 0.05f,0.05f);
                break;
            case 3:
                transform.Rotate(90, 0, 0);

                Move(-0.05f,0.05f);
                break;
            case 4:
                transform.Rotate(-90, 0, 0);

                Move(0.05f, 0.05f);
                break;
            case 5:
                transform.Rotate(0, 0, 90);

                Move(0f, 0.05f,-0.05f);
                break;
            case 6:
                break;
            default:
                break;
        }
       // Debug.Log("Rotate: " + quaternion + "for RESULT: " + result);
       // Rotate(quaternion);
    }

    private void Move(float x = 0f, float y = 0f, float z = 0f)
    {
        Vector3 pos = transform.position;
        x += pos.x;
        y += pos.y;
        z += pos.z;

        pos.x = x;
        pos.y = y;
        pos.z = z;

        transform.position = pos;
    }

    void OnMouseOver()
    {
        //currentMaterial = outlineMaterial;
        if (interactable)
        {
            currentMaterial.color = Color.red;

            if (Input.GetMouseButtonDown(0))
            {
                if (!selected)
                {                    
                    selected = true;
                }
                else
                {
                    selected = false;
                    currentMaterial.color = originMaterial.color;
                }
                
            }
                
            //Debug.Log("mouse over");
        }
        
    }
    void OnMouseExit()
    {
        //currentMaterial = originMaterial;
        if (interactable && !selected)
        {
            currentMaterial.color = originMaterial.color;
           // Debug.Log("mouse exit");
        }
        
    }

    public void Select()
    {
        selected = true;
        currentMaterial.color = Color.red;
    }

    public void UnSelect()
    {
        selected = false;
        currentMaterial.color = originMaterial.color;
    }

    public bool IsSelected()
    {
        return selected;
    }
}
