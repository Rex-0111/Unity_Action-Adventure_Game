using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBreathingEffect : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    private float value;
    private float maxValue = 3f;
    private float minValue = 1f;
    [SerializeField] private float currentVelocity;
    [SerializeField] private float speed = 1f; // You may want to set a default speed

    private void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light>();
        }
        value = minValue; // Start at minValue
    }

    private void Update()
    {
        // Calculate the target value based on a ping-pong effect
        float targetValue = Mathf.PingPong(Time.time * speed, maxValue - minValue) + minValue;
        value = Mathf.SmoothDamp(value, targetValue, ref currentVelocity, 0.1f); // Smooth transition

        lightSource.range = value;
    }
}
