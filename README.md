# 7 Wonders Architects - Multiplatform Client (.NET MAUI)

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![SignalR Client](https://img.shields.io/badge/SignalR_Client-00599C?style=for-the-badge&logo=microsoft&logoColor=white)

Cliente multiplataforma desarrollado en .NET 9 MAUI que interactúa con un servidor centralizado para emular el juego de mesa *7 Wonders Architects*. El proyecto no es solo una interfaz gráfica, sino un motor de reglas de negocio completo que automatiza la lógica del juego físico, estructurado bajo una arquitectura de software limpia y altamente desacoplada.

## Arquitectura del Software (Clean Architecture)

El cliente ha sido estructurado separando radicalmente el estado, la lógica y la representación visual para garantizar su escalabilidad y facilitar la serialización del estado en red.

### 1. Capa de Dominio (`Domain.Entities`)
Compuesta exclusivamente por clases POCO (*Plain Old CLR Objects*) puras y sin dependencias de la interfaz de usuario. 
* **Modelo Dinámico de Cartas (`Card.cs`):** Define los tipos a través de enumerados (`Resource`, `Science`, `Military`, `VictoryPoint`). Su estructura permite que una misma clase maneje cuernos de guerra, símbolos científicos o la presencia de la ficha del "Gato" mediante propiedades tipadas.
* **Estructuras de Juego (`Wonder.cs` & `Player.cs`):** Cada maravilla encapsula su matriz de puntos de victoria por etapa (`PointsPerStage`). La entidad jugador gestiona independientemente su mazo de mano (`HandDeck`), su mazo de construcción (`WonderDeck`) y sus escudos militares.
* **Estado Global (`GameState.cs`):** Actúa como la fuente única de verdad (Single Source of Truth), encapsulando los mazos centrales, los jugadores y el contador global de avisos de guerra.

### 2. Capa de Negocio (`Manager.GameManager`)
El núcleo duro del cliente. Aísla toda la algoritmia y reglas de negocio del framework de MAUI. 
* **Algoritmo de Construcción (`ComprobarConstruccion`):** Analiza el `HandDeck` utilizando `LINQ` para clasificar recursos. Implementa un algoritmo dinámico que evalúa la etapa actual del jugador (0 a 4) y determina si posee los requisitos (recursos idénticos o distintos), **calculando dinámicamente las cartas de oro como comodines universales** y ejecutando eliminaciones en cascada.
* **Habilidades Asimétricas (`EvaluarHabilidadesMaravilla`):** Ejecuta lógica condicional inyectando ventajas específicas tras la construcción de etapas, como el robo adicional del mazo central (Éfeso), robo de los mazos de maravilla (Olimpia) o la inyección directa de escudos en la mano del jugador (Rodas).
* **Resolución Militar (`ResolverGuerra`):** Motor de conflicto que se dispara automáticamente al alcanzar los 3 avisos de guerra. Compara los inventarios de ambos jugadores y aplica multiplicadores condicionales (ej. una victoria por aplastamiento —duplicar los escudos del rival— otorga +6 Fichas de Victoria en lugar de las +3 habituales).
* **Sincronización P2P e Inversión de Estado:** Expone métodos para serializar/deserializar el estado (`System.Text.Json`). Al recibir un estado de red, ejecuta una **inversión de referencias en memoria** (`var temp = newState.LocalPlayer; newState.LocalPlayer = newState.RemotePlayer;`) para que la perspectiva de la interfaz asuma siempre al jugador actual como `LocalPlayer`, evitando duplicar lógica en el front-end.

### 3. Capa de Presentación e Infraestructura
* **Conexión P2P (`SignalRService.cs`):** Inyectado como Singleton, mantiene una conexión persistente por WebSockets. Expone eventos puros (`Action<string>`) para que la UI reaccione pasivamente a los mensajes del Hub de Azure.
* **UI Asíncrona y Thread-Safety:** La manipulación visual y el repintado de las cartas/maravillas en el tablero se ejecuta estrictamente dentro de delegados `MainThread.BeginInvokeOnMainThread()`. Esto evita colisiones de concurrencia y excepciones de interbloqueo cuando los datos llegan asíncronamente desde la red.
* **Popups Modales No Bloqueantes:** Uso de `CommunityToolkit.Maui.Views` para gestionar ventanas de selección (Login), notificaciones y el inventario detallado del jugador (`PlayerDeckPopup`) sin interrumpir el ciclo de vida de la página principal.

## Despliegue Local

Para compilar y ejecutar el cliente:

1.  Asegúrate de tener **Visual Studio 2022** con la carga de trabajo de **Desarrollo de .NET MAUI** instalada.
2.  Clona el repositorio y abre la solución.
3.  Por defecto, el servicio de red (`SignalRService.cs`) apunta al endpoint alojado en Azure (`_baseUrl`). Para pruebas de desarrollo backend, modifica esta ruta apuntando a `https://localhost:<puerto>`.
4.  Compila y ejecuta en Windows Machine o emulador Android.

---

## Aviso de Propiedad Intelectual y Uso Educativo

Este proyecto utiliza material gráfico, nombres y mecánicas basadas en el juego de mesa *7 Wonders Architects*, cuya propiedad intelectual pertenece a **Repos Production** y **Asmodee**. 

No se reclama ningún derecho sobre el material gráfico, diseño o marca registrada. Este proyecto es **exclusivamente académico y educativo**, no tiene ningún fin comercial y se publica públicamente de buena fe y únicamente como portafolio para demostrar habilidades de ingeniería de software, arquitectura en capas, algoritmia y manejo de concurrencia.

## Licencia
El código fuente de este proyecto (excluyendo los assets gráficos y las mecánicas mencionadas en el aviso superior) está bajo la Licencia MIT.
