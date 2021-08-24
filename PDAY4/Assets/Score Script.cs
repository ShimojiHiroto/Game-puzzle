using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "Score:" + score.ToString();
    }

    public void AddPoint (int point) 
    {
        score = score + point;
        GetComponent<Text>().text = "Score:" + score.ToString();
    }
   
}
