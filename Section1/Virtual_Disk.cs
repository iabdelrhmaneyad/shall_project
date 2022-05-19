// Decompiled with JetBrains decompiler
// Type: Section1.Virtual_Disk
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;
using System.IO;

namespace Section1
{
    public static class Virtual_Disk
    {
        public static FileStream Disk;

        public static void CREATEorOPEN_Disk(string path) => Virtual_Disk.Disk = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        public static int getFreeSpace() => 1048576 - (int)Virtual_Disk.Disk.Length;

        public static void initalize(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Virtual_Disk.CREATEorOPEN_Disk(path);
                    byte[] cluster = new byte[1024];
                    for (int index = 0; index < cluster.Length; ++index)
                        cluster[index] = (byte)0;
                    Virtual_Disk.writeCluster(cluster, 0);
                    Mini_FAT.createFAT();
                    Directory directory = new Directory("K:", (byte)16, 5, (Directory)null);
                    directory.writeDirectory();
                    Mini_FAT.setClusterPointer(5, -1);
                    Program.current = directory;
                    Mini_FAT.writeFAT();
                }
                else
                {
                    Virtual_Disk.CREATEorOPEN_Disk(path);
                    Mini_FAT.readFAT();
                    Directory directory = new Directory("K:", (byte)16, 5, (Directory)null);
                    directory.readDirectory();
                    Program.current = directory;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void writeCluster(byte[] cluster, int clusterIndex, int offset = 0, int count = 1024)
        {
            Virtual_Disk.Disk.Seek((long)(clusterIndex * 1024), SeekOrigin.Begin);
            Virtual_Disk.Disk.Write(cluster, offset, count);
            Virtual_Disk.Disk.Flush();
        }

        public static byte[] readCluster(int clusterIndex)
        {
            Virtual_Disk.Disk.Seek((long)(clusterIndex * 1024), SeekOrigin.Begin);
            byte[] buffer = new byte[1024];
            Virtual_Disk.Disk.Read(buffer, 0, 1024);
            return buffer;
        }
    }
}
