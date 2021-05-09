using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public bool ProjectileActive;
    public Vector2 Direction;


    private const float PROJECTILE_SPEED = 3;

    internal void Init() {
        gameObject.SetActive(false);

        ProjectileActive = false;

    }

    internal void Shoot(Vector3 targetPos, Vector3 originPos) {
        ProjectileActive = true;
        
        Direction = targetPos - originPos;

        Direction.Normalize();

        Direction *= PROJECTILE_SPEED;

        transform.position = originPos;
        gameObject.SetActive(true);
    }

    internal void Progress() {
        if (ProjectileActive) {
            transform.Translate(Direction * Time.deltaTime);

        }
    }

    internal void Terminate() {
        ProjectileActive = false;
        gameObject.SetActive(false);
    }
}
