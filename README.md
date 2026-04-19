# 7Wonders-Architects-MAUI-Client

Descripción:
Cliente multiplataforma desarrollado para la interacción con el servidor multijugador del juego. El proyecto automatiza el motor de reglas priorizando un código limpio, modular y mantenible.

Stack Técnico: .NET 9 MAUI | C# | SignalR Client | JSON

Arquitectura y Soluciones Implementadas:

Arquitectura en 3 Capas: Separación estricta de responsabilidades. La capa de Dominio (Entities) agrupa modelos POCO agnósticos al framework; la capa de Negocio (GameManager) procesa la lógica principal; y la Presentación reacciona a los cambios de estado.

Sincronización y Renderizado Asíncrono: El cliente procesa el estado (JSON) recibido del servidor, realizando una inversión de roles dinámica (Local/Remote) y repintando la interfaz gráfica sin bloquear el hilo principal mediante MainThread.BeginInvokeOnMainThread.

Motor de Reglas Desacoplado: Desarrollo de algoritmos específicos para la evaluación de requisitos de construcción (evaluando dinámicamente recursos idénticos, distintos o comodines de oro) y resolución automática de conflictos militares.
