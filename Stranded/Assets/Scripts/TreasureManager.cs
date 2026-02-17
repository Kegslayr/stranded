using UnityEngine;
using System.Collections.Generic;

public class TreasureManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> TreasureLocations = new List<Transform>();
    
    [SerializeField]
    private GameObject TreasurePrefab;

    private void Start(){
        int randomTreasure = Random.Range(0, TreasureLocations.Count);
        Debug.Log($"Spawn treasure at location:{randomTreasure}");

        Transform treasureTransform = TreasureLocations[randomTreasure].transform;
        GameObject treasure = Instantiate(TreasurePrefab, treasureTransform.position, treasureTransform.rotation);
    }
}
