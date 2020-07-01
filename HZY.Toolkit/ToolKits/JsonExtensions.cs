using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit.ToolKits
{
    public class JsonExtensions
    {
       
        public static string Json<T>(object p)
        {
            //T t = GYLib.Base.Utils.JsonUtils.GetResultFromT<T>(p);
            T obj = (T)p;
            return  JsonConvert.SerializeObject(obj);
            
        }

    }
}
