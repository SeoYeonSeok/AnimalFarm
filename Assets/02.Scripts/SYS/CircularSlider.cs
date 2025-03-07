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

    public int maxTime = 120; // �ִ� �ð� (120��)
    public int minTime = 10;  // �ּ� �ð� (10��)
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

        // �ʱ�ȭ: 10:00�� �ش��ϴ� FillAmount �� ����
        float initialFillAmount = 10f / maxTime; // 10�� -> 0.0833 (8.33%)
        float initialAngle = initialFillAmount * maxAngle;

        // �ڵ��� �ʱ� ��ġ�� FillAmount ������Ʈ
        //UpdateHandlePosition(initialAngle);
        fillImage.fillAmount = initialFillAmount;
        currentTime = 10 * 60; // 10���� �ʷ� ����

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
        if (angle < 0) angle += 360; // ���� ������ ����

        // ���� ������ �ݴ�� ��ȯ
        currentAngle = angle;

        // FillAmount ����� �ݴ�� ����
        fillImage.fillAmount = 1 - (currentAngle / 360f);

        // 5�� ������ ���� ����
        float rawTime = ((fillImage.fillAmount) * maxTime) * 60; // �� ���� ��ȯ
        float snappedTime = Mathf.Round(rawTime / 300f) * 300f;  // 5��(300��) ������ �ݿø�
        snappedTime = Mathf.Clamp(snappedTime, minTime * 60, maxTime * 60); // �ּ�/�ִ� ����

        // FillAmount �ٽ� ���� (������ �ð� ����)
        fillImage.fillAmount = (snappedTime / 60f) / maxTime;

        // �ڵ� ��ġ ������Ʈ
        currentAngle = (1 - fillImage.fillAmount) * 360f; // FillAmount ��� ���� ����


        // �ڵ� ��ġ ������Ʈ
        Vector2 newPosition = (Vector2)centerPoint.anchoredPosition +
            new Vector2(Mathf.Cos((currentAngle + 90) * Mathf.Deg2Rad),
                        Mathf.Sin((currentAngle + 90) * Mathf.Deg2Rad)) * radius;
        handle.anchoredPosition = newPosition;

        // �ð� �� ��� (�� ����) - FillAmount ���� ����
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
        // Ÿ�̸� ���� �ʱ�ȭ
        isTimerActive = false;

        if (corou_Timer != null)
        {
            StopCoroutine(corou_Timer);
            corou_Timer = null; // ���� ����
        }

        // �ʱ� ���·� ���� (10:00)
        currentTime = minTime * 60;
        fillImage.fillAmount = (float)minTime / maxTime;
        currentAngle = (1 - fillImage.fillAmount) * 360f;

        // �ڵ� ��ġ �缳��
        Vector2 newPosition = (Vector2)centerPoint.anchoredPosition +
            new Vector2(Mathf.Cos((currentAngle + 90) * Mathf.Deg2Rad),
                        Mathf.Sin((currentAngle + 90) * Mathf.Deg2Rad)) * radius;
        handle.anchoredPosition = newPosition;

        // UI ������Ʈ        
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