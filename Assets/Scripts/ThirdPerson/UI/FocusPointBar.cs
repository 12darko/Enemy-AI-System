using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusPointBar : MonoBehaviour
{
     public Slider Slider;

     private void Start()
     {
          Slider = GetComponent<Slider>();
     }

     public void SetMaxFocusPoint(float maxFocusPoint)
     {
          Slider.maxValue = maxFocusPoint;
          Slider.value = maxFocusPoint;
     }

     public void SetCurrentFocusPoint(float currentFocusPoint)
     {
          Slider.value = currentFocusPoint;
     }
}
