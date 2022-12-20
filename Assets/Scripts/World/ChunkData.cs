using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.World
{
    [Serializable]
    public class ChunkData : MonoBehaviour
    {

        public static Vector3Int ChunkSize = new Vector3Int(
            16,
            64,
            16
        );

        [SerializeField]
        public int[] ChunkPosition { 
            get =>
                 new int[3] { Mathf.RoundToInt(this.gameObject.transform.position.x), Mathf.RoundToInt(this.gameObject.transform.position.y), Mathf.RoundToInt(this.gameObject.transform.position.z) };
            }

        [SerializeField]
        public int[,,] BlockData { get => BlockDataToInt(blockData); set => blockData = IntToBlockData(value); }

        protected BlockType[,,] blockData = new BlockType[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        private static BlockType[,,] IntToBlockData(int[,,] blockDataValue)
        {
            var newData = new BlockType[ChunkSize.x, ChunkSize.y, ChunkSize.z];
            for (int x = 0; x < ChunkSize.x; x++)
            {
                for (int y = 0; y < ChunkSize.y; y++)
                {
                    for (int z = 0; z < ChunkSize.z; z++)
                    {
                        newData[x, y, z] = (BlockType)blockDataValue[x, y, z];
                    }
                }

            }
            return newData;
        }


        private static int[,,] BlockDataToInt(BlockType[,,] blockDataValue)
        {
            int[,,] newData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];
            for (int x = 0; x < ChunkSize.x; x++)
            {
                for (int y = 0; y < ChunkSize.y; y++)
                {
                    for (int z = 0; z < ChunkSize.z; z++)
                    {
                        newData[x, y, z] = (int)blockDataValue[x, y, z];
                    }
                }

            }
            return newData;
        }
    }
}
