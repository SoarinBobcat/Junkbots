using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar _healthbar;
    [SerializeField] BatteryBar _batterybar;

    void Start()
    {
        
    }


    void Update()
    {   
        //Health input
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            PlayerTakeDmg(20);
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            PlayerHeal(10);
        }

        //Battery stamina input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerUseBattery(60f);
        }
        else
        {
            PlayerRegenBattery();
        }
    }

    private void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    public void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        _healthbar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

      private void PlayerUseBattery(float batteryAmount)
    {
        GameManager.gameManager._playerBattery.UseBattery(batteryAmount);
        _batterybar.SetBattery(GameManager.gameManager._playerBattery.Battery);
    }

        private void PlayerRegenBattery()
    {
        GameManager.gameManager._playerBattery.RegenBattery();
        _batterybar.SetBattery(GameManager.gameManager._playerBattery.Battery);
    }
}
