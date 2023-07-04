using System.Text.Json;

namespace WebDavManga
{

    

    public class VarStorage
    {
        [Serializable]
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

        private readonly string _filePath;

        public VarStorage(string filePath)
        {
            _filePath = filePath;
        }

        public static VarStorage IDstorage = new VarStorage(Path.Combine(CacheFile.CacheDir, "IDStorage.dat"));

        public void Save(string name, object variable)
        {
            List<NamedVariable> namedVariables = LoadNamedVariables();

            var namedVariable = namedVariables.Find(nv => nv.Name == name);
            if (namedVariable != null)
            {
                namedVariable.Value = variable;
            }
            else
            {
                namedVariable = new NamedVariable(name, variable);
                namedVariables.Add(namedVariable);
            }

            SaveNamedVariables(namedVariables);
        }

        public T Load<T>(string name)
        {
            List<NamedVariable> namedVariables = LoadNamedVariables();
            var namedVariable = namedVariables.Find(nv => nv.Name == name);
            if (namedVariable == null)
            {
                Console.WriteLine($"No variable found with name '{name}'");
                object result = "";
                return (T)result;
            }
            return (T)namedVariable.Value;
        }

        private List<NamedVariable> LoadNamedVariables()
        {
            if (!File.Exists(_filePath))
            {
                return new List<NamedVariable>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<NamedVariable>>(json);
        }

        private void SaveNamedVariables(List<NamedVariable> namedVariables)
        {
            string json = JsonSerializer.Serialize(namedVariables);
            File.WriteAllText(_filePath, json);
        }
    }
}
