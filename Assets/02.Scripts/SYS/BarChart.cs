using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public enum Timed
{
    Year,
    Month,
    Week,
    Day
}

public class BarChart : MonoBehaviour
{
    public Bar barPrefab;
    public int[] inputValues;
    public string[] labels;
    public Color[] colors;

    List<Bar> bars = new List<Bar>();

    float charHeight;

    [Header("Areas")]
    public TextMeshProUGUI[] areas;

    [Header("Time Unit Dropdown")]
    public Timed timeUnit; // 드롭다운으로 나타날 항목

    void Start()
    {
        charHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        //float[] values = { 0.1f, 0.2f, 0.7f };
        //DisplayGraph(values);

        DisplayGraph(inputValues);
    }

    void DisplayGraph(int[] vals)
    {
        int maxValue = vals.Max();
        SetAreas(maxValue);

        int adjustedMaxValue = ((maxValue + 24) / 25) * 25;
       
        for (int i = 0 ; i < vals.Length ; i++)
        {
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform, false);
            
            // Size Bar
            RectTransform rt = newBar.GetComponent<RectTransform>();

            //float normalizedValue = ((float)vals[i] / (float)maxValue) * 0.95f;
            float normalizedValue = ((float)vals[i] / (float)adjustedMaxValue) * 0.95f;            
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, charHeight * normalizedValue);
            Debug.Log(normalizedValue + rt.sizeDelta.y);
            //newBar.bar.color = colors[i % colors.Length];

            // Set Label
            /*
            if (labels.Length <= i)
            {
                newBar.label.text = "UNO";
            }
            else
            {
                newBar.label.text = labels[i];
            }
            newBar.barValue.text = vals[i].ToString();            
            */
            newBar.label.text = (i + 1).ToString();
            newBar.barValue.text = inputValues[i].ToString();

            // Set Value Label
            /*
            if (rt.sizeDelta.y < 30f)
            {
                newBar.barValue.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                newBar.barValue.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            } 
            */
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
