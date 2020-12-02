using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KihBot.Data;
using Microsoft.Extensions.DependencyInjection;

namespace KihBot.Services
{
    /// <summary>
    /// A class used to serialize/deserialize and manage data
    /// TO DO: Add auto serializing timer with interval of 1h (autosaving)
    /// </summary>
    public class DataService
    {
        public List<IData> DataList { get; set; }

        public DataService(IServiceProvider services)
        {
            DataList = new List<IData>()
            {
                services.GetRequiredService<ConfigData>(),
                services.GetRequiredService<FunData>()
            };
        }

        public void SerializeData()
        {
            foreach (var set in DataList)
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(set, set.GetType(), options);

                if (!File.Exists(set.FileName))
                    File.Create(set.FileName).Close();

                File.WriteAllText(set.FileName, json);
            }
        }

        public void DeserializeData()
        {
            foreach(var set in DataList)
            {
                var json = File.ReadAllText(set.FileName);
                var data = (IData)JsonSerializer.Deserialize(json, set.GetType());
                set.LoadData(data);
            }
        }

    }
}
