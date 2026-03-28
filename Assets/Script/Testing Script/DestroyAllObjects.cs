using UnityEngine;

public class DestroyAllObjects : MonoBehaviour
{
    public GameObject[] objectsToDestroy;

    // Destroy only objects
    public void DestroyThem()
    {
        for (int i = 0; i < objectsToDestroy.Length; i++)
        {
            if (objectsToDestroy[i] != null)
            {
                Destroy(objectsToDestroy[i]);
            }
        }
    }
}
