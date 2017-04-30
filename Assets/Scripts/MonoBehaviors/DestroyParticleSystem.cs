using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem _ps;

    // Use this for initialization
    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
