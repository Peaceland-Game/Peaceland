using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MemoryParticleManager : MonoBehaviour
{
    [SerializeField] GameObject linesParticles;
    [SerializeField] GameObject sparkleParticles;
    [SerializeField] float thresholdRange;

    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float disSqr = Vector3.SqrMagnitude(this.transform.position - cam.transform.position);
        
        if(disSqr > thresholdRange)
        {
            linesParticles.SetActive(false);
            sparkleParticles.SetActive(true);
        }
        else
        {
            linesParticles.SetActive(true);
            sparkleParticles.SetActive(false);
        }
    }
}
