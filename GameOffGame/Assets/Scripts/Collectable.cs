using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Collectable : MonoBehaviour
{
    public event Action<Collectable> onPickUp;
    public int antBlackValue = 10;
    public int antRedValue = 10;

    [SerializeField] TMP_Text antCountText;

    private void Start()
    {
        
        antCountText.text = "B: " + antBlackValue.ToString() + " / R: " + antRedValue.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            onPickUp?.Invoke(this);

            gameObject.SetActive(false);
        }
    }
}
