using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dice : MonoBehaviour, IResult, IThrowable, IMoveable, IRotable
{
    // Start is called before the first frame update
    public List<Vector3> directions;
    public List<int> values;

    public Rigidbody rigidbody;
    public Transform transform;

    public bool interactable;
    private bool selected;

    public Material currentMaterial;
    public Material originMaterial;
    public Material outlineMaterial;

    void Awake()
    {
        

        interactable = false;
        selected = false;

             
    }

    #region IResult
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
    #endregion

    #region IThrowable
    public void AddForce(float force)
    {
        rigidbody.AddForce(transform.right * force);
    }
    public void AddRandomTorque()
    {
        rigidbody.AddTorque(Random.Range(100, 200), Random.Range(100, 200), Random.Range(100, 200));
    }
    #endregion

    #region IMoveable

    public void Move(float x = 0f, float y = 0f, float z = 0f)
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

    public void Move(Vector3 vector3)
    {
        transform.position += vector3;
    }

    public void MoveTo(Vector3 vector3)
    {
        transform.position = vector3;
    }

    public void MoveFromObject(Vector3 vector3, Vector3 objectPosition)
    {
        transform.position = objectPosition + vector3;
    }

    #endregion

    #region IRotable
    public void RandomRotate()
    {
        transform.rotation = new Quaternion(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160), 0);
        //transform.Rotate(Random.Range(0, 160), Random.Range(0, 160), Random.Range(0, 160));
    }

    public abstract void RotateToShowResult(int result, Quaternion spawnerQuaternion);


    #endregion

    #region ISelectable
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

    #endregion

    #region Mouse input methods
    void OnMouseOver()
    {
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

    #endregion
}
