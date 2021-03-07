using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CC_A1
{
    
    

   

    
    class Program
    {
        

        static void Main(string[] args)
        {
            int index = Directory.GetCurrentDirectory().IndexOf("bin");

            string file_name = @"palindrome.json";
           
            try
            {

                string current_path = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory().Substring(0, index - 1) );
                string path = current_path + "\\" + file_name;
                CodeGenerator cg = new CodeGenerator();
                cg.readFile(path);
                // string jsonString = File.ReadAllText(fileName);
                string code = cg.generateCode();

                Console.WriteLine(code);
                Console.WriteLine(cg.error);
                File.WriteAllText(current_path+"\\" + file_name.Substring(0,file_name.Length-5) + "-Output.cpp", code);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }




        }
    }
}
