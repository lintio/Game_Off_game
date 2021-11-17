using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collectable : MonoBehaviour
{
    public event Action<Collectable> onPickUp;
    public int antValue = 10;

    [SerializeField] GameObject antCountText;

    private void Start()
    {
        antCountText.GetComponent<TextMesh>().text = antValue.ToString();
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
