using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSController : MonoBehaviour
{
    public EnemyController mainController;

    public Transform target;

    public Transform eyePoint;

    public float LOSrange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 isLOS()
    {
        

        int layerMask = 1 << 9;

        Vector2 dir = target.position - eyePoint.position;

        RaycastHit2D info = Physics2D.Raycast(eyePoint.position, dir.normalized, LOSrange, ~layerMask);

        Debug.DrawRay(eyePoint.position, dir, Color.black);


        if (info)
        {
            if (info.collider.tag == "Player")
            {
                return dir;
            }
        }

        return Vector2.zero;
    }
}
