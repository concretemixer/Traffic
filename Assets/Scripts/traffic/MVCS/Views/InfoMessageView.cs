using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;
using strange.extensions.mediation.impl;

namespace Traffic.MVCS.Views.UI
{
    public class InfoMessageView : RotatableView
    {
        [SerializeField]
        Button okButton;

        [SerializeField]
        Button closeButton;

        [SerializeField]
        Image baseImage;

        [SerializeField]
        Text text;

        [SerializeField]
        Text caption;

        public readonly Signal onButtonClose = new Signal();
        public readonly Signal onButtonOk = new Signal();

        public void Show(bool value)
        {
            baseImage.gameObject.SetActive(value);
        }

        public void SetCaption(string data)
        {
            caption.text = data;
        }

        public void SetText(string data)
        {
            text.text = data;
        }

        protected override void Awake()
        {
            okButton.onClick.AddListener(onButtonOk.Dispatch);
            closeButton.onClick.AddListener(onButtonClose.Dispatch);
            base.Awake();
        }

       



        protected override void OnDestroy()
        {
               okButton.onClick.RemoveListener(onButtonOk.Dispatch);
            closeButton.onClick.RemoveListener(onButtonClose.Dispatch);

            base.OnDestroy();
        }


        public override void Layout()
        {
            base.Layout();
        }
    }
}