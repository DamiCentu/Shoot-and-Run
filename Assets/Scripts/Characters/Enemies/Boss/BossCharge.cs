﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCharge : MonoBehaviour,BossActions {
    bool _moving = false;
    public float distanceToCharge = 5f;
    public float timeToStartCharging = 1f;
    public float timeToStartMovingAgain = 1f;
    public float speedOfCharge = 3f;
    private Boss boss;
    public bool upgrade;
    public float ExtraSpeedOfCharge=5;
    public int count=3;

    void BossActions.Begin(Boss boss)
    {
        EventManager.instance.SubscribeEvent(Constants.CHARGER_CRUSH,StopCharging);
        this.boss = boss;
        StartCoroutine(Wait());
        boss.SetAnimation("Shield", true);
        if (upgrade) {
            Upgrade();
        }
    }
    void BossActions.Finish(Boss boss)
    {
        boss.SetAnimation("Shield", false);
        EventManager.instance.UnsubscribeEvent(Constants.CHARGER_CRUSH, StopCharging);
    }


    private void StopCharging(object[] parameterContainer)
    {
        count--;
        if (count > 0)
        {

            StartCoroutine(ChargeMethod());
        }
        else {
            _moving = false;
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToStartCharging);
        StartCoroutine(ChargeMethod());
    }
    IEnumerator ChargeMethod()
    {
        _moving = true;
        //Vector3 target = new Vector3(boss.player.transform.position.x, boss.player.transform.position.y, boss.player.transform.position.z);
        Vector3 targetPosition = new Vector3(boss.player.transform.position.x, boss.transform.position.y, boss.player.transform.position.z);
        boss.transform.LookAt(targetPosition);
        while (_moving)
        {
            boss.transform.position += boss.transform.forward * speedOfCharge * Time.deltaTime * SectionManager.instance.EnemiesMultiplicator;
            yield return null;
        }
        yield return new WaitForSeconds(timeToStartMovingAgain);
    }

    void BossActions.Update(Transform boss, Vector3 playerPosition)
    {


    }

    public void Upgrade()
    {

        speedOfCharge += ExtraSpeedOfCharge;
        boss.SpawnEnemies("ChargeUpgrade",EnemiesManager.TypeOfEnemy.Charger);
    }
}
