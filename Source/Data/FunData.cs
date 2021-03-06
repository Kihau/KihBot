﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KihBot.Data
{
    [Serializable]
    public class FunData : IData
    {
        public Dictionary<ulong, List<string>> CustomAnswers { get; set; }

        [JsonIgnore]
        public string FileName { get; init; }
        public FunData() => FileName = "fundata.json";

        public void LoadData(IData data)
        {
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties)
                prop.SetValue(this, prop.GetValue(data));
        }
    }
}
