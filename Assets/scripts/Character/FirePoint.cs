using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{


    public bool faceRight = true;
    public Vector2 moveVector;

  

    void Update()
    {
        Reflect();
        walk();
    }

    void walk()
    {
        
            moveVector.x = Input.GetAxis("Horizontal");

           
        

    }

    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            faceRight = !faceRight;
            transform.Rotate(0, 180, 0);


        }
        
            

        
    }
}
