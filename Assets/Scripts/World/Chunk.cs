using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : ChunkData {

    private readonly Dictionary<EnumBlockSide, Vector3> _blockSides = new Dictionary<EnumBlockSide, Vector3>(){
            { EnumBlockSide.EAST, new Vector3(1, 0, 0) },
            { EnumBlockSide.WEST, new Vector3(-1, 0, 0) },
            { EnumBlockSide.TOP, new Vector3(0, 1, 0) },
            { EnumBlockSide.BOTTOM, new Vector3(0, -1, 0) },
            { EnumBlockSide.NORTH, new Vector3(0, 0, 1) },
            { EnumBlockSide.SOUTH, new Vector3(0, 0, -1)  }
            };

    private readonly Dictionary<EnumBlockSide, Vector3[]> _blockSidesVertices = new Dictionary<EnumBlockSide, Vector3[]>(){
                { EnumBlockSide.EAST, new Vector3[4]{new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(1, 1, 1), new Vector3(1, 0, 1)} },
                { EnumBlockSide.WEST, new Vector3[4]{new Vector3(0, 0, 1), new Vector3(0, 1, 1), new Vector3(0, 1, 0), new Vector3(0, 0, 0)} },
                { EnumBlockSide.TOP, new Vector3[4]{new Vector3(0, 1, 0), new Vector3(0, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 0)} },
                { EnumBlockSide.BOTTOM, new Vector3[4]{new Vector3(1, 0, 0), new Vector3(1, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 0)} },
                { EnumBlockSide.NORTH, new Vector3[4]{new Vector3(1, 0, 1), new Vector3(1, 1, 1), new Vector3(0, 1, 1), new Vector3(0, 0, 1)} },
                { EnumBlockSide.SOUTH, new Vector3[4]{new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0), new Vector3(1, 0, 0)} },
            };

    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private int vertexIndex;

    private BlockType blockType;

    public static List<Chunk> chunkData = new List<Chunk>();

    private void Start() {        
        chunkData.Add(this);

        ChunkGen();
    }

    public void SaveChunk()
    {
        SaveSystem.SaveChunk(this);
    }

    private void Update() {
        
    }

    public void SetBlock(Vector3 worldPos, BlockType blockType) {
        Vector3 localPos = worldPos - transform.position;

        int x = Mathf.FloorToInt(localPos.x);
        int y = Mathf.FloorToInt(localPos.y);
        int z = Mathf.FloorToInt(localPos.z);

        blockData[x, y, z] = blockType;

        ChunkRenderer();
    }

    public static Chunk GetChunk(Vector3 pos) {
        for(int i = 0; i < chunkData.Count; i++) {            
            Vector3 cpos = chunkData[i].transform.position;

            if(
                pos.x < cpos.x || pos.x >= cpos.x + ChunkSize.x || 
                pos.y < cpos.y || pos.y >= cpos.y + ChunkSize.y || 
                pos.z < cpos.z || pos.z >= cpos.z + ChunkSize.z
            ) {
                continue;
            }

            return chunkData[i];
        }

        return null;
    }
    
    private void ChunkLayersGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        float _x = x + transform.position.x;
        float _y = y + transform.position.y;
        float _z = z + transform.position.z;

        _x += (World.WorldSize.x * ChunkSize.x);
        _z += (World.WorldSize.z * ChunkSize.z);

        if(_y < 32) {
            blockData[x, y, z] = BlockType.stone;
        }
        else if(_y == 32) {
            blockData[x, y, z] = BlockType.grass_block;
        }
    }

    private void ChunkGen() {
        if (SaveSystem.LoadChunk(this))
        {
            ChunkRenderer();
            return;
        }

        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    ChunkLayersGen(new Vector3(x, y, z));
                }
            }
        }

        ChunkRenderer();
    }

    private void ChunkRenderer() {
        mesh = new Mesh();
        mesh.name = "Chunk";

        vertices.Clear();
        triangles.Clear();
        uv.Clear();

        vertexIndex = 0;

        for(int x = 0; x < ChunkSize.x; x++) {
            for(int y = 0; y < ChunkSize.y; y++) {
                for(int z = 0; z < ChunkSize.z; z++) {
                    if(blockData[x, y, z] != BlockType.air) {
                        BlockGen(new Vector3(x, y, z));
                    }
                }
            }
        }

        MeshGen();
    }

    private void MeshGen() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshRenderer>().material = World.material;
    }

    private bool HasAdjacenteBlock(Vector3 adjacentBlock) {
        int x = (int)adjacentBlock.x;
        int y = (int)adjacentBlock.y;
        int z = (int)adjacentBlock.z;

        if(
            x < 0 || x > ChunkSize.x - 1 ||
            y < 0 || y > ChunkSize.y - 1 ||
            z < 0 || z > ChunkSize.z - 1
        ) {
            return true;// TODO: Pegar do chunk onde o bloco está
        }
        if(blockData[x, y, z] == BlockType.air) {
            return false;
        }
        else {
            return true;
        }        
    }

    private void BlockGen(Vector3 offset) {
        int x = (int)offset.x;
        int y = (int)offset.y;
        int z = (int)offset.z;

        blockType = blockData[x, y, z];
        foreach (var side in _blockSides)
        {
            if (!HasAdjacenteBlock(side .Value + offset))
            {
                VerticesGen(side.Key, offset);
            }
        }
    }

    private void UVsGen(Vector2 textureCoordinate) {
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

    private void TrianglesGen() {
        // Primeiro Tiangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(1 + vertexIndex);
        triangles.Add(2 + vertexIndex);

        // Segundo Triangulo
        triangles.Add(0 + vertexIndex);
        triangles.Add(2 + vertexIndex);
        triangles.Add(3 + vertexIndex);

        vertexIndex += 4;
    }

    private void VerticesGen(EnumBlockSide side, Vector3 offset)
    {
        for (var verticeIdx = 0; verticeIdx < 4; verticeIdx++)
        {
            vertices.Add(_blockSidesVertices[side][verticeIdx] + offset);
        }

        TrianglesGen();

        UVsPositionsGen();
    }

    private void UVsPositionsGen() {
        // Pre-Classic | rd-132211
        
        // STONE
        if(blockType == BlockType.stone) {
            UVsGen(new Vector2(1, 0));
        }

        // GRASS BLOCK
        if(blockType == BlockType.grass_block) {
            UVsGen(new Vector2(0, 0));
        }
    }
}
