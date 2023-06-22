using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExplosionTrigger : MonoBehaviour
{

    /*public ParticleSystem explosion;*/
    public CameraShake cameraShake;
    //ThirdPersonCam thirdpersoncam;

    //public CameraShake thirdPersonCam;
    //public CameraShake leftWaveCam;
    //public CameraShake rightWaveCam;

 /*   void camerSwitching()
    {
       
    }*/
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            /*explosion.Play();*/
            StartCoroutine(cameraShake.Shake(.15f, .2f));
            //camerSwitching();

            /* if (thirdpersoncam.GetCameraStyle() == ThirdPersonCam.CameraStyle.Basic)
                     StartCoroutine(thirdPersonCam.Shake(.15f, .2f));
             else if(thirdpersoncam.GetCameraStyle() == ThirdPersonCam.CameraStyle.LeftWave)
                     StartCoroutine(leftWaveCam.Shake(.15f, .2f));
             else if (thirdpersoncam.GetCameraStyle() == ThirdPersonCam.CameraStyle.RightWave)
                     StartCoroutine(rightWaveCam.Shake(.15f, .2f));*/
        }
        
    }
}
