using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarRequestService.Models;

namespace CarRequestService.Services
{
    public class RealCarService : Service
    {
        private ConnectionSettings connectionSettings = ConnectionSettings.getInstance();

        public RealCarService(string link) : base(link) { }

        public async Task<AccuListWithTime> GetAccuState()
        {
            var httpResponseMessage = await Get($"accus");
            if (httpResponseMessage == null || httpResponseMessage.Content == null)
                return null;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            try
            {
                var a = JsonConvert.DeserializeObject<AccuListWithTime>(response);
                return a;
            }
            catch
            {
                return null;
            }
        }

        public async Task<AccuListWithTime> GetAccuState(int id)
        {
            var httpResponseMessage = await Get($"accus/{id}");
            if (httpResponseMessage == null || httpResponseMessage.Content == null)
                return null;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            try
            {
                var a = JsonConvert.DeserializeObject<AccuListWithTime>(response);
                return a;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> PutAccuInit(int id)
        {
            var httpResponseMessage = await PutForm($"init/{id}", new Dictionary<string, string>());
            if (httpResponseMessage == null || httpResponseMessage.Content == null)
                return -1;

            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            return 0;
        }
    }
}
