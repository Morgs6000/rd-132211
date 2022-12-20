using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.World
{
    public static class SaveSystem
    {
        private static string GetFileName(ChunkData chunk)
        {
            string[] chunkPosStr = Array.ConvertAll(chunk.ChunkPosition, x => x.ToString());
            string chunkPos = String.Join("_", chunkPosStr);
            string path = Application.persistentDataPath + $"/saveFile{chunkPos}.save";
            return path;
        }

        public static void SaveChunk(ChunkData chunk)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            var path = GetFileName(chunk);
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, chunk.BlockData);
            stream.Close();

        }

        public static bool LoadChunk(ChunkData chunk)
        {
            var path = GetFileName(chunk);
            if (!File.Exists(path))
            {
                return false;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            chunk.BlockData =  formatter.Deserialize(stream) as int[,,];
            stream.Close();
            return true;
        }
    }
}
