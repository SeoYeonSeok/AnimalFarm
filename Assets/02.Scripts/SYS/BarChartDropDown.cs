using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;

public enum TimeUnit
{
    Year,
    Month,
    Week,
    Day
}

public enum Month
{
    January = 1,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December
}

public class BarChartDropDown : MonoBehaviour
{
    public Bar barPrefab;
    public Color[] colors;

    [Header("Time Settings")]
    public TimeUnit timeUnit;
    public Month selectedMonth = Month.January; // Dropdown
    public int whatYear = 2025;  // can set

    List<Bar> bars = new List<Bar>();
    float charHeight;

    [Header("Areas")]
    public TextMeshProUGUI[] areas;

    int[] inputValues;
    string[] generatedLabels;    

    void Start()
    {
        charHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        GenerateInputValues();
        DisplayGraph(inputValues);
    }

    void GenerateInputValues()
    {
        List<int> values = new List<int>();
        List<string> labelTexts = new List<string>();

        switch (timeUnit)
        {
            case TimeUnit.Year:
                for (int i = 1; i <= 12; i++)
                {
                    // Random - Need Change Later
                    values.Add(UnityEngine.Random.Range(5, 100));
                    // Random - Need Change Later

                    labelTexts.Add(i.ToString());
                }
                break;

            case TimeUnit.Month:
                // Need Change Later - DateTime.Now.Year
                int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, (int)selectedMonth);
                // Need Change Later

                for (int i = 1; i <= daysInMonth; i++)
                {
                    // Random - Need Change Later
                    values.Add(UnityEngine.Random.Range(5, 100));
                    // Random - Need Change Later
                }

                for (int i = 1; i <= daysInMonth; i++)
                {
                    if (i == 1 || i == daysInMonth)
                        labelTexts.Add(i.ToString());
                    else
                    {
                        int step = Mathf.RoundToInt((daysInMonth - 1) / 4f);
                        if ((i - 1) % step == 0 && labelTexts.Count(l => l != null) < 4)
                            labelTexts.Add(i.ToString());
                        else
                            labelTexts.Add(null);
                    }
                }
                break;

            case TimeUnit.Week:
                string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                for (int i = 0; i < 7; i++)
                {
                    // Random - Need Change Later
                    values.Add(UnityEngine.Random.Range(5, 100));
                    // Random - Need Change Later

                    labelTexts.Add(weekDays[i]);
                }
                break;

            case TimeUnit.Day:
                for (int i = 0; i < 24; i++)
                {
                    // Random - Need Change Later
                    values.Add(UnityEngine.Random.Range(5, 100));
                    // Random - Need Change Later

                    if (i == 0 || i == 6 || i == 12 || i == 18 || i == 23)
                        labelTexts.Add($"{i:00}:00");
                    else
                        labelTexts.Add(null);
                }
                break;
        }

        inputValues = values.ToArray();
        generatedLabels = labelTexts.ToArray();
    }

    void DisplayGraph(int[] vals)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        int maxValue = vals.Max();
        int adjustedMaxValue = ((maxValue + 24) / 25) * 25;
        SetAreas(adjustedMaxValue);

        for (int i = 0; i < vals.Length; i++)
        {
            Bar newBar = Instantiate(barPrefab);
            newBar.transform.SetParent(transform, false);

            RectTransform rt = newBar.GetComponent<RectTransform>();
            float normalizedValue = ((float)vals[i] / (float)adjustedMaxValue) * 0.95f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, charHeight * normalizedValue);

            newBar.barValue.text = vals[i].ToString();
            newBar.label.text = generatedLabels[i] ?? "";
        }
    }

    public void SetAreas(int maxValue)
    {
        int currMaxValue = 25;
        if (((maxValue / 25) != 1))
        {
            if (maxValue % 25 == 0) currMaxValue = (maxValue / 25) * 25;
            else currMaxValue = ((maxValue / 25) + 1) * 25;
        }

        areas[4].text = currMaxValue.ToString();

        for (int i = 0; i < 4; i++)
        {
            areas[i].text = ((currMaxValue * (i + 1)) / 5).ToString();
        }
    }
}
