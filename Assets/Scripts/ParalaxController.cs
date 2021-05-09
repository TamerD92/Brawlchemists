using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{

    public Transform[] LayerTransforms;
    public float[] LayerSpeedRatios;

    private float _camXPosition;

    public float CamXPosition {
        get {
            return _camXPosition;
        }
        set {
            _camXPosition = value;

            LayerTransforms[0].transform.position = new Vector3(_camXPosition, LayerTransforms[0].transform.position.y, LayerTransforms[0].transform.position.z);

            for (int i = 0; i < LayerSpeedRatios.Length; i++) {
                LayerTransforms[i + 1].transform.position = new Vector3(_camXPosition * LayerSpeedRatios[i], LayerTransforms[i + 1].transform.position.y, LayerTransforms[i + 1].transform.position.z);
            }

        }
    }

    void Start()
    {
        
    }

    

}
