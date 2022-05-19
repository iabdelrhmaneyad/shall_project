// Decompiled with JetBrains decompiler
// Type: Section1.Commands
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;
using System.Collections.Generic;

namespace Section1
{
    public static class Commands
    {
        public static void clear(List<Token> tokens)
        {
            if (tokens.Count > 1)
                Console.WriteLine("Error: " + tokens[0].value + " command syntax is \n cls \n function: Clear the screen.");
            else
                Console.Clear();
        }

        public static void quit(List<Token> tokens)
        {
            if (tokens.Count > 1)
            {
                Console.WriteLine("Error: " + tokens[0].value + " command syntax is \n quit \n function: Quit the shell.");
            }
            else
            {
                Mini_FAT.writeFAT();
                Virtual_Disk.Disk.Close();
                Environment.Exit(0);
            }
        }

        public static void createDirectory(List<Token> tokens)
        {
            if (tokens.Count <= 1 || tokens.Count > 2)
                Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n md [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
            else if (tokens[1].key == TokenType.DirName)
            {
                if (Program.current.searchDirectory(tokens[1].value) == -1)
                {
                    if (Mini_FAT.getAvilableCluster() != -1)
                    {
                        Directory_Entry directoryEntry = new Directory_Entry(tokens[1].value, (byte)16, 0);
                        Program.current.DirOrFiles.Add(directoryEntry);
                        Program.current.writeDirectory();
                        if (Program.current.parent != null)
                        {
                            Program.current.parent.updateContent(Program.current.GetDirectory_Entry());
                            Program.current.parent.writeDirectory();
                        }
                        Mini_FAT.writeFAT();
                    }
                    else
                        Console.WriteLine("Error : sorry the disk is full!");
                }
                else
                    Console.WriteLine("Error : this directory \" " + tokens[1].value + " \" is already exists!");
            }
            else if (tokens[1].key == TokenType.FullPathToDirectory)
            {
                Directory directory = Commands.moveTodir(tokens[1], false);
                if (directory == null)
                    Console.WriteLine("Error : this path \" " + tokens[1].value + " \" is not exists!");
                else if (Mini_FAT.getAvilableCluster() != -1)
                {
                    string[] strArray = tokens[1].value.Split('\\');
                    Directory_Entry directoryEntry = new Directory_Entry(strArray[strArray.Length - 1], (byte)16, 0);
                    directory.DirOrFiles.Add(directoryEntry);
                    directory.writeDirectory();
                    directory.parent.updateContent(directory.GetDirectory_Entry());
                    directory.parent.writeDirectory();
                    Mini_FAT.writeFAT();
                }
                else
                    Console.WriteLine("Error : sorry the disk is full!");
            }
            else
                Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n md [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
        }

        public static File_Entry createFile(Token token)
        {
            if (token.key == TokenType.FileName)
            {
                if (Program.current.searchDirectory(token.value) == -1)
                {
                    if (Mini_FAT.getAvilableCluster() != -1)
                    {
                        Directory_Entry directoryEntry = new Directory_Entry(token.value, (byte)0, 0);
                        Program.current.DirOrFiles.Add(directoryEntry);
                        Program.current.writeDirectory();
                        if (Program.current.parent != null)
                        {
                            Program.current.parent.updateContent(Program.current.GetDirectory_Entry());
                            Program.current.parent.writeDirectory();
                        }
                        Mini_FAT.writeFAT();
                        return new File_Entry(token.value, (byte)0, 0, Program.current);
                    }
                    Console.WriteLine("Error : sorry the disk is full!");
                }
                else
                    Console.WriteLine("Error : this file name \" " + token.value + " \" is already exists!");
            }
            else if (token.key == TokenType.FullPathToFile)
            {
                Directory pa = Commands.moveTodir(new Token()
                {
                    value = token.value.Substring(0, token.value.LastIndexOf('\\')),
                    key = token.key
                }, false);
                if (pa == null)
                {
                    Console.WriteLine("Error : this path \" " + token.value + " \" is not exists!");
                }
                else
                {
                    if (Mini_FAT.getAvilableCluster() != -1)
                    {
                        string[] strArray = token.value.Split('\\');
                        Directory_Entry directoryEntry = new Directory_Entry(strArray[strArray.Length - 1], (byte)16, 0);
                        pa.DirOrFiles.Add(directoryEntry);
                        pa.writeDirectory();
                        pa.parent.updateContent(pa.GetDirectory_Entry());
                        pa.parent.writeDirectory();
                        Mini_FAT.writeFAT();
                        return new File_Entry(strArray[strArray.Length - 1], (byte)0, 0, pa);
                    }
                    Console.WriteLine("Error : sorry the disk is full!");
                }
            }
            return (File_Entry)null;
        }

        private static Directory moveTodir(Token token, bool changedirFlag)
        {
            Directory directory1 = (Directory)null;
            if (token.key == TokenType.DirName)
            {
                if (token.value != "..")
                {
                    int index = Program.current.searchDirectory(token.value);
                    if (index == -1)
                        return (Directory)null;
                    string name = new string(Program.current.DirOrFiles[index].dir_name);
                    byte dirAttr = Program.current.DirOrFiles[index].dir_attr;
                    int dirFirstCluster = Program.current.DirOrFiles[index].dir_firstCluster;
                    directory1 = new Directory(name, dirAttr, dirFirstCluster, Program.current);
                    directory1.readDirectory();
                    string str = Program.currentPath + "\\" + name.Trim(char.MinValue, ' ');
                    if (changedirFlag)
                        Program.currentPath = str;
                }
                else if (Program.current.parent != null)
                {
                    directory1 = Program.current.parent;
                    directory1.readDirectory();
                    string currentPath = Program.currentPath;
                    string str = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
                    if (changedirFlag)
                        Program.currentPath = str;
                }
                else
                {
                    directory1 = Program.current;
                    directory1.readDirectory();
                }
            }
            else if (token.key == TokenType.FullPathToDirectory)
            {
                string[] strArray = token.value.Split('\\');
                List<string> stringList = new List<string>();
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (strArray[index] != "")
                        stringList.Add(strArray[index]);
                }
                Directory directory2 = new Directory("K:", (byte)16, 5, (Directory)null);
                directory2.readDirectory();
                if (stringList[0].ToLower().Equals("k:"))
                {
                    string str = "k:";
                    int num = changedirFlag ? stringList.Count : stringList.Count - 1;
                    for (int index1 = 1; index1 < num; ++index1)
                    {
                        int index2 = directory2.searchDirectory(stringList[index1]);
                        if (index2 == -1)
                            return (Directory)null;
                        Directory pa = directory2;
                        string name = new string(directory2.DirOrFiles[index2].dir_name);
                        byte dirAttr = directory2.DirOrFiles[index2].dir_attr;
                        int dirFirstCluster = directory2.DirOrFiles[index2].dir_firstCluster;
                        directory2 = new Directory(name, dirAttr, dirFirstCluster, pa);
                        directory2.readDirectory();
                        str = str + "\\" + name.Trim(char.MinValue, ' ');
                    }
                    directory1 = directory2;
                    if (changedirFlag)
                        Program.currentPath = str;
                }
                else
                {
                    if (!(stringList[0] == ".."))
                        return (Directory)null;
                    directory1 = Program.current;
                    for (int index = 0; index < stringList.Count; ++index)
                    {
                        if (directory1.parent != null)
                        {
                            directory1 = directory1.parent;
                            directory1.readDirectory();
                            string currentPath = Program.currentPath;
                            string str = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
                            if (changedirFlag)
                                Program.currentPath = str;
                        }
                        else
                        {
                            directory1 = Program.current;
                            directory1.readDirectory();
                            break;
                        }
                    }
                }
            }
            return directory1;
        }

        public static void changeDirectory(List<Token> tokens)
        {
            if (tokens.Count == 1)
                Console.WriteLine(Program.currentPath);
            else if (tokens.Count == 2)
            {
                if (tokens[1].key == TokenType.DirName || tokens[1].key == TokenType.FullPathToDirectory)
                {
                    Directory directory = Commands.moveTodir(tokens[1], true);
                    if (directory != null)
                    {
                        directory.readDirectory();
                        Program.current = directory;
                    }
                    else
                        Console.WriteLine("Error : this path \" " + tokens[1].value + " \" is not exists!");
                }
                else
                    Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n cd \n or \n cd [directory]\n[directory] can be a new directory name or fullpath of a directory");
            }
            else
                Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n cd \n or \n cd [directory]\n[directory] can be a new directory name or fullpath of a directory");
        }

        public static void help(List<Token> tokens)
        {
            if (tokens.Count > 2)
                Console.WriteLine("Error: " + tokens[0].value + " command syntax is \n help \n or \n help [command] \n function:Provides Help information for commands.");
            else if (tokens.Count == 2)
            {
                if (tokens[1].key == TokenType.Command)
                {
                    string s = tokens[1].value;

                    if (!(s == "cls"))
                        Console.WriteLine("Clear the screen.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n cls");
                    if (!(s == "import"))
                        Console.WriteLine("– import text file(s) from your computer ");
                    Console.WriteLine(tokens[1].value + " command syntax is \n import [destination] [file]+");
                    Console.WriteLine("+ after [file] represent that you can pass more than file Name (or fullpath of file) of text file");
                    Console.WriteLine("[file] can be file Name (or fullpath of file) of text file");
                    Console.WriteLine("[destination] can be directory name or fullpath of a directory in your implemented file system");
                    if (!(s == nameof(help)))
                        Console.WriteLine("Provides Help information for commands.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n help \n or \n For more information on a specific command, type help [command]");
                    Console.WriteLine("command - displays help information on that command.");
                    if (!(s == "quit"))
                        Console.WriteLine("Quit the shell.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n quit");
                    if (!(s == "type"))
                        Console.WriteLine("Displays the contents of a text file.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n type [file]");
                    Console.WriteLine("NOTE: it displays the filename before its content for every file");
                    Console.WriteLine("[file] can be file Name (or fullpath of file) of text file");
                    if (!(s == "rd"))
                        Console.WriteLine("Removes a directory.");
                    Console.WriteLine("NOTE: it confirms the user choice to delete the directory before deleting");
                    Console.WriteLine(tokens[1].value + " command syntax is \n rd [directory]");
                    Console.WriteLine("[directory] can be a directory name or fullpath of a directory");
                    if (!(s == "cd"))
                        Console.WriteLine("Change the current default directory to the directory given in the argument.");
                    Console.WriteLine("If the argument is not present, report the current directory.");
                    Console.WriteLine("If the directory does not exist an appropriate error should be reported.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n cd \n or \n cd [directory]");
                    Console.WriteLine("[directory] can be directory name or fullpath of a directory");
                    if (!(s == "md"))
                        Console.WriteLine("Creates a directory.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n md [directory]");
                    Console.WriteLine("[directory] can be a new directory name or fullpath of a new directory");
                    if (!(s == "rename"))
                        Console.WriteLine("Renames a file.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n rd [fileName] [new fileName]");
                    Console.WriteLine("[fileName] can be a file name or fullpath of a filename ");
                    Console.WriteLine("[new fileName] can be a new file name not fullpath ");
                    if (!(s == "del"))
                        Console.WriteLine("Deletes one or more files.");
                    Console.WriteLine("NOTE: it confirms the user choice to delete the file before deleting");
                    Console.WriteLine(tokens[1].value + " command syntax is \n del [file]+");
                    Console.WriteLine("+ after [file] represent that you can pass more than file Name (or fullpath of file)");
                    Console.WriteLine("[file] can be file Name (or fullpath of file)");
                    if (!(s == "copy"))
                        Console.WriteLine("Copies one or more files to another location.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n copy [source]+ [destination]");
                    Console.WriteLine("+ after [source] represent that you can pass more than file Name (or fullpath of file) or more than directory Name (or fullpath of directory)");
                    Console.WriteLine("[source] can be file Name (or fullpath of file) or directory Name (or fullpath of directory)");
                    Console.WriteLine("[destination] can be directory name or fullpath of a directory");
                    if (!(s == "dir"))
                        Console.WriteLine("List the contents of directory given in the argument.");
                    Console.WriteLine("If the argument is not present, list the content of the current directory.");
                    Console.WriteLine("If the directory does not exist an appropriate error should be reported.");
                    Console.WriteLine(tokens[1].value + " command syntax is \n dir \n or \n dir [directory]");
                    Console.WriteLine("[directory] can be directory name or fullpath of a directory");
                    if (!(s == "export"))
                        Console.WriteLine("– export text file(s) to your computer ");
                    Console.WriteLine(tokens[1].value + " command syntax is \n export [destination] [file]+");
                    Console.WriteLine("+ after [file] represent that you can pass more than file Name (or fullpath of file) of text file");
                    Console.WriteLine("[file] can be file Name (or fullpath of file) of text file in your implemented file system");
                    Console.WriteLine("[destination] can be directory name or fullpath of a directory in your computer");
                }
            
        
                else
                    Console.WriteLine("Error: " + tokens[1].value + " => This command is not supported by the help utility.");
        }
            else
            {
                if (tokens.Count != 1)
                    return;
                Console.WriteLine("cd       - Change the current default directory to .");
                Console.WriteLine("           If the argument is not present, report the current directory.");
                Console.WriteLine("           If the directory does not exist an appropriate error should be reported.");
                Console.WriteLine("cls      - Clear the screen.");
                Console.WriteLine("dir      - List the contents of directory .");
                Console.WriteLine("quit     - Quit the shell.");
                Console.WriteLine("copy     - Copies one or more files to another location");
                Console.WriteLine("del      - Deletes one or more files.");
                Console.WriteLine("help     - Provides Help information for commands.");
                Console.WriteLine("md       - Creates a directory.");
                Console.WriteLine("rd       - Removes a directory.");
                Console.WriteLine("rename   - Renames a file.");
                Console.WriteLine("type     - Displays the contents of a text file.");
                Console.WriteLine("import   – import text file(s) from your computer");
                Console.WriteLine("export   – export text file(s) to your computer");
            }
        }

        public static void listDirectory(List<Token> tokens)
        {
            if (tokens.Count == 1)
            {
                int num1 = 0;
                int num2 = 0;
                int num3 = 0;
                Console.WriteLine(" Directory of " + Program.currentPath);
                Console.WriteLine();
                int num4 = 1;
                if (Program.current.parent != null)
                {
                    num4 = 2;
                    Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)".");
                    int num5 = num2 + 1;
                    Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)"..");
                    num2 = num5 + 1;
                }
                for (int index = num4; index < Program.current.DirOrFiles.Count; ++index)
                {
                    if (Program.current.DirOrFiles[index].dir_attr == (byte)0)
                    {
                        Console.WriteLine("\t{0:9}{1:11}", (object)Program.current.DirOrFiles[index].dir_filesize, (object)new string(Program.current.DirOrFiles[index].dir_name));
                        ++num1;
                        num3 += Program.current.DirOrFiles[index].dir_filesize;
                    }
                    else if (Program.current.DirOrFiles[index].dir_attr == (byte)16)
                    {
                        Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)new string(Program.current.DirOrFiles[index].dir_name));
                        ++num2;
                    }
                }
                Console.WriteLine(string.Format("{0}{1} File(s)    {2} bytes", (object)"              ", (object)num1, (object)num3));
                Console.WriteLine(string.Format("{0}{1} Dir(s)    {2} bytes free", (object)"              ", (object)num2, (object)Virtual_Disk.getFreeSpace()));
            }
            else if (tokens.Count == 2)
            {
                if (tokens[1].key == TokenType.DirName || tokens[1].key == TokenType.FullPathToDirectory)
                {
                    Directory directory = Commands.moveTodir(tokens[1], false);
                    if (directory != null)
                    {
                        directory.readDirectory();
                        int num6 = 0;
                        int num7 = 0;
                        int num8 = 0;
                        if (tokens[1].key == TokenType.DirName)
                            Console.WriteLine(" Directory of " + Program.currentPath + "\\" + tokens[1].value);
                        else
                            Console.WriteLine(" Directory of " + tokens[1].value);
                        Console.WriteLine();
                        int num9 = 1;
                        if (directory.parent != null)
                        {
                            num9 = 2;
                            Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)".");
                            int num10 = num7 + 1;
                            Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)"..");
                            num7 = num10 + 1;
                        }
                        for (int index = num9; index < directory.DirOrFiles.Count; ++index)
                        {
                            if (directory.DirOrFiles[index].dir_attr == (byte)0)
                            {
                                Console.WriteLine("\t{0:9} {1:11}", (object)directory.DirOrFiles[index].dir_filesize, (object)new string(directory.DirOrFiles[index].dir_name));
                                ++num6;
                                num8 += directory.DirOrFiles[index].dir_filesize;
                            }
                            else if (directory.DirOrFiles[index].dir_attr == (byte)16)
                            {
                                Console.WriteLine("{0}{1:11}", (object)"\t<DIR>    ", (object)new string(directory.DirOrFiles[index].dir_name));
                                ++num7;
                            }
                        }
                        Console.WriteLine(string.Format("{0}{1} File(s)    {2} bytes", (object)"              ", (object)num6, (object)num8));
                        Console.WriteLine(string.Format("{0}{1} Dir(s)    {2} bytes free", (object)"              ", (object)num7, (object)Virtual_Disk.getFreeSpace()));
                    }
                    else
                        Console.WriteLine("Error : this path \" " + tokens[1].value + " \" is not exists!");
                }
                else
                    Console.WriteLine("Error: " + tokens[0].value + " command syntax is \n dir \n or \n dir [directory]\n[directory] can be a new directory name or fullpath of a directory");
            }
            else
                Console.WriteLine("Error: " + tokens[0].value + " command syntax is \n dir \n or \n dir [directory]\n[directory] can be a new directory name or fullpath of a directory");
        }

        public static void removeDirectory(List<Token> tokens)
        {
            if (tokens.Count <= 1 || tokens.Count > 2)
                Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n rd [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
            else if (tokens[1].key == TokenType.DirName || tokens[1].key == TokenType.FullPathToDirectory)
            {
                Directory directory = Commands.moveTodir(tokens[1], false);
                if (directory != null)
                {
                    Console.Write("Are you sure that you want to delete " + new string(directory.dir_name).Trim(char.MinValue, ' ') + " , please enter Y for yes or N for no:");
                    if (Console.ReadLine().ToLower().Equals("y"))
                        directory.deleteDirectory();
                }
                else
                    Console.WriteLine("Error : this directory \" " + tokens[1].value + " \" is not exists!");
            }
            else
                Console.WriteLine("Error: " + tokens[0].value + "command syntax is \n rd [directory]\n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
        }
    }
}
