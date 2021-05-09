using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public Projectile ProjectilePF;

    List<Projectile> _projectiles;

    float _myVar;

    public float MyVar { get => Mathf.Clamp(_myVar, 0, 1); set => _myVar = value; }

    public float MySetGet {
        set {
            //

        }
        get {
            //
            return 3f;
        }
    }

    

    public void Init() {
        
        _projectiles = new List<Projectile>();
        
        /*for (int i = 0; i < TOTAL_POJECTILES; i++) {
            
            _projectiles.Add(Instantiate<Projectile>(ProjectilePF));
            _projectiles[i].Init();
                
        }*/
    }
   

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            Projectile proj = _projectiles.Find( (p) => !p.ProjectileActive );

            if (proj == null) {
                proj = Instantiate<Projectile>(ProjectilePF);

                _projectiles.Add(proj);
                proj.Init();
            }


            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9));

            proj.Shoot(worldMousePos, transform.position);

        }

        for (int i = 0; i < _projectiles.Count; i++) {
            
            _projectiles[i].Progress();

            if ((_projectiles[i].transform.position - transform.position).magnitude > 10) {
                
                _projectiles[i].Terminate();
            }

        }
        
    }
}
