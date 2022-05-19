// Decompiled with JetBrains decompiler
// Type: Section1.Mini_FAT
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;
using System.Collections.Generic;

namespace Section1
{
    [Serializable]
    public static class Mini_FAT
    {
        public static int[] FAT = new int[1024];

        public static void createFAT()
        {
            for (int index = 0; index < Mini_FAT.FAT.Length; ++index)
            {
                if (index == 0 || index == 4)
                {
                    Mini_FAT.FAT[index] = -1;
                }
                else
                {
                    int num = index <= 0 ? 0 : (index <= 3 ? 1 : 0);
                    Mini_FAT.FAT[index] = num == 0 ? 0 : index + 1;
                }
            }
        }

        public static void writeFAT()
        {
            List<byte[]> numArrayList = Converter.splitBytes(Converter.ToBytes(Mini_FAT.FAT));
            for (int index = 0; index < numArrayList.Count; ++index)
                Virtual_Disk.writeCluster(numArrayList[index], index + 1, count: numArrayList[index].Length);
        }

        public static void readFAT()
        {
            List<byte> byteList = new List<byte>();
            for (int clusterIndex = 1; clusterIndex <= 4; ++clusterIndex)
                byteList.AddRange((IEnumerable<byte>)Virtual_Disk.readCluster(clusterIndex));
            Mini_FAT.FAT = Converter.ToInt(byteList.ToArray());
        }

        public static void printFAT()
        {
            Console.WriteLine("FAT has the following: ");
            for (int index = 0; index < Mini_FAT.FAT.Length; ++index)
                Console.WriteLine("FAT[" + index.ToString() + "] = " + Mini_FAT.FAT[index].ToString());
        }

        public static void setFAT(int[] arr)
        {
            if (arr.Length > 1024)
                return;
            Mini_FAT.FAT = arr;
        }

        public static int getAvilableCluster()
        {
            for (int avilableCluster = 0; avilableCluster < Mini_FAT.FAT.Length; ++avilableCluster)
            {
                if (Mini_FAT.FAT[avilableCluster] == 0)
                    return avilableCluster;
            }
            return -1;
        }

        public static void setClusterPointer(int clusterIndex, int pointer) => Mini_FAT.FAT[clusterIndex] = pointer;

        public static int getClusterPointer(int clusterIndex) => clusterIndex >= 0 && clusterIndex < Mini_FAT.FAT.Length ? Mini_FAT.FAT[clusterIndex] : -1;
    }
}
