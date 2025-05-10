using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TimerBtnHoldMinus : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CircularSlider circularSlider;
    public float curTime = 0;

    private bool isHolding = false;
    private float holdStartTime = 0f;
    private float nextTickTime = 0f;

    private float holdTime = 0f;
    private Coroutine holdCoroutine;

    private void CurTimeSetup()
    {
        curTime = circularSlider.GetCurrentTime();
        Debug.Log(curTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdStartTime = Time.time;
        nextTickTime = Time.time + 1f; // 최소 1초 이상 눌러야 반응
        StartCoroutine(HoldIncreaseCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTime = 0f;

        if (holdCoroutine != null)
            StopCoroutine(holdCoroutine);
    }

    private IEnumerator HoldIncreaseCoroutine()
    {
        CurTimeSetup();
        while (isHolding)
        {
            float heldDuration = Time.time - holdStartTime;

            float interval = 1; // default: no increment in first second

            if (heldDuration >= 3f)
                interval = 0.0625f; // 16 per second
            else if (heldDuration >= 2f)
                interval = 0.125f; // 8 per second
            else if (heldDuration >= 1f)
                interval = 0.25f; // 4 per second
            else
                interval = -1f;

            if (interval > 0f && circularSlider.GetCurrentTime() > 600 && Time.time >= nextTickTime)
            {
                curTime -= 60.0f;
                circularSlider.SetCurrentTime(curTime);
                circularSlider.UpdateValueText();
                nextTickTime = Time.time + interval;
            }

            yield return null; // wait without increment until threshold reached
        }
    }
}
