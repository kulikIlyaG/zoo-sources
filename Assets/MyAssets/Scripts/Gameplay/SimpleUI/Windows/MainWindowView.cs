using TMPro;
using UnityEngine;

namespace Zoo.Gameplay.SimpleUI
{
    public sealed class MainWindowView : WindowView
    {
        [SerializeField] private TMP_Text _preysCounter;
        [SerializeField] private TMP_Text _predatorsCounter;
        
        public void SetPredatorsCounter(int count)
        {
            _predatorsCounter.text = count.ToString();
        }

        public void SetPreysCounter(int count)
        {
            _preysCounter.text = count.ToString();
        }
    }
}