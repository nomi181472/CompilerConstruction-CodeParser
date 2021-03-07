using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CC_A1
{
    public class CodeGenerator
    {
        private string filePath { get; set; }
        private string codeExecuting { get; set; }
        public string error { get; set; }
        private List<string> declareVariable = new List<string>();
        private CodeMapper cM { get; set; }
        Dictionary<string, string> allTypes = new Dictionary<string, string>
        {
            { "int", "int" },
            { "float", "float" },
            { "string", "string" },
            { "double", "double" },
            { "bool", "bool" }
        };
        public CodeGenerator()
        {
            error = "";
        }
        public void TypeChecking(string fromFile)
        {
         
            if (this.allTypes.Keys.Contains(fromFile)) {
                return;
            }
            this.Terminate(fromFile + " type does not exist");
            
        }
        public void Terminate(string str)
        {
            Console.WriteLine(str);
            System.Environment.Exit(0);
        }
        public void IdentifierChecking(string ff)
        {
            
            string pattern = "^([a-zA-Z_$][a-zA-Z\\d_$]*)$";
            Regex reg = new Regex(pattern);
            
            if (reg.IsMatch(ff))
            {
                
                return;
            }
            
            this.Terminate("identifer syntax error"+ ff);
            
        }

        public void readFile(string fileName)
        {
            filePath = fileName;
            string jsonString = File.ReadAllText(fileName);
            cM = JsonSerializer.Deserialize<CodeMapper>(jsonString);
        }
        public void CheckDuplicateIdentifer(string fromFile)
        {
            
            if(this.declareVariable.Contains(fromFile))
            {
                this.Terminate(fromFile + " variable already declared");
            }
            this.declareVariable.Add(fromFile);
            return;


        }
        public string RemoveWhiteSpace(string str)
        {
            return str
                .ToCharArray()
                .Where(e => !Char.IsWhiteSpace(e))
                .Select(e => e.ToString()).Aggregate((a, b) => a + b);
        }
        public  void checkStatement(Conditions[] arr)
        {
            
            Regex re = new Regex(@"((([_a-zA-Z_]+[0-9]*)([\[]([_a-zA-Z_]|[0-9])[\]])?|([0-9]+))(([<>])|([<>!=][=]))(([_a-zA-z_]+[0-9]*)|[0-9]+))((([&]{2}|[\|]{2}))((([_a-zA-Z_]+[0-9]*)|([0-9]+))(([<>])|([<>=][=]))(([_a-zA-z_]+[0-9]*)|[0-9]+))){0,10}");
            //((([_a-zA-Z_]+[0-9]*)|([0-9]+))(([<>])|([<>=][=]))(([_a-zA-z_]+[0-9]*)|[0-9]+))((([&]{2}|[\|]{2}))((([_a-zA-Z_]+[0-9]*)|([0-9]+))(([<>])|([<>=][=]))(([_a-zA-z_]+[0-9]*)|[0-9]+))){0,10}
            for (int i = 0; i < arr.Length ; i++)
            {
                var a = "" + arr[i].condition + arr[i].action + "";
               
               
                if (arr[i].condition != "")
                {
                   
                    var str = RemoveWhiteSpace(arr[i].condition);
                   // Console.WriteLine(re.Match(str));
                    if (re.Match(str).Length!= str.Length )
                    {
                        error += "\nerror in condition: " + str;
                    }
                }     
            }
        }
        private string  LineSeparator(string arr)
        {
            int index = arr.IndexOf("&");
            string[] temp= arr.Split("&");
            string temp2 = "";
            
            for(int i=0;i< temp.Count(); i++) {
               
                if (i==(temp.Count()- 1))
                {
                    temp2 += "return" + temp[i];

                }
                else
                {
                    temp2 +=  temp[i]+";\n";
                }
                
            }

            
            return temp2;
        }
        public string generateCode()
        {


            this.TypeChecking(cM.returntype);


            this.IdentifierChecking(cM.function);


            this.CheckDuplicateIdentifer(cM.function);

            this.codeExecuting = cM.returntype + " " + cM.function + " (";

            foreach (var i in cM.parameters)
            {


                this.CheckDuplicateIdentifer(i.parameter);
                this.TypeChecking(i.type);

                this.IdentifierChecking(i.parameter);

                this.codeExecuting += " " + i.type + " " + i.parameter + " ,";

            }
            this.codeExecuting = this.codeExecuting.Substring(0, this.codeExecuting.Length - 1);
            string _return = " \nreturn ";
            string termination = ";\n";
            var arr = cM.conditions.ToArray();
           
            this.checkStatement(arr);
            this.codeExecuting += " ){\n if ( " + arr[0].condition + " ){ " + _return + arr[0].action + termination+"}\n";
         
            string ifElse = "else if (";
            for (int i = 1; i < arr.Length - 1; i++)
            {
                if (arr[i].action.Contains("="))
                    this.codeExecuting += ifElse + arr[i].condition + " ){\n" + arr[i].action + termination + "}\n";
                else if (arr[i].action.Contains("&"))
                    this.codeExecuting += ifElse + arr[i].condition + " ){\n" + this.LineSeparator(arr[i].action) + termination + "}\n";
                else
                    this.codeExecuting += ifElse + arr[i].condition + " ) {\n" + _return + arr[i].action + termination + "}\n";
            }
            if (arr[arr.Length - 1].condition == "")
            {
                this.codeExecuting += "else " + _return + arr[arr.Length - 1].action + termination + "}\n";
            }
            else if (arr[arr.Length - 1].action.Contains("&"))
            {
                this.codeExecuting += ifElse + arr[arr.Length - 1].condition + " ){\n" + this.LineSeparator(arr[arr.Length - 1].action) + termination + "}\n";
            }
            else
            {
                this.codeExecuting += ifElse + arr[arr.Length - 1].condition + "){\n" + _return + arr[arr.Length - 1].action + termination + "}\n";
            }

            return this.codeExecuting;
        
            
        }
    }
}
