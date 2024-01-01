using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyManager EnemyManager;
    public Slider slider;
    public TMP_Text enemyName;
    public GameObject enemyCanvas;
    private float _timeUntilBarIsHidden = 0;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        EnemyManager = GetComponentInParent<EnemyManager>();
        enemyName = GetComponentInChildren<TMP_Text>();
        enemyName.text = EnemyManager.charName.ToString();

    }

    public void SetHealth(int health)
    {
        slider.value = health;
      //  _timeUntilBarIsHidden = 3;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    private void Update()
    {/*
        _timeUntilBarIsHidden = _timeUntilBarIsHidden - Time.deltaTime;

        if (slider != null)
        {
            if (_timeUntilBarIsHidden <= 0)
            {
                _timeUntilBarIsHidden = 0;
                slider.gameObject.SetActive(false);
            }
            else
            {
                if (!slider.gameObject.activeInHierarchy)
                {
                    slider.gameObject.SetActive(true);
                }
            }

            if (slider.value <= 0)
            {
                Destroy(slider.gameObject);
            }
        }
        */
    }
}