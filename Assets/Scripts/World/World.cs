using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    [SerializeField] private GameObject chunkPrefab;
    //[SerializeField] public static Material material;

    public static Vector3Int WorldSizeInBlocks = new Vector3Int(
        256,
        64,
        256
    );
    
    public static Vector3Int WorldSize = new Vector3Int(
        WorldSizeInBlocks.x / Chunk.ChunkSize.x,
        WorldSizeInBlocks.y / Chunk.ChunkSize.y,
        WorldSizeInBlocks.z / Chunk.ChunkSize.z
    );

    [SerializeField] private Transform player;

    public static int viewDistance = 5;

    private void Start() {
        InitialWorldGen();
    }

    private void Update() {
        StartCoroutine(WorldGen());
    }

    private void InitialWorldGen() {
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    int r2 = viewDistance * viewDistance;

                    if(new Vector3(x, y, z).sqrMagnitude < r2) {
                        Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                    }
                }
            }
        }

        Vector3 spawn = new Vector3(
            0,
            WorldSizeInBlocks.y,
            0
        );
        
        //player.position = spawn;
    }

    private IEnumerator WorldGen() {
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    Vector3 chunkOffset = new Vector3(
                        (x + posX) * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        (z + posZ) * Chunk.ChunkSize.z
                    );

                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(chunkOffset.x),
                        Mathf.FloorToInt(chunkOffset.y),
                        Mathf.FloorToInt(chunkOffset.z)
                    ));

                    if(c == null) {
                        int r2 = viewDistance * viewDistance;

                        if(new Vector3(x, y, z).sqrMagnitude < r2) {
                            Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                        }
                    }

                    yield return null;
                }
            }
        }
    }
}
