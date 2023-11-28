using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = _player.position.x;
        temp.y = _player.position.y;

        transform.position = temp;
    }
}
