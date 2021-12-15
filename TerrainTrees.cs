using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainTrees : MonoBehaviour
{
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    NavMeshSurface surface;
    [SerializeField]
    float volumeSizeOffset = 2f;

    private void Start()
    {
        TerrainData data = terrain.terrainData;
        //making a list of gameobjects so we can delete the extra objects that are generated considering
        //most of us will have thousands of trees on our terrains
        List<GameObject> trees = new List<GameObject>();
        //loop through each tree in the terrain, create a gameobject and place it in the same position as the tree
        //then, give that object a NavMeshModifierVolume component of the specified size
        //then rebake the navmesh and delete all of the objects that were generated. The navmesh will remain baked with
        //the unwalkable areas leftover
        foreach (TreeInstance item in data.treeInstances)
        {
            //creating an empty gameobject to use as a "prefab"
            GameObject empty = new GameObject();
            //instantiating the empty object at the correct position
            GameObject fakeTree = Instantiate(empty, transform.position, Quaternion.identity, terrain.transform);
            //getting the size of the terrain
            var size = data.size;
            fakeTree.transform.position = new Vector3(item.position.x * size.x + terrain.transform.position.x, item.position.y * size.y + terrain.transform.position.y, item.position.z * size.z + terrain.transform.position.z);
            NavMeshModifierVolume mod = fakeTree.AddComponent<NavMeshModifierVolume>();
            mod.size = new Vector3(volumeSizeOffset, 3, volumeSizeOffset);
            //making volume not walkable
            mod.area = 1;
            trees.Add(fakeTree);
            trees.Add(empty);
        }
        //rebuilding navmesh
        surface.BuildNavMesh();
        //destroying the thousands of gameobjects that were created for each tree
        foreach (GameObject tree in trees)
        {
            Destroy(tree);
        }
    }
}
