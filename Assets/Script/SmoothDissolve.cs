//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SmoothDissolve : MonoBehaviour
//{
//    public Material dissolveMat;
//    public string propertyName = "_DissolveAmount"; // change if your shader uses another name
//    public float speed = 1f;

//    private float value = 1f; // start fully visible

//    void Start()
//    {
//        // Set material to start state
//        dissolveMat.SetFloat(propertyName, value);
//    }

//    void Update()
//    {
//        // Gradually reduce the dissolve amount
//        value -= speed * Time.deltaTime;
//        value = Mathf.Clamp01(value);

//        dissolveMat.SetFloat(propertyName, value);
//    }
//}


//using UnityEngine;
//using System.Collections;

//public class SmoothDissolve : MonoBehaviour
//{
//    [Header("Renderer")]
//    public SkinnedMeshRenderer skinnedRenderer;

//    [Header("Materials")]
//    public Material highlightMat;
//    public Material dissolveMat;
//    public Material realMat;

//    [Header("Dissolve Settings")]
//    public string propertyName = "_DissolveAmount";
//    public float speed = 1f;

//    private Coroutine dissolveRoutine;

//    // Call this when user starts dragging
//    public void ApplyHighlight()
//    {
//        skinnedRenderer.material = highlightMat;
//    }

//    // Call this when dropping the object
//    public void StartDissolve()
//    {
//        if (dissolveRoutine != null)
//            StopCoroutine(dissolveRoutine);

//        dissolveRoutine = StartCoroutine(DissolveEffect());
//    }

//    private IEnumerator DissolveEffect()
//    {
//        // Switch to dissolve mat
//        skinnedRenderer.material = dissolveMat;

//        float value = 1f;
//        dissolveMat.SetFloat(propertyName, value);

//        // Dissolve from 1 → 0
//        while (value > 0f)
//        {
//            value -= speed * Time.deltaTime;
//            value = Mathf.Clamp01(value);

//            dissolveMat.SetFloat(propertyName, value);

//            yield return null;
//        }

//        // After dissolve ends, switch back to real material
//        skinnedRenderer.material = realMat;
//    }
//}

using UnityEngine;
using System.Collections;

public class SmoothDissolve : MonoBehaviour
{
    [Header("Materials")]
    public Material dissolveMaterial;   // The boom-reveal shader material
    public Material finalMaterial;      // The actual final material after dissolve

    [Header("Renderer")]
    public Renderer targetRenderer;     // MeshRenderer or SkinnedMeshRenderer

    [Header("Shader Property")]
    public string propertyName = "_DissolveAmount";

    [Header("Effect Settings")]
    public float speed = 2f;            // Faster = quicker boom effect

    private bool isPlaying = false;

    public void StartBoomReveal()
    {
        if (isPlaying || targetRenderer == null) return;

        StartCoroutine(DissolveRoutine());
    }

    private IEnumerator DissolveRoutine()
    {
        isPlaying = true;

        // 1) Switch mesh to dissolve material
        targetRenderer.material = dissolveMaterial;

        // 2) Start invisible
        dissolveMaterial.SetFloat(propertyName, 1f);

        float value = 1f;

        // 3) Animate 1 → 0 (boom reveal)
        while (value > 0f)
        {
            value -= Time.deltaTime * speed;
            value = Mathf.Clamp01(value);

            dissolveMaterial.SetFloat(propertyName, value);

            yield return null;
        }

        // 4) Switch to final material instantly after dissolve finishes
        targetRenderer.material = finalMaterial;

        isPlaying = false;
    }
}

