using CommunityToolkit.Maui.Extensions;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;
using TFG_FranciscoCarreroCarrero_7WondersArchitects.Presentation;
//using Windows.Networking.Connectivity;
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
        //lo hago aqui de momento, pero luego se hara en la capa de dominio
        private int etapaConstruccion = 0;
        
        private void PreparacionCartas() {
            //luego todo esto intentare hacerlo un metodo generico, recibiendo parametros de cantidad de cartas, tipo, etc

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
            int cristales = mazoMano.Count(c => c.Resource == Card.ResourceType.Glass);
            int oros = mazoMano.Count(c => c.Resource == Card.ResourceType.Gold);

            int[] recursos = { maderas, piedras, arcillas, papiros, cristales };

            int diferentes = recursos.Count(c => c > 0);

            //para pasarle a borrar
            int maxIguales = maderas;
            Card.ResourceType recursoMayor = Card.ResourceType.Wood;
            
            if (piedras > maxIguales) { maxIguales = piedras; recursoMayor = Card.ResourceType.Stone; }
            if (arcillas > maxIguales) { maxIguales = arcillas; recursoMayor = Card.ResourceType.Clay; }
            if (papiros > maxIguales) { maxIguales = papiros; recursoMayor = Card.ResourceType.Papyrus; }
            if (cristales > maxIguales) { maxIguales = cristales; recursoMayor = Card.ResourceType.Glass; }

            bool puedeConstruir = false;
            
            //construyo segun etapaConstruccion
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
                        borrarCartas(2, true, recursoMayor);
                        break;
                    case 1:
                        Parte2Reliquia.Source = "guiza/maravilla/guiza1b.png";
                        borrarCartas(2, false, recursoMayor);
                        break;
                    case 2:
                        Parte3Reliquia.Source = "guiza/maravilla/guiza2b.png";
                        borrarCartas(3, true, recursoMayor);
                        break;
                    case 3:
                        Parte4Reliquia.Source = "guiza/maravilla/guiza3b.png";
                        borrarCartas(3, false, recursoMayor);
                        break;
                    case 4:
                        Parte5Reliquia.Source = "guiza/maravilla/guiza4b.png";
                        borrarCartas(4, true, recursoMayor);
                        break;
                }

                DisplayAlert("Nueva etapa!", $"Has completado la parte {etapaConstruccion + 1} de Guiza", "Ok");

                etapaConstruccion++;
            }
        }

        public void borrarCartas(int cantidad, bool sonDiferentes, Card.ResourceType recursoMayor) {
            int borradas = 0;
                     
            if (sonDiferentes) {
                //si diferentes se va borrando de cada recurso en cascada, excepto si no tiene, que devuelve false y sigue para abajo
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Wood)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Stone)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Clay)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Papyrus)) borradas++;
                if (borradas < cantidad && BorrarUna(Card.ResourceType.Glass)) borradas++;
            } else {
                //si iguales se pasa "recursoMayor" "cantidad" veces
                while (borradas < cantidad && BorrarUna(recursoMayor)) {
                    borradas++;
                }
            }

            // ORO: Lo que falte, lo rellenamos con oro
            while (borradas < cantidad) {
                BorrarUna(Card.ResourceType.Gold);
                borradas++;
            }
        }

        //metodo que devuelve true o false segun borrado
        private bool BorrarUna(Card.ResourceType tipo) {
            //cojo la carta de tipo recurso y de recursoTipo args
            var carta = mazoMano.FirstOrDefault(c => c.Type == Card.CardType.Resource && c.Resource == tipo);
            return carta != null && mazoMano.Remove(carta);
        }

        //sacar popup de mazoPropio
        private void ButtonShowPlayerDeck(object sender, EventArgs e) {
            var popup = new PlayerDeckPopup(mazoMano);
            this.ShowPopup(popup);
        }


    }
}
