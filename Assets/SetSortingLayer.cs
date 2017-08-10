using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script intends to allow inspector setting of sorting layers for renderers that do not normally
/// expose the sorting layer property in the inspector.
/// </summary>

public class SetSortingLayer : MonoBehaviour
{
    private SortingLayer[] layers;
    private Renderer _renderer;

    public SortingLayer layer;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        layers = SortingLayer.layers;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
