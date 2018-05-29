using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderHandler : MonoBehaviour
{
    Slider m_slider;

    [SerializeField]
    float m_value;

    // Use this for initialization
    void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    public void Increase()
    {
        m_slider.value += m_value;
        if (m_slider.value >= m_slider.maxValue)
            m_slider.value = m_slider.maxValue;
    }

    public void Decrease()
    {
        m_slider.value -= m_value;
        if (m_slider.value <= m_slider.minValue)
            m_slider.value = m_slider.minValue;
    }
}