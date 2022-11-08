using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
}
