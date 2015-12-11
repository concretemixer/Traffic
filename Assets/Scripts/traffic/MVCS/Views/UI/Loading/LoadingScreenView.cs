using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Traffic.MVCS.Views.UI.Loading
{
    public class LoadingScreenView : View
    {
        [SerializeField]
        Text statusLabel;

        public void SetStatus(string text)
        {
            statusLabel.text = text;
        }
    }
}