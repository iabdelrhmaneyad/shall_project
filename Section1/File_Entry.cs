// Decompiled with JetBrains decompiler
// Type: Section1.File_Entry
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System.Collections.Generic;

namespace Section1
{
    public class File_Entry : Directory_Entry
    {
        public string content;
        public Directory parent;

        public File_Entry(string name, byte dir_attr, int dir_firstCluster, Directory pa)
          : base(name, dir_attr, dir_firstCluster)
        {
            this.content = string.Empty;
            if (pa == null)
                return;
            this.parent = pa;
        }

        public Directory_Entry GetDirectory_Entry() => new Directory_Entry(new string(this.dir_name), this.dir_attr, this.dir_firstCluster);

        public void writeFileContent()
        {
            List<byte[]> numArrayList = Converter.splitBytes(Converter.StringToBytes(this.content));
            int num;
            if (this.dir_firstCluster != 0)
            {
                num = this.dir_firstCluster;
            }
            else
            {
                num = Mini_FAT.getAvilableCluster();
                this.dir_firstCluster = num;
            }
            int clusterIndex = -1;
            for (int index = 0; index < numArrayList.Count; ++index)
            {
                if (num != -1)
                {
                    Virtual_Disk.writeCluster(numArrayList[index], num, count: numArrayList[index].Length);
                    Mini_FAT.setClusterPointer(num, -1);
                    if (clusterIndex != -1)
                        Mini_FAT.setClusterPointer(clusterIndex, num);
                    clusterIndex = num;
                    num = Mini_FAT.getAvilableCluster();
                }
            }
        }

        public void readFileContent()
        {
            if (this.dir_firstCluster == 0)
                return;
            this.content = string.Empty;
            int clusterIndex = this.dir_firstCluster;
            int clusterPointer = Mini_FAT.getClusterPointer(clusterIndex);
            List<byte> byteList = new List<byte>();
            do
            {
                byteList.AddRange((IEnumerable<byte>)Virtual_Disk.readCluster(clusterIndex));
                clusterIndex = clusterPointer;
                if (clusterIndex != -1)
                    clusterPointer = Mini_FAT.getClusterPointer(clusterIndex);
            }
            while (clusterPointer != -1);
            this.content = Converter.BytesToString(byteList.ToArray());
        }

        public void deleteFile()
        {
            if (this.dir_firstCluster != 0)
            {
                int clusterIndex = this.dir_firstCluster;
                int clusterPointer = Mini_FAT.getClusterPointer(clusterIndex);
                do
                {
                    Mini_FAT.setClusterPointer(clusterIndex, 0);
                    clusterIndex = clusterPointer;
                    if (clusterIndex != -1)
                        clusterPointer = Mini_FAT.getClusterPointer(clusterIndex);
                }
                while (clusterIndex != -1);
            }
            if (this.parent == null)
                return;
            int index = this.parent.searchDirectory(new string(this.dir_name));
            if (index != -1)
            {
                this.parent.DirOrFiles.RemoveAt(index);
                this.parent.writeDirectory();
                Mini_FAT.writeFAT();
            }
        }
    }
}
