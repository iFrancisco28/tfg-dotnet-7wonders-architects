# 7 Wonders Architects - Multiplatform Client (.NET MAUI)

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SignalR Client](https://img.shields.io/badge/SignalR_Client-00599C?style=for-the-badge&logo=microsoft&logoColor=white)

Cliente multiplataforma desarrollado para interactuar con el servidor multijugador del juego *7 Wonders Architects*. Este proyecto automatiza el motor de reglas de mesa, validaciones de construcción y resolución de guerras, priorizando un código limpio, modular y mantenible.

## Arquitectura del Software

El proyecto se estructura siguiendo una **Arquitectura en 3 Capas** fuertemente desacoplada, inspirada en principios de *Domain-Driven Design (DDD)*:

1. **Capa de Dominio (`Entities`):** * Compuesta por clases POCO puras (Plain Old CLR Objects) agnósticas al framework visual (`Player`, `Card`, `Wonder`, `GameState`). 
2. **Capa de Negocio (`Manager`):** * Centralizada en el `GameManager`. Actúa como el cerebro del juego. Evalúa dinámicamente combinaciones de recursos (distintos, idénticos y comodines de oro), valida el progreso de las maravillas y resuelve los algoritmos de guerra (calculando multiplicadores por aplastamiento).
3. **Capa de Presentación (`Views`):** * Compuesta por XAML y Code-Behind. Reacciona de forma pasiva a los cambios de estado dictados por el Manager.

## Características Técnicas Destacadas

* **Inversión Dinámica de Estado:** Al recibir el `GameState` en JSON desde el servidor, el motor machaca el estado local e invierte matemáticamente los roles (Local / Remote) para que la perspectiva del tablero siempre sea la correcta para quien tiene el turno.
* **Sincronización y UI Asíncrona:** Integración de `CommunityToolkit.Maui` para modales no bloqueantes. Para evitar excepciones de concurrencia y bloqueos de red, la UI se sincroniza y repinta desde el hilo principal utilizando delegados (`MainThread.BeginInvokeOnMainThread`).
* **Sincronización de Red P2P:** Inyección de dependencias del servicio de SignalR para mantener la conexión por WebSockets de forma global durante el ciclo de vida de la aplicación.

## Capturas de Pantalla
<img width="250" alt="7WondersArhictects_paginaInicio" src="https://github.com/user-attachments/assets/6f667b05-20c2-4fe7-acd4-f27da0ea086f" /> <img width="250" alt="7WondersArhictects_tablero_turnoRival" src="https://github.com/user-attachments/assets/d3551a23-b4cd-4ac3-8c82-4e64eb8a71f0" /> <img width="250" alt="7WondersArhictects_tablero_turnoYo" src="https://github.com/user-attachments/assets/3036b189-9d37-460a-a7d6-60c9402e5742" /> <img width="250" alt="7WondersArhictects_inventario" src="https://github.com/user-attachments/assets/cd67e0b9-c987-4aff-8223-27d2ecc5bfd9" />


## Despliegue Local

Para compilar y ejecutar el cliente:

1. Asegúrate de tener **Visual Studio 2022** con la carga de trabajo de **Desarrollo de .NET MAUI** instalada.
2. Clona el repositorio y abre la solución.
3. Restaura los paquetes NuGet.
4. Por defecto, el cliente (`SignalRService.cs`) apunta al servidor alojado en Azure. Si deseas probarlo en local, cambia la variable `_baseUrl` a `https://localhost:<puerto>`.
5. Compila y ejecuta en el emulador de Android o en Windows Machine.

---

## Aviso de Propiedad Intelectual y Uso Educativo

Este proyecto utiliza material gráfico, nombres y mecánicas basadas en el juego de mesa *7 Wonders Architects*, cuya propiedad intelectual pertenece a **Repos Production** y **Asmodee**. 

No se reclama ningún derecho sobre el material gráfico, diseño o marca registrada. Este proyecto es **exclusivamente académico y educativo**, no tiene ningún fin comercial y se publica públicamente de buena fe y únicamente como portafolio para demostrar habilidades de desarrollo de software, arquitectura limpia y programación orientada a objetos.

## Licencia
El código fuente de este proyecto (excluyendo los assets gráficos y las mecánicas mencionadas en el aviso superior) está bajo la Licencia MIT.
