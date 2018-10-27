using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisObjectController : MonoBehaviour
{
    public bool allowRotation = true;
    public bool limitRotation = false;

    private int incrementValue = 1;
    private int rotationValue = 90;

    private float fallTime = 0;
    private float fallSpeed = 1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckUserInput();
	}

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(incrementValue, 0, 0);
            
            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(-incrementValue, 0, 0);
            }
            else
            {
                FindObjectOfType<GameController>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-incrementValue, 0, 0);

            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(incrementValue, 0, 0);
            }
            else
            {
                FindObjectOfType<GameController>().UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || (Time.time - fallTime) >= fallSpeed)
        {
            transform.position += new Vector3(0, -incrementValue, 0);

            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(0, incrementValue, 0);

                FindObjectOfType<GameController>().DeleteRow();

                if (FindObjectOfType<GameController>().CheckAboveGrid(this))
                {
                    FindObjectOfType<GameController>().GameOver();
                }

                enabled = false;
                FindObjectOfType<GameController>().SpawnNextTetrisObject();
            }
            else
            {
                FindObjectOfType<GameController>().UpdateGrid(this);
            }

            fallTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -rotationValue);
                    }
                    else
                    {
                        transform.Rotate(0, 0, rotationValue);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, rotationValue);
                }

                if (!CheckIsValidPosition())
                {
                    if (limitRotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -rotationValue);
                        }
                        else
                        {
                            transform.Rotate(0, 0, rotationValue);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, -rotationValue);
                    }
                }
                else
                {
                    FindObjectOfType<GameController>().UpdateGrid(this);
                }
            }
            
        }
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<GameController>().Round(mino.position);

            if (FindObjectOfType<GameController>().CheckInsideGrid(pos) == false)
            {
                return false;
            }

            if (FindObjectOfType<GameController>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<GameController>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
