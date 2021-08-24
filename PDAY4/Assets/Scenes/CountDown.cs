using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CountDown : MonoBehaviour
{
    private float time = 60.0f;
    public Text timerText;
    public bool isTimeUp;

    // Start is called before the first frame update
    void Start()
    {
        isTimeUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(0 < time)
        {
            time -= Time.deltaTime;
            timerText.text = time.ToString("F1");
        }
        else if(time < 0)
        {
            isTimeUp = true;
        }
    }
}
