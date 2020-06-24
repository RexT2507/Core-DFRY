using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace ApiLibrary.Core.Utils
{
    class TreatParams
    {
        public static Boolean CanConvert(String value, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            return converter.IsValid(value);
        }

        public static List<String> ParamDataToArray(String paramData, String delimiteur, Regex exeptionArea)
        {
            String paramDataTempo = paramData;
            List<String> tabParam = new List<String>();
            int i = 0;
            while(i < 1)
            {
               
                Match match = exeptionArea.Match(paramDataTempo);
                if (match.Success)
                {
                    i = 0;
                    tabParam.Add(match.Value);
                    paramDataTempo = paramDataTempo.Substring(0, match.Index) + paramDataTempo.Substring(match.Index + match.Value.Length, paramDataTempo.Length - match.Index - match.Value.Length);
                }else
                {
                    i = 1;
                }
            }

            string[] results = paramDataTempo.Split(delimiteur, StringSplitOptions.RemoveEmptyEntries);
            foreach (string result in results)
            {
                tabParam.Add(result);
            }

            return tabParam;
        }
    }
}
