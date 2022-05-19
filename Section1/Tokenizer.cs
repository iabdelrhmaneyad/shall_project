// Decompiled with JetBrains decompiler
// Type: Section1.Tokenizer
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System.Collections.Generic;
using System.Linq;

namespace Section1
{
    public static class Tokenizer
    {
        private static Token generateToken(string arg, TokenType tokenType)
        {
            Token token;
            token.key = tokenType;
            token.value = arg;
            return token;
        }

        private static bool checkArg(string arg) => arg == "cd" || arg == "cls" || arg == "dir" || arg == "quit" || arg == "copy" || arg == "del" || arg == "help" || arg == "md" || arg == "rd" || arg == "rename" || arg == "type" || arg == "import" || arg == "export";

        private static bool isFullPathd(string arg) => (arg.Contains(":") || arg.Contains("\\")) && !arg.Contains<char>('.');

        private static bool isFullPathf(string arg) => (arg.Contains(":") || arg.Contains("\\")) && arg.Contains<char>('.');

        private static bool isFileName(string arg) => arg.Contains<char>('.');

        public static List<Token> GetTokens(string input)
        {
            List<Token> tokens = new List<Token>();
            if (input.Length == 0)
                return (List<Token>)null;
            string[] strArray = input.Split(' ');
            List<string> stringList = new List<string>();
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index] != "" && strArray[index] != " ")
                    stringList.Add(strArray[index]);
            }
            string[] array = stringList.ToArray();
            array[0] = array[0].ToLower();
            switch (array[0])
            {
                case "cd":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    if (array.Length == 2)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        if (Tokenizer.isFullPathd(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToDirectory));
                            break;
                        }
                        if (Tokenizer.isFullPathf(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToFile));
                            break;
                        }
                        if (Tokenizer.isFileName(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FileName));
                            break;
                        }
                        tokens.Add(Tokenizer.generateToken(array[1], TokenType.DirName));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "cls":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    if (array.Length > 1)
                    {
                        for (int index = 1; index < array.Length; ++index)
                            tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                        break;
                    }
                    break;
                case "copy":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                case "del":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                case "dir":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    if (array.Length == 2)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        if (Tokenizer.isFullPathd(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToDirectory));
                            break;
                        }
                        if (Tokenizer.isFullPathf(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToFile));
                            break;
                        }
                        if (Tokenizer.isFileName(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FileName));
                            break;
                        }
                        tokens.Add(Tokenizer.generateToken(array[1], TokenType.DirName));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "export":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                case "help":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    if (array.Length == 2)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        array[1] = array[1].ToLower();
                        if (Tokenizer.checkArg(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.Command));
                            break;
                        }
                        tokens.Add(Tokenizer.generateToken(array[1], TokenType.Not_Recognized));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "import":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                case "md":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    if (array.Length == 2)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        if (Tokenizer.isFullPathd(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToDirectory));
                            break;
                        }
                        if (Tokenizer.isFullPathf(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToFile));
                            break;
                        }
                        if (Tokenizer.isFileName(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FileName));
                            break;
                        }
                        tokens.Add(Tokenizer.generateToken(array[1], TokenType.DirName));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "quit":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "rd":
                    if (array.Length == 1)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        break;
                    }
                    if (array.Length == 2)
                    {
                        tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                        if (Tokenizer.isFullPathd(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToDirectory));
                            break;
                        }
                        if (Tokenizer.isFullPathf(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FullPathToFile));
                            break;
                        }
                        if (Tokenizer.isFileName(array[1]))
                        {
                            tokens.Add(Tokenizer.generateToken(array[1], TokenType.FileName));
                            break;
                        }
                        tokens.Add(Tokenizer.generateToken(array[1], TokenType.DirName));
                        break;
                    }
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    for (int index = 1; index < array.Length; ++index)
                        tokens.Add(Tokenizer.generateToken(array[index], TokenType.Not_Recognized));
                    break;
                case "rename":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                case "type":
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Command));
                    break;
                default:
                    tokens.Add(Tokenizer.generateToken(array[0], TokenType.Not_Recognized));
                    break;
            }
            return tokens;
        }
    }
}
