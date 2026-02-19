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

        private void PreparacionCartas() {
            //cartas recurso
            var resourceTypes = Card.GetAllResourceTypes();
            for (int i = 0; i < resourceTypes.Length; i++) {
                for (int j = 0; j < 4; j++) {
                    mazoPrincipal.Add(new Card("MazoPrincipal_"+resourceTypes[i].ToString()+ j, Card.CardType.Resource, resourceTypes[i]));
                }
            }

            //cartas militares
            int aux=0;
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    mazoPrincipal.Add(new Card("MazoPrincipal_Military" + i, Card.CardType.Military, aux));
                    if (i == 2&& j==2) {
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
            //
            //

            var arrayMazoPrincipal = mazoPrincipal.ToArray();
            Random.Shared.Shuffle(arrayMazoPrincipal);//suffle no esta para listas
            mazoPrincipal = arrayMazoPrincipal.ToList();

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            mazoMano.Add(mazoPrincipal[0]);
            mazoPrincipal.RemoveAt(0);
            System.Diagnostics.Debug.WriteLine($"Cartas en mano: {mazoMano.Count}");
            System.Diagnostics.Debug.WriteLine($"Cartas en mazoPrincipal: {mazoPrincipal.Count}\n");
             
        }

    }
}
