using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    [Header("Scoring")]
    public int oneFullRow = 30;
    public int twoFullRows = 100;
    public int threeFullRows = 350;
    public int fourFullRows = 1200;

    [Header("Display")]
    public Text scoreText;
    public Text cleanText;

    [HideInInspector]public static int currentScore = 0;       
    private int fullRowsCountPerTurn = 0;
    private int totalCleanRows = 0;

    [Header("Audio")]
    public AudioClip clearRowSound;
    private AudioSource audioSource;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        scoreText.text = currentScore.ToString();
        SpawnNextTetrisObject();
	}

    void Update()
    {
        UpdateScore();
        UpdateTextDisplay();
    }

    public void SpawnNextTetrisObject()
    {
        GameObject nextTetromino = Instantiate(Resources.Load(GetRandomTetrisObject(), typeof(GameObject)), new Vector2(gridWidth / 2, gridHeight), Quaternion.identity) as GameObject;
    }

    public void UpdateGrid(TetrisObjectController tetrisObject)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetrisObject.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach (Transform brick in tetrisObject.transform)
        {
            Vector2 pos = Round(brick.position);

            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = brick;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public bool IsFullRowAt(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        fullRowsCountPerTurn++;
        return true;
    }

    public void DeleteRowAt(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            PlayAudio(clearRowSound);

            grid[x, y] = null;
        }
        totalCleanRows++;
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; i++)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (IsFullRowAt(y))
            {
                DeleteRowAt(y);
                MoveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    public bool CheckInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public bool CheckAboveGrid(TetrisObjectController tetromino)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            foreach (Transform brick in tetromino.transform)
            {
                Vector2 pos = Round(brick.position);

                if (pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    string GetRandomTetrisObject()
    {
        int randomTetrisObject = Random.Range(1, 7);
        string randomTetrisObjectName = "Prefabs/J";

        switch (randomTetrisObject)
        {
            case 1:
                randomTetrisObjectName = "Prefabs/I";
                break;
            case 2:
                randomTetrisObjectName = "Prefabs/J";
                break;
            case 3:
                randomTetrisObjectName = "Prefabs/L";
                break;
            case 4:
                randomTetrisObjectName = "Prefabs/S";
                break;
            case 5:
                randomTetrisObjectName = "Prefabs/Square";
                break;
            case 6:
                randomTetrisObjectName = "Prefabs/T";
                break;
            case 7:
                randomTetrisObjectName = "Prefabs/Z";
                break;
        }
        //Debug.Log(randomTetrisObjectName);
        return randomTetrisObjectName;
    }

    public void UpdateScore()
    {
        if (fullRowsCountPerTurn > 0)
        {
            switch (fullRowsCountPerTurn)
            {
                case 1:
                    currentScore += oneFullRow;
                    break;
                case 2:
                    currentScore += twoFullRows;
                    break;
                case 3:
                    currentScore += threeFullRows;
                    break;
                case 4:
                    currentScore += fourFullRows;
                    break;
            }

            fullRowsCountPerTurn = 0;
        }
    }

    public void UpdateTextDisplay()
    {
        scoreText.text = currentScore.ToString();
        cleanText.text = totalCleanRows.ToString();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
