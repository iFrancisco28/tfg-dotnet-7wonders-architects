using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Services {
    public class SignalRService {
        private readonly HubConnection _connection;

        public SignalRService() {
            string urlServidor = "http://10.0.2.2:5222/gamehub";

            _connection = new HubConnectionBuilder()
                .WithUrl(urlServidor)
                .Build();
        }

    }
}
