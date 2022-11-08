using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreCounter : MonoBehaviour
{
    public Text ScoreText;
    private float Score;
    // Start is called before the first frame update
    void Start()
    {
        Score = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        Score = (Score - 40 * Time.deltaTime);
        if(Score <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        ScoreText.text = Score.ToString("0");
        
    }
}
