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

    public AudioClip tetrisObjectLandSound;
    private AudioSource audioSource;

    [Header("Scoring")]
    public int tetrisObjectScore = 100;
    private float scoreTime;

    // Use this for initialization
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update()
    {
        CheckUserInput();
        UpdateTetrisObjectScore();
    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || (Time.time - fallTime) >= fallSpeed)
        {
            MoveDown();
            fallTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
            
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            FastDrop();
        }
    }

    void MoveRight()
    {
        transform.position += new Vector3(incrementValue, 0, 0);

        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(-incrementValue, 0, 0);
        }
        else
        {
            PlayAudio(tetrisObjectLandSound);
            gameController.UpdateGrid(this);
        }
    }

    void MoveLeft()
    {
        transform.position += new Vector3(-incrementValue, 0, 0);

        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(incrementValue, 0, 0);
        }
        else
        {
            PlayAudio(tetrisObjectLandSound);
            gameController.UpdateGrid(this);
        }
    }

    void MoveDown()
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
            gameController.SpawnNextTetrisObject();
            GameController.currentScore += tetrisObjectScore;
            enabled = false;
        }
        else
        {
            gameController.UpdateGrid(this);

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                PlayAudio(tetrisObjectLandSound);
            }
        }
    }

    void Rotate()
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
                PlayAudio(tetrisObjectLandSound);
                gameController.UpdateGrid(this);
            }
        }
    }

    void FastDrop()
    {
        while (CheckIsValidPosition())
        {
            transform.position += new Vector3(0, -incrementValue, 0);
        }

        transform.position += new Vector3(0, incrementValue, 0);
        PlayAudio(tetrisObjectLandSound);
        gameController.UpdateGrid(this);
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

    void UpdateTetrisObjectScore()
    {
        if (scoreTime < 1)
        {
            scoreTime += Time.deltaTime;
        }
        else
        {
            scoreTime = 0;
            tetrisObjectScore = Mathf.Max(tetrisObjectScore - 10, 0);
        }
    }

    void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
