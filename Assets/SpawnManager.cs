using Unity.Netcode;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance;

    public Transform[] spawnPoints;
    private int nextSpawnIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetNextSpawnPosition()
    {
        Vector3 pos = spawnPoints[nextSpawnIndex].position;
        nextSpawnIndex = (nextSpawnIndex + 1) % spawnPoints.Length;
        return pos;
    }

    public Quaternion GetNextSpawnRotation()
    {
        return spawnPoints[nextSpawnIndex].rotation;
    }
}
