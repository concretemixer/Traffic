using UnityEngine;
using UnityEngine.UI;
using strange.extensions.signal.impl;

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

        [SerializeField]
        Image messageImage;

        [SerializeField]
        Image rotator;

        public readonly Signal onButtonClose = new Signal();
        public readonly Signal onButtonOk = new Signal();

        void Update()
        {
            rotator.GetComponent<RectTransform>().rotation = Quaternion.EulerAngles(0, 0, Time.realtimeSinceStartup*3.0f);
        }

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

        public void SetMessageMode(bool value)
        {
            messageImage.gameObject.SetActive(value);
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

        public override void Layout(int width, int height)
        {
            base.Layout(width, height);
            this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
}