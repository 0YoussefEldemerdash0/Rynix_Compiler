using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rynix_Compiler
{
    public static class rynix_compiler
    {
        public static Scanner Rynix_Scanner = new Scanner();
       
        public static List<string> Lexemes= new List<string>();
        public static List<Token> TokenStream = new List<Token>();


        public static void Start_Compiling(string SourceCode) 
        {
            //Scanner

            Rynix_Scanner.StartScanning(SourceCode);
            //Parser
            //Sematic Analysis
        } 


        static void SplitLexemes(string SourceCode)
        {
            string[] Lexemes_arr=SourceCode.Split(' ');
            for(int i=0; i< Lexemes_arr.Length; i++)
            {
                if(Lexemes_arr[i].Contains("\r\n"))
                {
                    Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
                }
                Lexemes.Add(Lexemes_arr[i]);
            }

        }

        
    }
}
