using UnityEngine;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;

namespace Traffic.MVCS.Models
{

    public interface ILocaleService 
    {
        void SetAllTexts(GameObject root);
        string ProcessString(string template);

        bool CanChange();
        SystemLanguage GetCurrentLanguage();
        void SetCurrentLanguage(SystemLanguage lang);
        
    }
}