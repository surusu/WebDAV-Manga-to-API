
using System.Text.Json;

namespace WebDavManga
{

    public class VarDirStorage
    {
        public class NamedVariable
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public NamedVariable(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        private readonly string _folderPath;

        public VarDirStorage(string folderPath)
        {
            _folderPath = folderPath;
        }

        //public static VarDirStorage IDstorage = new VarDirStorage(Path.Combine(CacheFile.CacheDir, "IDStorage"));
        //public static VarDirStorage ChaptersSorage = new VarDirStorage(Path.Combine(CacheFile.CacheDir, "ChapterStorage"));

        public static VarDirStorage Title = new VarDirStorage(Path.Combine(CacheFile.CacheDir, "TitleInfo"));
        
        public static VarDirStorage Chapter = new VarDirStorage(Path.Combine(CacheFile.CacheDir, "ChapterInfo"));
        public static VarDirStorage Pages = new VarDirStorage(Path.Combine(CacheFile.CacheDir, "Pages"));

        public void Save(string name, object value)
        {
            CacheFile.FilePathExists(Path.Combine(_folderPath, name[0] + ".json"));
            // check if the variable has already been saved
            if (VariableExists(name,value))
            {
                Console.WriteLine("Variable with the name {0} already exists.", name);
                return;
            }

            // save the variable to a JSON file
            /*string fileContent = string.Empty;
            if (File.Exists(Path.Combine(_folderPath, name[0] + ".json")))
            {
                fileContent = File.ReadAllText(Path.Combine(_folderPath, name[0] + ".json"));
            }
            var variables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent) ?? new Dictionary<string, object>();
            variables[name] = value;
            File.WriteAllText(Path.Combine(_folderPath, name[0] + ".json"), JsonSerializer.Serialize(variables));
            Console.WriteLine("Variable {0} saved to {1}.json", name, name[0]);*/

            else
            {
                if (File.Exists(Path.Combine(_folderPath, name[0] + ".json")))
                {
                    string fileContent = File.ReadAllText(Path.Combine(_folderPath, name[0] + ".json"));
                    var variables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent);
                    
                    // save the variable to a file
                    //variables = new Dictionary<string, object>();
                    variables[name] = value;
                    string jsonContent = JsonSerializer.Serialize(variables);
                    File.WriteAllText(Path.Combine(_folderPath, name[0] + ".json"), jsonContent);
                    Console.WriteLine("Variable {0} saved to {1}.json", name, name[0]);
                } else
                {
                    // save the variable to a file
                    var variables = new Dictionary<string, object>();
                    variables[name] = value;
                    string jsonContent = JsonSerializer.Serialize(variables);
                    File.WriteAllText(Path.Combine(_folderPath, name[0] + ".json"), jsonContent);
                    Console.WriteLine("Variable {0} saved to {1}.json", name, name[0]);
                }
            }
            
        }

        public bool VariableExists(string name, object value)
        {
            // check if the variable has already been saved
            if (File.Exists(Path.Combine(_folderPath, name[0] + ".json")))
            {
                string fileContent = File.ReadAllText(Path.Combine(_folderPath, name[0] + ".json"));
                var variables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent);
                if (variables.ContainsKey(name))
                {
                    var existingValue = variables[name];
                    if (existingValue == null && value == null)
                    {
                        // both values are null, consider them equal
                        return true;
                    }
                    string existingValueJson = JsonSerializer.Serialize(existingValue);
                    string newValueJson = JsonSerializer.Serialize(value);
                    if (existingValueJson == newValueJson)
                        //if (existingValue != null && existingValue.Equals(newValueJson))
                    {
                        // the existing value is not null and equals the provided value
                        return true;
                    }
                }
            }

            return false;
        }

        public bool VariableExists(string name)
        {
            // check if the variable has already been saved
            if (File.Exists(Path.Combine(_folderPath, name[0] + ".json")))
            {
                string fileContent = File.ReadAllText(Path.Combine(_folderPath, name[0] + ".json"));
                var variables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent);
                if (variables.ContainsKey(name))
                {
                    return true;
                }
            }

            return false;
        }

        public object Load(string name)
        {
            // check if the variable has been saved
            if (!VariableExists(name))
            {
                Console.WriteLine("Variable with the name {0} does not exist.", name);
                return "";
            }

            // read the variable from the file
            string fileContent = File.ReadAllText(Path.Combine(_folderPath, name[0] + ".json"));
            var variables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent);
            return variables[name];
        }

        public Dictionary<string, object> GetAllVariables()
        {
            var variables = new Dictionary<string, object>();

            // read all files in the directory
            foreach (string filePath in Directory.GetFiles(_folderPath, "*.json"))
            {
                string fileContent = File.ReadAllText(filePath);
                var fileVariables = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContent);

                // add all variables in the file to the dictionary
                foreach (KeyValuePair<string, object> kvp in fileVariables)
                {
                    variables[kvp.Key] = kvp.Value;
                }
            }

            return variables;
        }

    }
}
