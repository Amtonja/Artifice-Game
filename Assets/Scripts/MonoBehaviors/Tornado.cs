using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    private ParticleSystem ps;
    private float frequency = Mathf.PI * 20;

    public float a, b;
    public int points;

    // Use this for initialization
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var vel = ps.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;

        vel.x = new ParticleSystem.MinMaxCurve(1.0f, GenerateXCurve(a, b, frequency, points));
        vel.y = new ParticleSystem.MinMaxCurve(1.0f, GenerateYCurve(a, b, frequency, points));
    }

    // Update is called once per frame
    void Update()
    {

    }

    AnimationCurve GenerateXCurve(float a, float b, float frequency, int points)
    {
        AnimationCurve curve = new AnimationCurve();

        for (int i = 0; i < points; i++)
        {
            float t = i / ((float)points - 1);
            float xVelocity = (b * frequency) * Mathf.Cos(frequency * t) - (a + b * frequency * t) * Mathf.Sin(frequency * t);
            curve.AddKey(t, xVelocity);

            //Debug.Log("X key: " + t + ", " + xVelocity);
        }
        return curve;
    }

    AnimationCurve GenerateYCurve(float a, float b, float frequency, int points)
    {
        AnimationCurve curve = new AnimationCurve();

        for (int i = 0; i < points; i++)
        {
            float t = i / ((float)points - 1);
            float yVelocity = (b * frequency) * Mathf.Sin(frequency * t) + (a + b * frequency * t) * Mathf.Cos(frequency * t);
            curve.AddKey(t, yVelocity);
        }
        return curve;
    }
}
