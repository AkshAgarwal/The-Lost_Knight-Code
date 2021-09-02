using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderControl : MonoBehaviour
{
    [SerializeField] private int damage;
    // Start is called before the first frame update
    private void Start()
    {
        damage = 2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("hitting Player");
            PlayerController2D.controller.TakeDamage(damage);
        }
    }
}
