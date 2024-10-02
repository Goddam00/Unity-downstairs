using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] GameObject[] floorPrefabs;
    public void SpawnFloor(){
        int rand = Random.Range(0, floorPrefabs.Length);
        GameObject floor = Instantiate(floorPrefabs[rand], transform);
        floor.transform.position = new Vector3(Random.Range(-3.8f, 3.8f), -6f, 0f);
    }
}
