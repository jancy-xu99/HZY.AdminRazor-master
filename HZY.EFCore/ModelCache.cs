using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.EFCore
{
    using HZY.EFCore.Base;
    using HZY.EFCore.Repository;

    public static class ModelCache
    {
        private static Dictionary<string, List<ModelInfo>> Container { get; set; }

        static ModelCache()
        {
            if (Container == null) Container = new Dictionary<string, List<ModelInfo>>();
        }

        public static Dictionary<string, List<ModelInfo>> All() => Container;

        public static List<ModelInfo> GetModelInfos(string TableName) => Container[TableName];

        public static void Set(Type t)
        {
            var list = new List<ModelInfo>();

            var propertyInfos = t.GetProperties();

            foreach (var item in propertyInfos)
            {
                var modelInfo = new ModelInfo();
                var Remark = Toolkit.ReadXmlSummary.XMLForMember(item)?.InnerText?.Trim()?.Split("=>")?[0];
                modelInfo.Name = item.Name;
                modelInfo.Remark = Remark;
                modelInfo.IsKey = HZYEFCoreExtensions.HasKey(item);
                list.Add(modelInfo);
            }

            Container[t.Name] = list;
        }

        public static void Set(List<Type> types)
        {
            foreach (var item in types) Set(item);
        }


    }
}
