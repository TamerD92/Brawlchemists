using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MovementTypeInterface
{
    public Vector2 direction;

    private void Start()
    {
        direction = Vector2.left;
    }

    private void Update()
    {
        Move();

        if (mainController.SeePlayerDir != Vector2.zero)
        {
            Vector2 flatDir = new Vector2(mainController.SeePlayerDir.x, 0).normalized;

            direction = flatDir;
        }
    }

    public override void setDir()
    {
            if (direction == Vector2.left)
            {
                direction = Vector2.right;
            }
            else
            {
                direction = Vector2.left;
            }
    }

    public override void Move()
    {
        mainController.rb.position += direction * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall")
        {
            setDir();
        }
    }
}
