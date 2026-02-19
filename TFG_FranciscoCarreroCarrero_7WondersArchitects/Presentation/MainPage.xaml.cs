using TFG_FranciscoCarreroCarrero_7WondersArchitects.Domain.Entities;

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
                    mazoPrincipal.Add(new Card("MazoPrincipal_Military" + i, Card.CardType.Military, aux));
                    if (i == 2 && j == 2) {
                        break;
                    }
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

        }

        private void ButtonMostrar_Clicked(object sender, EventArgs e) {

        }



    }
}
