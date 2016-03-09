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
            entries.Add("%START%", "START!");
            entries.Add("%MUSIC%", "MUSIC");
            entries.Add("%SOUND%", "SOUND");
            entries.Add("%PROMO_TITLE%", "PROMOTION");
            entries.Add("%ENTER_CODE%", "ENTER CODE");
            entries.Add("%SETTINGS%", "SETTINGS");
            entries.Add("%SHOP%", "SHOP");
            entries.Add("%BUY%", "BUY");

            entries.Add("%LEVELS_DESC_SHOP%", "12 ADDITIONAL LEVELS FOR %PRICE_NO_ADS%");
            entries.Add("%NO_ADS_DESC_SHOP%", "TURN OFF ADVERTISMENTS FOR %PRICE_LEVELS%");

            entries.Add("%PURCHASE_OK%", "PURCHASE_OK");
            entries.Add("%PURCHASE_FAILED%", "PURCHASE FAILED");

            entries.Add("%LEVELS_BOUGHT%", "You have purchased 12 additional levels for  %PRICE_LEVELS%");
            entries.Add("%NO_ADS_BOUGHT%", "You have purchased the permanent advert removal for %PRICE_NO_ADS%");

            entries.Add("%LEVELS_LOCKED_TITLE%", "ATTENTION");
            entries.Add("%LEVELS_LOCKED_TEXT%", "YOU CAN BUY ADDITIONAL 12 LEVELS FOR %PRICE_LEVELS%");
            entries.Add("%NO_TRIES_TITLE%", "NO MORE TRIES LEFT");
            entries.Add("%NO_TRIES_DESC%", "BUY UNLIMITED TRIES FOR %PRICE_NO_ADS% OR WATCH AN AVDERTISMENT TO REFRESH !");
            entries.Add("%NO_TRIES_TIMER%", "TRIES WILL REFRESH AUTOMATICALY IN {0}:{1}");

            entries.Add("%NO_TRIES_ADVERT%", "VIEW\nADVERT");

            entries.Add("%NEW_TRIES_TITLE%", "CONGRATULATIONS");
            entries.Add("%NEW_TRIES_DESC%", "YOU TRIES HAS BEEN RESTORED!");


        }

        [PostConstruct]
        public void UpdatePriceStrings()
        {
            float price = 1000;
            string currency = "?";

            if (iapService.GetProductPrice(IAPType.NoAdverts, out price, out currency))
                entries.Add("%PRICE_NO_ADS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
            if (iapService.GetProductPrice(IAPType.AdditionalLevels, out price, out currency))
                entries.Add("%PRICE_LEVELS%", currency + (currency.Length > 1 ? " " : "") + price.ToString("F2"));
        }

        public void SetAllTexts(GameObject root)         
        {
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