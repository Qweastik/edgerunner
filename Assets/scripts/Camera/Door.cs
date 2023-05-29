using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController camera;
    [SerializeField] private playerMove player;

    private void Start()
    {
        camera.MoveToNewRoom(player.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                camera.MoveToNewRoom(nextRoom);
            }

            else
            {
                camera.MoveToNewRoom(previousRoom);
            }
        }
    }

}
