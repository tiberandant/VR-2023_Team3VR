using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;
   

    void OnCollisionEnter(Collision collision)
    {
     if(!isRock && collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }  
     
    }
}
