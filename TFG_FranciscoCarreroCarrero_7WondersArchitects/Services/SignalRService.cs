using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects.Services {
    public class SignalRService {
        private readonly HubConnection _connection;
        //evento para escuchar el gameState
        public event Action<string> OnGameStateReceived;

        //evento para avisar a la interfaz cuando llegue un mensaje
        public event Action<string, string> OnMessageReceived;

        //evento para registrar al rival
        public event Action<string, string> OnPlayerJoined;

        public SignalRService() {
            string urlServidor = "http://10.0.2.2:5222/gamehub";

            _connection = new HubConnectionBuilder()
                .WithUrl(urlServidor)
                .Build();

            //configuramos el cliente para escuchar el saludo del servidor
            _connection.On<string, string>("ReceiveMessage", (user, message) => {
                OnMessageReceived?.Invoke(user, message);
            });

            //para recibir el gameState del juego
            _connection.On<string>("ReceiveGameState", (jsonState) => {
                OnGameStateReceived?.Invoke(jsonState);
            });
            
            //evento para registrar al rival
            _connection.On<string, string>("PlayerJoined", (guestName, guestWonder) => {
                OnPlayerJoined?.Invoke(guestName, guestWonder);
            });
        }

        //para enviar gameState como json al rival
        public async Task SendGameStateAsync(string roomCode, string jsonState) {
            await _connection.InvokeAsync("SendGameState", roomCode, jsonState);
        }



        //metodo para iniciar la conexión
        public async Task ConnectAsync() {
            if (!await IsServerAliveAsync()) {
                throw new Exception("El servidor está apagado o no responde, contacte con soporte.");
            }

            if (_connection.State == HubConnectionState.Disconnected) {
                await _connection.StartAsync();
            }
        }




        //metodo para crear una sala (Host) y recibir el código generado
        public async Task<string> CreateRoomAsync(string hostName, string hostWonder) {
            //llama al método "CreateRoom" del servidor y le devuelve el codigo de sala
            return await _connection.InvokeAsync<string>("CreateRoom", hostName, hostWonder);
        }

        //metodo para unirse a la sala
        public async Task<string> JoinRoomAsync(string roomCode, string guestName, string guestWonder) {
            //llama al método "JoinRoom" del servidor y le devuelve si la sala existe o no
            return await _connection.InvokeAsync<string>("JoinRoom", roomCode, guestName, guestWonder);
        }




        //mensaje a todos los jugadores de x sala 
        public async Task SendGreetingAsync(string roomCode) {
            string mensaje = $"Todos los jugadores conectados, ¡a construir!";
            await _connection.InvokeAsync("SendMessageToRoom", roomCode, mensaje);
        }

        //comprobar si el servidor esta levantado
        private async Task<bool> IsServerAliveAsync() {
            try {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3); // 3 segundos para responder

                // petición rápida a ruta raíz
                var response = await client.GetAsync("http://10.0.2.2:5222/");

                return response.IsSuccessStatusCode; //true si esta levantado
            } catch {
                //si esta apagado o tardo mas de 3 segundos false
                return false;
            }
        }


    }
}