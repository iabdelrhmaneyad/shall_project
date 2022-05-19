// Decompiled with JetBrains decompiler
// Type: Section1.Directory
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System.Collections.Generic;

namespace Section1
{
    public class Directory : Directory_Entry
    {
        public List<Directory_Entry> DirOrFiles;
        public Directory parent;

        public Directory(string name, byte dir_attr, int dir_firstCluster, Directory pa)
          : base(name, dir_attr, dir_firstCluster)
        {
            this.DirOrFiles = new List<Directory_Entry>();
            if (pa == null)
                return;
            this.parent = pa;
        }

        public void updateContent(Directory_Entry d)
        {
            int index = this.searchDirectory(new string(d.dir_name));
            if (index == -1)
                return;
            this.DirOrFiles.RemoveAt(index);
            this.DirOrFiles.Insert(index, d);
        }

        public Directory_Entry GetDirectory_Entry() => new Directory_Entry(new string(this.dir_name), this.dir_attr, this.dir_firstCluster);

        public void writeDirectory()
        {
            byte[] bytes1 = new byte[this.DirOrFiles.Count * 32];
            for (int index1 = 0; index1 < this.DirOrFiles.Count; ++index1)
            {
                byte[] bytes2 = Converter.Directory_EntryToBytes(this.DirOrFiles[index1]);
                int index2 = index1 * 32;
                int index3 = 0;
                while (index3 < bytes2.Length)
                {
                    bytes1[index2] = bytes2[index3];
                    ++index3;
                    ++index2;
                }
            }
            List<byte[]> numArrayList = Converter.splitBytes(bytes1);
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
            if (this.parent != null)
            {
                this.parent.updateContent(this.GetDirectory_Entry());
                this.parent.writeDirectory();
            }
            Mini_FAT.writeFAT();
        }

        public void readDirectory()
        {
            if (this.dir_firstCluster == 0)
                return;
            this.DirOrFiles = new List<Directory_Entry>();
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
            for (int index1 = 0; index1 < byteList.Count; ++index1)
            {
                byte[] bytes = new byte[32];
                int index2 = index1 * 32;
                for (int index3 = 0; index3 < bytes.Length && index2 < byteList.Count; ++index2)
                {
                    bytes[index3] = byteList[index2];
                    ++index3;
                }
                if (bytes[0] != (byte)0)
                    this.DirOrFiles.Add(Converter.BytesToDirectory_Entry(bytes));
                else
                    break;
            }
        }

        public void deleteDirectory()
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
            if (this.parent != null)
            {
                int index = this.parent.searchDirectory(new string(this.dir_name));
                if (index != -1)
                {
                    this.parent.DirOrFiles.RemoveAt(index);
                    this.parent.writeDirectory();
                }
            }
            if (Program.current == this && this.parent != null)
            {
                Program.current = this.parent;
                Program.currentPath = Program.currentPath.Substring(0, Program.currentPath.LastIndexOf('\\'));
                Program.current.readDirectory();
            }
            Mini_FAT.writeFAT();
        }

        public int searchDirectory(string name)
        {
            if (name.Length < 11)
            {
                name += "\0";
                for (int index = name.Length + 1; index < 12; ++index)
                    name += " ";
            }
            else
                name = name.Substring(0, 11);
            for (int index = 0; index < this.DirOrFiles.Count; ++index)
            {
                if (new string(this.DirOrFiles[index].dir_name).Equals(name))
                    return index;
            }
            return -1;
        }
    }
}
