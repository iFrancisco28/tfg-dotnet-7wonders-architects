using CommunityToolkit.Maui.Extensions;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TFG_FranciscoCarreroCarrero_7WondersArchitects
{
    public partial class MainPage : ContentPage
    {
        List<Card> mazoPrincipal = new List<Card>();
        List<Card> mazoMano= new List<Card>();

        public MainPage()
        {
            InitializeComponent();
            PreparacionCartas();
        }

        private int etapaConstruccion = 0;

        //lo hago aqui de momento, pero luego se hara en la capa de dominio
        private void PreparacionCartas() {
            //luego todo esto intentare hacerlo un metodo generico, recibiendo parametros de cantidad de cartas, tipo, etc

            //cartas recurso
            var resourceTypes = Card.GetAllResourceTypes();
            for (int i = 0; i < resourceTypes.Length; i++) {
                int cartasPorHacer = 4;
                if (resourceTypes[i] == Card.ResourceType.Gold) {
                    cartasPorHacer = 6;
                }
                for (int j = 0; j < cartasPorHacer; j++) {
                    mazoPrincipal.Add(new Card("MazoPrincipal_" + resourceTypes[i].ToString() + j, Card.CardType.Resource, resourceTypes[i]));
                }
            }

            //cartas militares
            int aux = 0;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    if (i == 2 && j == 2) {
                        break;
                    }
                    mazoPrincipal.Add(new Card("MazoPrincipal_Military" + i, Card.CardType.Military, aux));
                }
                aux++;
            }

            //cartas ciencia
            var scienceTypes = Card.GetAllScienceTypes();
            for (int i = 0; i < scienceTypes.Length; i++) {
                for (int j = 0; j < 4; j++) {
                    mazoPrincipal.Add(new Card("MazoPrincipal_" + scienceTypes[i].ToString() + j, Card.CardType.Science, scienceTypes[i]));
                }
            }

            //cartas victory point
            for (int i = 0; i < 12; i++) {
                if (i < 8){ 
                    mazoPrincipal.Add(new Card("MazoPrincipal_VictoryPoint" + i, Card.CardType.VictoryPoint, 2, true));
                } else {
                    mazoPrincipal.Add(new Card("MazoPrincipal_VictoryPoint" + i, Card.CardType.VictoryPoint, 3, false));
                }
            }

            var arrayMazoPrincipal = mazoPrincipal.ToArray();
            Random.Shared.Shuffle(arrayMazoPrincipal);//suffle no esta para listas
            mazoPrincipal = arrayMazoPrincipal.ToList();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            mazoMano.Add(mazoPrincipal[0]);

            DisplayAlert("Has robado", mazoPrincipal[0].ToString(), "OK");

            mazoPrincipal.RemoveAt(0);
            ComprobarConstruccion();

        }

        private void ComprobarConstruccion() {
            if (etapaConstruccion > 4) return;

            //numero cartas por recurso
            int maderas = mazoMano.Count(c => c.Resource == Card.ResourceType.Wood);
            int piedras = mazoMano.Count(c => c.Resource == Card.ResourceType.Stone);
            int arcillas = mazoMano.Count(c => c.Resource == Card.ResourceType.Clay);
            int papiros = mazoMano.Count(c => c.Resource == Card.ResourceType.Papyrus);
            int cristales = mazoMano.Count(c => c.Resource == Card.ResourceType.Bottle);
            int oros = mazoMano.Count(c => c.Resource == Card.ResourceType.Gold);

            int[] recursos = { maderas, piedras, arcillas, papiros, cristales };

            int diferentes = recursos.Count(c => c > 0); 
            int maxIguales = recursos.Max(); 

            bool puedeConstruir = false;
            
            //como se en que etapa estoy no salto
            switch (etapaConstruccion) {
                case 0: // 2 diferentes
                    puedeConstruir = (diferentes + oros) >= 2;
                    break;
                case 1: // 2 iguales
                    puedeConstruir = (maxIguales + oros) >= 2;
                    break;
                case 2: // 3 diferentes
                    puedeConstruir = (diferentes + oros) >= 3;
                    break;
                case 3: // 3 iguales
                    puedeConstruir = (maxIguales + oros) >= 3;
                    break;
                case 4: // 4 diferentes
                    puedeConstruir = (diferentes + oros) >= 4;
                    break;
            }

            //cambiar imagen (construir para el usuario)
            if (puedeConstruir) {
                switch (etapaConstruccion) {
                    case 0:
                        Parte1Reliquia.Source = "guiza/maravilla/guiza0b.png";
                        break;
                    case 1:
                        Parte2Reliquia.Source = "guiza/maravilla/guiza1b.png";
                        break;
                    case 2:
                        Parte3Reliquia.Source = "guiza/maravilla/guiza2b.png";
                        break;
                    case 3:
                        Parte4Reliquia.Source = "guiza/maravilla/guiza3b.png";
                        break;
                    case 4:
                        Parte5Reliquia.Source = "guiza/maravilla/guiza4b.png";
                        break;
                }

                DisplayAlert("Nueva etapa!", $"Has completado la parte {etapaConstruccion + 1} de Guiza", "Ok");

                etapaConstruccion++;

                // falta gastar las cartas usadas
            }
        }

        private void ButtonShowPlayerDeck(object sender, EventArgs e) {
            var popup = new PlayerDeckPopup();
            this.ShowPopup(popup);
        }

        //metodo super detallado hecho con la ia, para ver todo el contenido del mazo principal
        private async void ButtonMostrar_Clicked(object sender, EventArgs e) {
            // Total
            int total = mazoPrincipal.Count;

            // --- RECURSOS ---
            int maderas = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Wood);
            int piedras = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Stone);
            int arcillas = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Clay);
            int papiros = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Papyrus);
            int cristales = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Bottle);
            int oros = mazoPrincipal.Count(c => c.Type == Card.CardType.Resource && c.Resource == Card.ResourceType.Gold);

            // --- MILITARES ---
            int mil0 = mazoPrincipal.Count(c => c.Type == Card.CardType.Military && c.Horns == 0);
            int mil1 = mazoPrincipal.Count(c => c.Type == Card.CardType.Military && c.Horns == 1);
            int mil2 = mazoPrincipal.Count(c => c.Type == Card.CardType.Military && c.Horns == 2);

            // --- CIENCIA ---
            int compass = mazoPrincipal.Count(c => c.Type == Card.CardType.Science && c.Science == Card.ScienceType.Compass);
            int tablet = mazoPrincipal.Count(c => c.Type == Card.CardType.Science && c.Science == Card.ScienceType.Tablet);
            int gear = mazoPrincipal.Count(c => c.Type == Card.CardType.Science && c.Science == Card.ScienceType.Gear);

            // --- PUNTOS DE VICTORIA ---
            // Como las de 2VP siempre tienen gato y las de 3VP no, basta con comprobar los puntos
            int vp2 = mazoPrincipal.Count(c => c.Type == Card.CardType.VictoryPoint && c.VictoryPoints == 2);
            int vp3 = mazoPrincipal.Count(c => c.Type == Card.CardType.VictoryPoint && c.VictoryPoints == 3);

            // Montamos el texto final bien formateado para la alerta
            string mensaje = $"TOTAL CARTAS BARAJA COMUN: {total} (solo debug)\n\n" +
                             $"🪵 RECURSOS:\n" +
                             $" - Madera: {maderas} | Piedra: {piedras} | Arcilla: {arcillas}\n" +
                             $" - Papiro: {papiros} | Cristal: {cristales} | Oro: {oros}\n\n" +
                             $"⚔️ MILITARES:\n" +
                             $" - 0 Cuernos: {mil0}\n" +
                             $" - 1 Cuerno: {mil1}\n" +
                             $" - 2 Cuernos: {mil2}\n\n" +
                             $"🔬 CIENCIA:\n" +
                             $" - Compás: {compass} | Tablilla: {tablet} | Engranaje: {gear}\n\n" +
                             $"🏛️ PUNTOS DE VICTORIA:\n" +
                             $" - 2 Puntos (+Gato): {vp2}\n" +
                             $" - 3 Puntos: {vp3}";

            // Mostramos el popup (recuerda que el método debe tener 'async')
            await DisplayAlert("Inventario del Mazo Principal", mensaje, "Cerrar");
        }



    }
}
