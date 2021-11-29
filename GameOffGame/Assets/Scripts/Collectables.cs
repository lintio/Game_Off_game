using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private List<Collectable> _levelCollectables = new List<Collectable>();

    public event Action<int> AddBlackAnt;
    public event Action<int> AddRedAnt;


    private void Awake()
    {
        foreach (Transform Child in this.transform)
        {
            _levelCollectables.Add(Child.GetComponent<Collectable>());
        }
    }

    private void Start()
    {
        foreach(var collectable in _levelCollectables)
        {
            collectable.onPickUp += Collectable_onPickUp;
        }
    }

    private void Collectable_onPickUp(Collectable obj)
    {
        AddBlackAnt?.Invoke(obj.antBlackValue);
        AddRedAnt?.Invoke(obj.antRedValue);
    }
}
