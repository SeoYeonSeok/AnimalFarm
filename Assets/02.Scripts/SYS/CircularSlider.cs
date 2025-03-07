using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CircularSlider : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("UI Elements")]
    public RectTransform handle;
    public RectTransform centerPoint;
    public Canvas canvas;
    public TextMeshProUGUI mentText;
    public TextMeshProUGUI valueText;
    public Image fillImage;
    public Button startButton;
    public TextMeshProUGUI btnText;
    public GameObject[] objsDisableTimer;

    public int maxTime = 120; // 최대 시간 (120분)
    public int minTime = 10;  // 최소 시간 (10분)
    private float radius;
    private float maxAngle = 360f;
    private float currentAngle = 0f;
    private bool isTimerActive = false;
    private float currentTime;
    private bool under10Min = false;

    private Coroutine corou_Timer;

    [Header("Other Scripts")]
    public GameMgr gameMgr;
    public GameObject debugBtn;

    void Start()
    {
        radius = Vector2.Distance(handle.anchoredPosition, centerPoint.anchoredPosition);

        // 초기화: 10:00에 해당하는 FillAmount 값 설정
        float initialFillAmount = 10f / maxTime; // 10분 -> 0.0833 (8.33%)
        float initialAngle = initialFillAmount * maxAngle;

        // 핸들의 초기 위치와 FillAmount 업데이트
        //UpdateHandlePosition(initialAngle);
        fillImage.fillAmount = initialFillAmount;
        currentTime = 10 * 60; // 10분을 초로 설정

        UpdateValueText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isTimerActive) UpdateHandlePosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTimerActive) UpdateHandlePosition(eventData);
    }

    private void UpdateHandlePosition(PointerEventData eventData)
    {
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(centerPoint, eventData.position, canvas.worldCamera, out localMousePosition);

        Vector2 direction = (localMousePosition - (Vector2)centerPoint.anchoredPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90;
        if (angle < 0) angle += 360; // 음수 각도를 보정

        // 현재 각도를 반대로 변환
        currentAngle = angle;

        // FillAmount 계산을 반대로 설정
        fillImage.fillAmount = 1 - (currentAngle / 360f);

        // 5분 단위로 스냅 적용
        float rawTime = ((fillImage.fillAmount) * maxTime) * 60; // 초 단위 변환
        float snappedTime = Mathf.Round(rawTime / 300f) * 300f;  // 5분(300초) 단위로 반올림
        snappedTime = Mathf.Clamp(snappedTime, minTime * 60, maxTime * 60); // 최소/최대 제한

        // FillAmount 다시 설정 (스냅된 시간 기준)
        fillImage.fillAmount = (snappedTime / 60f) / maxTime;

        // 핸들 위치 업데이트
        currentAngle = (1 - fillImage.fillAmount) * 360f; // FillAmount 기반 각도 재계산


        // 핸들 위치 업데이트
        Vector2 newPosition = (Vector2)centerPoint.anchoredPosition +
            new Vector2(Mathf.Cos((currentAngle + 90) * Mathf.Deg2Rad),
                        Mathf.Sin((currentAngle + 90) * Mathf.Deg2Rad)) * radius;
        handle.anchoredPosition = newPosition;

        // 시간 값 계산 (초 단위) - FillAmount 반전 적용
        currentTime = snappedTime;


        UpdateValueText();
    }


    private void UpdateValueText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        valueText.text = $"{minutes:00}:{seconds:00}";

        if (currentTime <= 0) EndTimer(false);
    }

    public void ActivateTimer()
    {        
        if (!isTimerActive)
        {            
            DisableObjsWhileTimer(true);
            isTimerActive = true;            
            corou_Timer = StartCoroutine(TimerCountdown());
            TimerTextChng();

            gameMgr.RemadeEgg();
            gameMgr.DoIdleEggs(true);
            gameMgr.SaveMinutes((int)Mathf.FloorToInt(currentTime / 60));
            gameMgr.sideUIOnBtn.SetActive(false);
            debugBtn.SetActive(true);            
        }
        else
        {
            gameMgr.sideUIOnBtn.SetActive(true);
            EndTimer(true);
        }        
    }

    private void DisableObjsWhileTimer(bool flag)
    {
        if (flag == true)
            objsDisableTimer[0].SetActive(false);
        else
            objsDisableTimer[0].SetActive(true);

        /*
        if (flag == true)
            for (int i = 0; i < objsDisableTimer.Length; i++) objsDisableTimer[i].SetActive(false);
        else
            for (int i = 0; i < objsDisableTimer.Length; i++) objsDisableTimer[i].SetActive(true);
        */
    }

    private IEnumerator TimerCountdown()
    {
        under10Min = false;
        while (isTimerActive)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            if (under10Min == false) CheckTime();
            UpdateValueText();
            fillImage.fillAmount = (currentTime / 60f) / maxTime;            
        }
    }

    private void TimerTextChng()
    {
        if (btnText.text == "Timer Start") btnText.text = "Timer end";
        else btnText.text = "Timer Start";
    }

    private void EndTimer(bool flag)
    {
        // 타이머 상태 초기화
        isTimerActive = false;

        if (corou_Timer != null)
        {
            StopCoroutine(corou_Timer);
            corou_Timer = null; // 참조 제거
        }

        // 초기 상태로 리셋 (10:00)
        currentTime = minTime * 60;
        fillImage.fillAmount = (float)minTime / maxTime;
        currentAngle = (1 - fillImage.fillAmount) * 360f;

        // 핸들 위치 재설정
        Vector2 newPosition = (Vector2)centerPoint.anchoredPosition +
            new Vector2(Mathf.Cos((currentAngle + 90) * Mathf.Deg2Rad),
                        Mathf.Sin((currentAngle + 90) * Mathf.Deg2Rad)) * radius;
        handle.anchoredPosition = newPosition;

        // UI 업데이트        
        TimerTextChng();
        UpdateValueText();        
        AfterEndTimer(flag);
        DisableObjsWhileTimer(false);

        gameMgr.DoIdleEggs(false);
        gameMgr.DoShakeEggs(false);        
    }

    private void AfterEndTimer(bool flag)
    {
        if (flag)
        {
            mentText.text = "Shame On You";
            gameMgr.MakeNewAnimal(0);
        }
        else
        {
            mentText.text = "yo wassup";
            int anim = Random.Range(1, gameMgr.animals.Length);
            Debug.Log("yo " + anim);
            gameMgr.MakeNewAnimal(anim);            
        }

        debugBtn.SetActive(false);
    }

    private void CheckTime()
    {
        if (currentTime < 600)
        {
            under10Min = true;
            gameMgr.DoShakeEggs(true);
        }
    }
}