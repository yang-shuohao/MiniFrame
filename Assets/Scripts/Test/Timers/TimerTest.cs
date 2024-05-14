
using TMPro;
using UnityEngine;

public class TimerTest : MonoBehaviour
{
    private TMP_Text countDownText;

    private float totalSeconds = 60 * 60 * 24;

    private DurationTimer durationTimer;

    private void Start()
    {
        countDownText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCountDonw();
        }

        if(durationTimer != null)
        {
            if(!durationTimer.HasElapsed())
            {
                durationTimer.Update();

                countDownText.text = durationTimer.GetLeftTimeString();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void StartCountDonw()
    {
        durationTimer = new DurationTimer(totalSeconds);
    }
}
