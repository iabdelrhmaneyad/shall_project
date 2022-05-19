// Decompiled with JetBrains decompiler
// Type: Section1.Parser
// Assembly: Section1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7E1CB3E4-DE59-4569-BCDA-85B9AFBAC86E
// Assembly location: C:\Users\Abdelrhman-Eyad\Desktop\Debug\Section1.exe

using System;
using System.Collections.Generic;

namespace Section1
{
    public static class Parser
    {
        public static void parse(string input)
        {
            List<Token> tokens = Tokenizer.GetTokens(input);
            if (tokens == null)
                return;
            if (tokens[0].key == TokenType.Not_Recognized)
                Console.WriteLine(tokens[0].value + " is not recognized as an internal or external command,operable program or batch file.");
            else if (tokens[0].key == TokenType.Command)
            {
                switch (tokens[0].value)
                {
                    case "cd":
                        Commands.changeDirectory(tokens);
                        break;
                    case "cls":
                        Commands.clear(tokens);
                        break;
                    case "dir":
                        Commands.listDirectory(tokens);
                        break;
                    case "help":
                        Commands.help(tokens);
                        break;
                    case "md":
                        Commands.createDirectory(tokens);
                        break;
                    case "quit":
                        Commands.quit(tokens);
                        break;
                    case "rd":
                        Commands.removeDirectory(tokens);
                        break;
                }
            }
        }
    }
}
