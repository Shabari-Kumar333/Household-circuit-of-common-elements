using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class particlehitdetection : MonoBehaviour
{
    [SerializeField] GameObject[] glowingWires;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject particleEffects;
    [SerializeField] GameObject target;
    // Start is called before the first frame update
      private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag(target.tag))
        {
            
        Debug.Log("Particle hit: " + other.name);
      foreach(GameObject gameObject in glowingWires)
            {
                gameObject.SetActive(true);
            }
        lineRenderer.gameObject.SetActive(false);

        particleEffects.SetActive(false);
        }
    }

}
