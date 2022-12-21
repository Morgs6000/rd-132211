using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    // Tamanho do mundo em blocos
    public static Vector3 WorldSizeInBlocks = new Vector3(
        256,
        64,
        256
    );
    
    // Tamanho do mundo em chunks
    public static Vector3 WorldSize = new Vector3(
        WorldSizeInBlocks.x / Chunk.ChunkSize.x,
        WorldSizeInBlocks.y / Chunk.ChunkSize.y,
        WorldSizeInBlocks.z / Chunk.ChunkSize.z
    );
    
    // Dicionário de chunks
    private Dictionary<Vector3, Chunk> chunks = new Dictionary<Vector3, Chunk>();

    // Transform do jogador
    [SerializeField] private Transform player;

    // Distância de visualização
    public static int viewDistance = 5;

    // Prefab de chunk
    [SerializeField] private GameObject chunkPrefab;

    // Inicializa a geração do mundo
    private void Start() {
        WorldGen();
    }

    // Atualiza a renderização do mundo
    private void Update() {
        StartCoroutine(WorldRenderer());
    }

    // Gera o mundo
    private void WorldGen() {
        // Itera pelas coordenadas de chunk
        for(int x = -((int)WorldSize.x / 2); x < ((int)WorldSize.x / 2); x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -((int)WorldSize.z / 2); z < ((int)WorldSize.z / 2); z++) {
                    // Calcula o deslocamento do chunk
                    Vector3 chunkOffset = new Vector3(
                        x * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        z * Chunk.ChunkSize.z
                    );

                    // Adiciona o chunk ao dicionário
                    chunks.Add(chunkOffset, new Chunk());
                }
            }
        }
    }

    // Renderiza os chunks do mundo
    private IEnumerator WorldRenderer() {
        // Calcula a posição do jogador em chunks
        int posX = Mathf.FloorToInt(player.position.x / Chunk.ChunkSize.x);
        int posZ = Mathf.FloorToInt(player.position.z / Chunk.ChunkSize.z);

        // Itera pelas coordenadas de chunk dentro da distância de visualização
        for(int x = -viewDistance; x < viewDistance; x++) {
            for(int y = 0; y < WorldSize.y; y++) {
                for(int z = -viewDistance; z < viewDistance; z++) {
                    // Calcula o deslocamento do chunk
                    Vector3 chunkOffset = new Vector3(
                        (x + posX) * Chunk.ChunkSize.x,
                        y * Chunk.ChunkSize.y,
                        (z + posZ) * Chunk.ChunkSize.z
                    );

                    // Obtém o chunk no deslocamento especificado
                    Chunk c = Chunk.GetChunk(new Vector3(
                        Mathf.FloorToInt(chunkOffset.x),
                        Mathf.FloorToInt(chunkOffset.y),
                        Mathf.FloorToInt(chunkOffset.z)
                    ));
                    
                    // Verifica se o chunk já existe no dicionário
                    if(chunks.ContainsKey(chunkOffset)) {
                        // Verifica se o chunk ainda não foi instanciado no mundo
                        if(c == null) {
                            GameObject chunk = Instantiate(chunkPrefab, chunkOffset, Quaternion.identity, this.transform);
                            chunk.name = "Chunk(" + (x + posX) + ", " + (z + posZ) + ")";
                        }
                    }

                    // Pausa a rotina por um frame
                    yield return null;
                }
            }
        }
    }
}
