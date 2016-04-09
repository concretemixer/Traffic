using UnityEngine;
using UnityEngine.UI;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;
using System.Collections.Generic;

namespace Traffic.MVCS.Models
{


    public class LocaleService : ILocaleService 
    {
        [Inject]
        public IAPService iapService { get; set; }

        Dictionary<string, string> entries = new Dictionary<string,string>();

        public LocaleService()
        {
            
            TextAsset locale;
            locale = UnityEngine.Resources.Load<TextAsset>("locale/default");           
            string[] lines = locale.text.Split(new char[] {'\r','\n'});

            foreach (var line in lines) 
            {
                string[] parts = line.Split(new char[] { '|' });
                if (parts.Length >= 2)
                {
                    entries.Add(parts[0],parts[1].Replace("<br>","\n"));
                }
            }             
        }


        bool priceStringsOk = false;
        [PostConstruct]
        public void UpdatePriceStrings()
        {
            if (priceStringsOk)
                return;

            float price = 1000;
            string currency = "?";

#if UNITY_IOS
            entries.Add("%PRICE_NO_ADS%", "$2");
            entries.Add("%PRICE_LEVELS%", "$1.5");
#else
            if (iapService.GetProductPrice(IAPType.NoAdverts, out price, out currency))
            {
                entries.Add("%PRICE_NO_ADS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
                priceStringsOk = true;
            }
            if (iapService.GetProductPrice(IAPType.AdditionalLevels, out price, out currency))
            {
                entries.Add("%PRICE_LEVELS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
                priceStringsOk = true;
            }
#endif
        }

        public void SetAllTexts(GameObject root)         
        {
            UpdatePriceStrings();
            foreach (Text textField in root.GetComponentsInChildren<Text>())
            {
                textField.text = ProcessString(textField.text);
            }
        }

        public string ProcessString(string template)
        {
            string text = template;

            for (int a = 0; a < 3; a++)
            {
                bool changed = false;
                foreach (var entry in entries)
                {
                    if (text.Contains(entry.Key))
                    {
                        text = text.Replace(entry.Key, entry.Value);
                        changed = true;
                    }
                }

                if (!changed)
                    break;
            }

            return text;
        }

    }
}
