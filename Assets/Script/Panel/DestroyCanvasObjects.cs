using UnityEngine;

public class DestroyCanvasObjects : MonoBehaviour
{
    public GameObject[] canvasesToDestroy;   // Add multiple canvases here

    public void DestroyCanvas()
    {
        for (int i = 0; i < canvasesToDestroy.Length; i++)
        {
            if (canvasesToDestroy[i] != null)
            {
                Destroy(canvasesToDestroy[i]);
            }
        }
    }
}
