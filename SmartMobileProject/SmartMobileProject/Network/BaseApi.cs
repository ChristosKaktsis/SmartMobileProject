﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Network
{
    public class BaseApi
    {
        protected static readonly string BaseAddress = "http://192.168.3.41:5260";
        protected static readonly string Url = $"{BaseAddress}/api";
        private static HttpClient client;

        protected static async Task<HttpClient> GetClient()
        {
            if (client != null)
                return client;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }
    }
}
