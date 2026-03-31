using UnityEngine;

namespace Multi2D
{
    public class PlayerControllDefinitionScreenTest : MonoBehaviour
    {   
        [SerializeField] private int maxPlayers;
        [SerializeField] private int maxInputs;
        [SerializeField] private PlayerControllDefinitionScreenView view;
        private PlayerControllDefinitionScreen presenter;     

        private void Start()
        {
            view.Initialize();
            presenter = new PlayerControllDefinitionScreen(view, maxPlayers, maxInputs);
            presenter.Init();
        }

        private void OnDestroy()
        {
            presenter.Dispose();
        }
    }
}
