using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public List<NavMeshSurface> surfaces;

    // Start is called before the first frame update
    public void Build()
    {
        for (int i = 0; i < surfaces.Count; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }

    public void AddSurfacesOfChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            surfaces.Add(transform.GetChild(i).GetComponent<NavMeshSurface>());
        }
    }
}
