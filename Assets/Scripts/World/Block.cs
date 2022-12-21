using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uv = new List<Vector2>();

    private enum BlockFace {        
        EAST,
        WEST,
        TOP,
        BOTTOM,
        NORTH,
        SOUTH
    }

    private int vertexIndex;

    [SerializeField] private BlockType blockType;
    
    void Start() {
        // Crie a malha
        mesh = new Mesh();
        mesh.name = "Block";

        BlockGen();

        MeshRenderer();
    }

    void Update() {
        
    }

    private void MeshRenderer() {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();

        // Adiciona a malha um colisor
        GetComponent<MeshCollider>().sharedMesh = mesh;

        // Adicione a malha ao MeshFilter do seu GameObject
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void BlockGen() {
        VerticesAdd(BlockFace.EAST);
        VerticesAdd(BlockFace.WEST);
        VerticesAdd(BlockFace.TOP);
        VerticesAdd(BlockFace.BOTTOM);
        VerticesAdd(BlockFace.NORTH);
        VerticesAdd(BlockFace.SOUTH);
    }

    // Adicione os Vertices da Malha
    private void VerticesAdd(BlockFace face) {
        switch(face) {
            case BlockFace.EAST: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 0, 1));

                break;
            }
            case BlockFace.WEST: {
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case BlockFace.TOP: {
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(1, 1, 0));

                break;
            }
            case BlockFace.BOTTOM: {
                vertices.Add(new Vector3(1, 0, 0));
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(0, 0, 1));
                vertices.Add(new Vector3(0, 0, 0));

                break;
            }
            case BlockFace.NORTH: {
                vertices.Add(new Vector3(1, 0, 1));
                vertices.Add(new Vector3(1, 1, 1));
                vertices.Add(new Vector3(0, 1, 1));
                vertices.Add(new Vector3(0, 0, 1));

                break;
            }
            case BlockFace.SOUTH: {
                vertices.Add(new Vector3(0, 0, 0));
                vertices.Add(new Vector3(0, 1, 0));
                vertices.Add(new Vector3(1, 1, 0));
                vertices.Add(new Vector3(1, 0, 0));

                break;
            }
        }

        TrianglesAdd();

        UVsPos();
    }

    // Adicone os Triangulos dos Vertices para renderizar a face
    private void TrianglesAdd() {
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

    // Adicione as UVs dos Vertices para renderizar a textura
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

    // Pegue a posição da UV no Texture Atlas
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
