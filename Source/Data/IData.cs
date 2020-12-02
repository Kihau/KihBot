using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    public interface IData
    {
        [JsonIgnore]
        string FileName { get; init; }

        // Same method used in all classes implementing IData
        public void LoadData(IData data)
        {
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
                prop.SetValue(this, prop.GetValue(data));
        }
    }
}
