using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattery
{
    //Fields
    float _currentBattery;
    float _currentMaxBattery;
    float _batteryRegenSpeed;
    bool _pauseBatteryRegen = false;

    //Properties
    public float Battery
    {
        get 
        {
            return _currentBattery;
        }
        set
        {
            _currentBattery = value;
        }
    }

    public float MaxBattery
    {
        get 
        {
            return _currentMaxBattery;
        }
        set
        {
            _currentMaxBattery = value;
        }
    }

        public float BatteryRegenSpeed
    {
        get 
        {
            return _batteryRegenSpeed;
        }
        set
        {
            _batteryRegenSpeed = value;
        }
    }

    public bool PauseBatteryRegen
    {
        get 
        {
            return _pauseBatteryRegen;
        }
        set
        {
            _pauseBatteryRegen = value;
        }
    }

    //Constructure
    public UnitBattery(float battery, float maxBattery, float batteryRegenSpeed, bool pauseBatteryRegen)
    {
        _currentBattery = battery;
        _currentMaxBattery = maxBattery;
        _batteryRegenSpeed = batteryRegenSpeed;
        _pauseBatteryRegen = pauseBatteryRegen;
    }

    //Methods
    public void UseBattery(float batteryAmount)
    {
        if (_currentBattery > 0)
        {
            _currentBattery -= batteryAmount * Time.deltaTime;
        }
    }

    public void RegenBattery()
    {
        if (_currentBattery < _currentMaxBattery && !_pauseBatteryRegen)
        {
            _currentBattery += _batteryRegenSpeed * Time.deltaTime;
        }
    }
}
