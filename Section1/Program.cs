// Decompiled with JetBrains decompiler
// Type: Section1.Program
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;

namespace Section1
{
    internal class Program
    {
        public static Directory current;
        public static string currentPath;

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to OS_Project_Virtual_DISK_shell ^_^\n\ndeveloped by KHALED ELTURKY\n\n");
            Virtual_Disk.initalize("virtualDisk");
            Mini_FAT.printFAT();
            Program.currentPath = new string(Program.current.dir_name);
            Program.currentPath = Program.currentPath.Trim(char.MinValue, ' ');
            while (true)
            {
                Console.Write(Program.currentPath + "\\>>>>>>");
                Program.current.readDirectory();
                for (int index = 0; index < Program.current.DirOrFiles.Count; ++index)
                    Console.WriteLine(new string(Program.current.DirOrFiles[index].dir_name) + string.Format(" fc={0}", (object)Program.current.DirOrFiles[index].dir_firstCluster));
                Parser.parse(Console.ReadLine());
            }
        }
    }
}
