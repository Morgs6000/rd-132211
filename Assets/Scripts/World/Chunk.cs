using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    // Malha do chunk usada para renderizar os blocos
    private Mesh mesh;

    // Listas de vértices, triângulos e coordenadas de textura da malha
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    // Enum com as faces de um bloco
    private enum BlockFace {        
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    // Índice do próximo vértice a ser adicionado à malha
    private int vertexIndex;

    // Tamanho do chunk em unidades de bloco
    public static Vector3 ChunkSize = new Vector3(
        16, 
        64, 
        16
    );

    // Dicionário que armazena as posições e os tipos de bloco presentes no chunk
    private Dictionary<Vector3, BlockType> voxelMap = new Dictionary<Vector3, BlockType>();

    // Tipo de bloco atual
    private BlockType blockType;

    // Lista de todos os chunks do mundo
    public static List<Chunk> chunkList = new List<Chunk>();

    private void Start() {        
        // Adiciona este chunk à lista de chunks
        chunkList.Add(this);

        // Gera os blocos do chunk
        ChunkGen();
    }

    private void Update() {
        // Atualizações do chunk podem ser feitas aqui (por exemplo, mudar a posição de um bloco)        
    }

    // Adiciona um bloco à voxel map e atualiza a malha do chunk
    public void SetBlock(Vector3 worldPos, BlockType b) {
        // Calcula a posição local do bloco em relação ao chunk
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        // Adiciona o bloco à voxel map
        voxelMap[new Vector3(x, y, z)] = b;

        // Atualiza a malha do chunk para refletir o novo bloco
        ChunkRenderer();
    }

    // Retorna o chunk que contém a posição dada, se houver
    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunkList.Count; i++) {            
            // Calcula a posição do chunk atual
            Vector3 chunkPos = chunkList[i].transform.position;

            // Verifica se a posição dada está dentro dos limites do chunk
            if(
                pos.x < chunkPos.x || pos.x >= chunkPos.x + ChunkSize.x || 
                pos.y < chunkPos.y || pos.y >= chunkPos.y + ChunkSize.y || 
                pos.z < chunkPos.z || pos.z >= chunkPos.z + ChunkSize.z
            ) {
                // A posição não está neste chunk, passa para o próximo
                continue;
            }

            // A posição está neste chunk, retorna o chunk
            return chunkList[i];
        }

        // Nenhum chunk contém a posição dada
        return null;
    }
    
    // Gera as camadas de blocos do chunk de acordo com sua posição
    private void ChunkLayersGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        // Calcula a posição global do bloco
        int _x = x + (int)transform.position.x;
        int _y = y + (int)transform.position.y;
        int _z = z + (int)transform.position.z;

        // Ajusta a posição global para o tamanho do mundo
        _x += (int)World.WorldSizeInBlocks.x;
        _z += (int)World.WorldSizeInBlocks.z;

        // Adiciona um bloco de pedra abaixo da superfície
        if(_y < 32) {
            voxelMap.Add(offset, BlockType.stone);
        }
        // Adiciona um bloco de grama na superfície
        else if(_y == 32) {
            voxelMap.Add(offset, BlockType.grass_block);
        }
        // Adiciona um bloco de ar acima da superfície
        else {
            voxelMap.Add(offset, BlockType.air);
        }
    }

    // Gera todos os blocos do chunk
    private void ChunkGen() {
        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    ChunkLayersGen(new Vector3(x, y, z));
                }
            }
        }

        // Atualiza a malha do chunk para refletir todos os blocos
        ChunkRenderer();
    }

    // Atualiza a malha do chunk para refletir a voxel map atual
    // É chamada sempre que um bloco é adicionado ou removido do chunk.
    private void ChunkRenderer() {
        // Cria uma nova malha
        mesh = new Mesh();
        mesh.name = "Chunk";

        // Limpa as listas de vértices, triângulos e coordenadas de textura
        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        // Reseta o índice de vértices
        vertexIndex = 0;

        // Percorre os voxels do chunk
        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    // Se o voxel atual não for ar, adiciona suas faces à malha
                    if(voxelMap[new Vector3(x, y, z)] != BlockType.air) {
                        BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        MeshRenderer();
    }

    private void MeshRenderer() {
        // Atribui as listas de vértices, triângulos e coordenadas de textura à malha
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private bool HasAdjacentBlock(Vector3 adjacentBlock) {
        int x = (int)adjacentBlock.x;
        int y = (int)adjacentBlock.y;
        int z = (int)adjacentBlock.z;

        if(
            x < 0 || x > ChunkSize.x - 1 ||
            y < 0 || y > ChunkSize.y - 1 ||
            z < 0 || z > ChunkSize.z - 1
        ) {
            return false;
        }
        if(voxelMap[adjacentBlock] == BlockType.air) {
            return false;
        }
        else {
            return true;
        }        
    }

    private void BlockGen(Vector3 offset) {
        blockType = voxelMap[offset];

        if(!HasAdjacentBlock(new Vector3(1, 0, 0) + offset)) {
            VerticesAdd(BlockFace.EAST, offset);
        }

        if(!HasAdjacentBlock(new Vector3(-1, 0, 0) + offset)) {
            VerticesAdd(BlockFace.WEST, offset);
        }

        if(!HasAdjacentBlock(new Vector3(0, 1, 0) + offset)) {
            VerticesAdd(BlockFace.TOP, offset);
        }

        if(!HasAdjacentBlock(new Vector3(0, -1, 0) + offset)) {
            VerticesAdd(BlockFace.BOTTOM, offset);
        }

        if(!HasAdjacentBlock(new Vector3(0, 0, 1) + offset)) {
            VerticesAdd(BlockFace.NORTH, offset);
        }

        if(!HasAdjacentBlock(new Vector3(0, 0, -1) + offset)) {
            VerticesAdd(BlockFace.SOUTH, offset);
        }
    }

    private void VerticesAdd(BlockFace face, Vector3 offset) {
        switch(face) {
            case BlockFace.EAST: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);

                break;
            }
            case BlockFace.WEST: {
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case BlockFace.TOP: {
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);

                break;
            }
            case BlockFace.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0) + offset);
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);
                vertices.Add(new Vector3(0, 0, 0) + offset);

                break;
            }
            case BlockFace.NORTH: {
                vertices.Add(new Vector3(1, 0, 1) + offset);
                vertices.Add(new Vector3(1, 1, 1) + offset);
                vertices.Add(new Vector3(0, 1, 1) + offset);
                vertices.Add(new Vector3(0, 0, 1) + offset);

                break;
            }
            case BlockFace.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0) + offset);
                vertices.Add(new Vector3(0, 1, 0) + offset);
                vertices.Add(new Vector3(1, 1, 0) + offset);
                vertices.Add(new Vector3(1, 0, 0) + offset);

                break;
            }
        }

        TrianglesAdd();

        UVsPos();
    }

    private void TrianglesAdd() {
        // Primeiro Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }

    private void UVsAdd(Vector2 textureCoordinate) {
        Vector2 offset = new Vector2(
            0, 
            0
        );

        Vector2 textureSize = new Vector2(
            16 + offset.x,
            16 + offset.y
        );
        
        float x = textureCoordinate.x + offset.x;
        float y = textureCoordinate.y + offset.y;

        float _x = 1.0f / textureSize.x;
        float _y = 1.0f / textureSize.y;

        y = (textureSize.y - 1) - y;

        x *= _x;
        y *= _y;

        uv.Add(new Vector2(x, y));
        uv.Add(new Vector2(x, y + _y));
        uv.Add(new Vector2(x + _x, y + _y));
        uv.Add(new Vector2(x + _x, y));
    }

    private void UVsPos() {
        // Pre-Classic | rd-132211
        
        // STONE
        if(blockType == BlockType.stone) {
            UVsAdd(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(blockType == BlockType.grass_block) {
            UVsAdd(new Vector2(0, 0));
        }
    }
}
