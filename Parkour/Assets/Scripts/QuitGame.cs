
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit The Game");
    }
}
