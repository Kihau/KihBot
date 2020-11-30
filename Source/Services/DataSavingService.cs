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
    public class DataSavingService
    {
        private List<IData> Data { get; set; }

        public DataSavingService(IServiceProvider services)
        {
            this.Data = services.GetService<List<IData>>();
        }

        public void Serialize()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            foreach(var set in Data)
            {
                var json = JsonSerializer.Serialize(set, set.GetType(), options);

                if (!File.Exists(set.FileName))
                    File.Create(set.FileName).Close();

                File.WriteAllText(set.FileName, json);
            }
        }

        public void Deserialize()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                var json = File.ReadAllText(Data[i].FileName);
                Data[i] = (IData)JsonSerializer.Deserialize(json, Data[i].GetType());
            }

        }
    }
}
