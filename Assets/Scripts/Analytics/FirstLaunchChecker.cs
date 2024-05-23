using Creatures.Model.Data;
using General.Components.Dialogs;
using UnityEngine;

namespace PirateIsland.Analytics
{
    public class FirstLaunchChecker : MonoBehaviour
    {
        [SerializeField] private ShowDialogComponent _dialogComponent;


        private void Start()
        {
            if (GameSettings.I.FirstLaunch.Value != 1)
            {
                _dialogComponent.Show();
            }

            GameSettings.I.FirstLaunch.Value = 1;
        }
    }
}
