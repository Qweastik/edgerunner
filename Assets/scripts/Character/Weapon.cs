using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator anim;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private playerMove playerMove;

    private void Awake()
    {
        playerMove = GetComponent<playerMove>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && playerMove.canShooting())
        {
            anim.SetTrigger("Shoot");

        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

}
