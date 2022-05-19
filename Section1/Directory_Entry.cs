// Decompiled with JetBrains decompiler
// Type: Section1.Directory_Entry
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;

namespace Section1
{
    [Serializable]
    public class Directory_Entry
    {
        public char[] dir_name = new char[11];
        public byte dir_attr;
        public byte[] dir_empty = new byte[12];
        public int dir_firstCluster;
        public int dir_filesize;

        public Directory_Entry()
        {
        }

        public Directory_Entry(string name, byte dir_attr, int dir_firstCluster)
        {
            this.dir_attr = dir_attr;
            switch (dir_attr)
            {
                case 0:
                    string[] strArray = name.Split('.');
                    this.assignFileName(strArray[0].ToCharArray(), strArray[1].ToCharArray());
                    break;
                case 16:
                    this.assignDIRName(name.ToCharArray());
                    break;
            }
            this.dir_firstCluster = dir_firstCluster;
        }

        public void assignFileName(char[] name, char[] extension)
        {
            if (name.Length <= 7 && extension.Length == 3)
            {
                int num1 = 0;
                for (int index = 0; index < name.Length; ++index)
                {
                    ++num1;
                    this.dir_name[index] = name[index];
                }
                int index1 = num1 + 1;
                this.dir_name[index1] = '.';
                for (int index2 = 0; index2 < extension.Length; ++index2)
                {
                    ++index1;
                    this.dir_name[index1] = extension[index2];
                }
                int num2;
                for (int index3 = num2 = index1 + 1; index3 < this.dir_name.Length; ++index3)
                    this.dir_name[index3] = ' ';
            }
            else
            {
                for (int index = 0; index < 7; ++index)
                    this.dir_name[index] = name[index];
                this.dir_name[7] = '.';
                int index4 = 0;
                int index5 = 8;
                for (; index4 < extension.Length; ++index4)
                {
                    this.dir_name[index5] = extension[index4];
                    ++index5;
                }
            }
        }

        public void assignDIRName(char[] name)
        {
            if (name.Length <= 11)
            {
                int num1 = 0;
                for (int index = 0; index < name.Length; ++index)
                {
                    ++num1;
                    this.dir_name[index] = name[index];
                }
                int num2;
                for (int index = num2 = num1 + 1; index < this.dir_name.Length; ++index)
                    this.dir_name[index] = ' ';
            }
            else
            {
                int num = 0;
                for (int index = 0; index < 11; ++index)
                {
                    ++num;
                    this.dir_name[index] = name[index];
                }
            }
        }
    }
}
