using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightTraficController : MonoBehaviour
{
    public int value = 0;
    public float[] timeGYR;
    public TextMeshProUGUI txtTime;
    public GameObject goLight;
    Color32[] colors = new Color32[]
{
    new Color32(0, 255, 0, 255),     // Green
    new Color32(255, 255, 0, 255),   // Yellow
    new Color32(255, 0, 0, 255)      // Red
};
    public GameObject boxCollider;
    float _time;
    void Start()
    {
        SetValue();
    }
    void SetValue()
    {
        _time = timeGYR[value];
        goLight.GetComponent<SpriteRenderer>().color = colors[value];
        txtTime.color = colors[value];
    }
    // Update is called once per frame
    void Update()
    {
        if(_time >= 0)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            value = (value + 1)%timeGYR.Length;
            SetValue();
        }

        txtTime.text = ((int)_time).ToString();
    }
}
