using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisObjectController : MonoBehaviour
{
    public GameController gameController;

    public bool allowRotation = true;
    public bool limitRotation = false;

    private int incrementValue = 1;
    private int rotationValue = 90;

    private float fallTime = 0;
    private float fallSpeed = 1;

	// Use this for initialization
	void Start ()
    {
        gameController = FindObjectOfType<GameController>();
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
                gameController.UpdateGrid(this);
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
                gameController.UpdateGrid(this);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || (Time.time - fallTime) >= fallSpeed)
        {
            transform.position += new Vector3(0, -incrementValue, 0);

            if (!CheckIsValidPosition())
            {
                transform.position += new Vector3(0, incrementValue, 0);

                gameController.DeleteRow();

                if (gameController.CheckAboveGrid(this))
                {
                    gameController.GameOver();
                }

                enabled = false;
                gameController.SpawnNextTetrisObject();
            }
            else
            {
                gameController.UpdateGrid(this);
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
                    gameController.UpdateGrid(this);
                }
            }
            
        }
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = gameController.Round(mino.position);

            if (gameController.CheckInsideGrid(pos) == false)
            {
                return false;
            }

            if (gameController.GetTransformAtGridPosition(pos) != null && gameController.GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
