using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("PlayMode");
    }

	public void Restart()
    {
        SceneManager.LoadScene("PlayMode");
    }
}
