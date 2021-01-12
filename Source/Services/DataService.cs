using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using DSharpPlus;
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
        public IServiceProvider Services { get; set; }

        public DataService(IServiceProvider services)
        {
            this.Services = services;
            DataList = new List<IData>()
            {
                services.GetRequiredService<ConfigData>(),
                services.GetRequiredService<FunData>(),
                services.GetRequiredService<TimerData>()
            };           
        }

        public void SerializeData()
        {
            foreach (var set in DataList)
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(set, set.GetType(), options);

                if (!File.Exists(set.FileName))
                    File.Create(set.FileName).Close();

                File.WriteAllText(set.FileName, json);
            }
        }

        public void DeserializeData()
        {
            foreach (var set in DataList)
            {
                if (!File.Exists(set.FileName))
                {
                    File.Create(set.FileName).Close();
                    var options = new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };

                    var json = JsonSerializer.Serialize(set, set.GetType(), options);
                    File.WriteAllText(set.FileName, json);
                }
                else
                {
                    var json = File.ReadAllText(set.FileName);
                    var data = (IData)JsonSerializer.Deserialize(json, set.GetType());
                    set.LoadData(data);
                }
            }
        }     

        public async Task StartTimers(DiscordClient client)
        {
            Services.GetRequiredService<TimerData>().Start(client);
        }
    }
}
