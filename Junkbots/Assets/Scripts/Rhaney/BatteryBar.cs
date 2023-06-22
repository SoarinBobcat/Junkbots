using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{
    Slider _batterySlider;

    private void Start()
    {
        _batterySlider = GetComponent<Slider>();
    }

    public void SetMaxBattery(float maxBattery)
    {
        _batterySlider.maxValue = maxBattery;
        _batterySlider.value = maxBattery;
    }

    public void SetBattery(float battery)
    {
        _batterySlider.value = battery;
    }
}
