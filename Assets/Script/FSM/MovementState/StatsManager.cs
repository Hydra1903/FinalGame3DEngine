﻿using UnityEngine;
using UnityEngine.TextCore.Text;

public class StatsManager : MonoBehaviour
{
    public BaseCharacter Character1;
    private MovementStateManager stateManager;
    void Start()
    {
        Character1 = new BaseCharacter(100,100,25,25);
        stateManager = FindAnyObjectByType<MovementStateManager>();
    }
    void Update()
    {
        if (stateManager.currentState == stateManager.Run)
        {
            ManaUsing();
        }
        else
        {
            ManaRecovery();
        }
    }

    // HÀM TỰ ĐỘNG HỒI NĂNG LƯỢNG KHI KHÔNG CHẠY
    private void ManaRecovery()
    {
        if (Character1.mana <= Character1.maxMana)
        {
            Character1.mana += Time.deltaTime * 3;
        }
        else
        {
            Character1.mana = Character1.maxMana;
        }
    }

    // HÀM SỬ DỤNG NĂNG LUỌNG KHI NHÂN VẬT CHẠY
    private void ManaUsing()
    {
        if (Character1.mana > 0)
        {
            Character1.mana -= Time.deltaTime * 5;
        }
        else
        {
            Character1.mana = 0;
        }
    }
}
