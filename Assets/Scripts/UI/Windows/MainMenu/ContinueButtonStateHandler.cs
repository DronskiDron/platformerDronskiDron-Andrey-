using General.Components.LevelManagement;
using UI.Widgets;
using UnityEngine;

namespace UI.Windows.MainMenu
{
    public class ContinueButtonStateHandler : MonoBehaviour
    {
        [SerializeField] private CustomButton _continueButton;


        private void Start()
        {
            CheckContinue();
        }


        private void CheckContinue()
        {
            var jsonLoader = FindObjectOfType<SaveLoadManager>();

            if (jsonLoader.ExistsSaveCheck())
                _continueButton.interactable = true;
            else
                _continueButton.interactable = false;

        }
    }
}
