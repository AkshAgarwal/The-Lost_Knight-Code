using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject player;
   
  
    public void respawn()
    {
        Instantiate(player, transform.position, transform.rotation);

    }
}
