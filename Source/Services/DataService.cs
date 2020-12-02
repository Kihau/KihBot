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
        private IServiceProvider Services { get; set;  }

        public DataService(IServiceProvider services)
        {
            this.Services = services;
        }

        public void Serialize()
        {
            SerializeSingle<ConfigData>("config.json");
            SerializeSingle<FunData>("fundata.json");
        }

        public void SerializeSingle<T>(string file)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(Services.GetRequiredService<T>(), typeof(T), options);

            if (!File.Exists(file))
                File.Create(file).Close();

            File.WriteAllText(file, json);
        }

        public T DeserializeSingle<T>(string file)
        {
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                return (T)JsonSerializer.Deserialize(json, typeof(T));
            }
            else
            {
                File.Create(file).Close();
                throw new Exception("Couldn't deserialize data");
            }
        }

        //public void Deserialize()
        //{
        //    for (int i = 0; i < Data.Count; i++)
        //    {
        //        if (File.Exists(Data[i].FileName))
        //        {
        //            var json = File.ReadAllText(Data[i].FileName);
        //            Data[i] = (IData)JsonSerializer.Deserialize(json, Data[i].GetType());
        //        }
        //        else
        //        {
        //            var json = JsonSerializer.Serialize(Data[i], Data[i].GetType(), new JsonSerializerOptions() { WriteIndented = true });
        //            File.Create(Data[i].FileName).Close();
        //            File.WriteAllText(Data[i].FileName, json);
        //        }
        //    }
        //}
    }
}
