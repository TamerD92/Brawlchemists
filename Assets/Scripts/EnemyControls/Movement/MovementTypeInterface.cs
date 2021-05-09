using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypeInterface : MonoBehaviour
{
    public  EnemyController mainController;

    public virtual void Move()
    { }

    public virtual void setDir()
    { }
}
