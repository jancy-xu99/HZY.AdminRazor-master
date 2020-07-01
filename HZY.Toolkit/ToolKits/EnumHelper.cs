


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace HZY.Toolkit.ToolKits
{
    public class EnumHelper
    {
        public static int GetEnumValue(Type enumType, string enumName)
        {
            try
            { 
                if (!enumType.IsEnum)
                    throw new ArgumentException("enumType必须是枚举类型");
                var values = System.Enum.GetValues(enumType);
                var ht = new Hashtable();
                foreach (var val in values)
                {
                    ht.Add(System.Enum.GetName(enumType, val), val);
                }
                return (int)ht[enumName];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable EnumToDataTable(Type enumType, string key, string val)
        {
            string[] Names = System.Enum.GetNames(enumType);

            DataTable table = new DataTable();
            table.Columns.Add(key, System.Type.GetType("System.Int32"));
            table.Columns.Add(val, System.Type.GetType("System.String"));
            table.Columns[key].Unique = true;
            for (int i = 0; i < Names.Length; i++)
            {
                DataRow DR = table.NewRow();
                int keyvalue = GYLib.Base.Utils.EnumUtils.GetValueByName(enumType, Names[i].ToString());
                DR[key] = keyvalue;
                DR[val] = GYLib.Base.Utils.EnumUtils.GetDescriptionByValue(enumType, keyvalue);
                table.Rows.Add(DR);
            }
            return table;
        }

        //public static List<SelectItemModel> EnumToSelectItem(Type enumType, string key, string val)
        //{
        //    string[] Names = System.Enum.GetNames(enumType);
                
        //    List<SelectItemModel> list = new List<SelectItemModel>();
        //    for (int i = 0; i < Names.Length; i++)
        //    {
        //        SelectItemModel liTemp = new SelectItemModel();
        //        int keyvalue = GYLib.Base.Utils.EnumUtils.GetValueByName(enumType, Names[i].ToString());
        //        liTemp.Text = GYLib.Base.Utils.EnumUtils.GetDescriptionByValue(enumType, keyvalue);
        //        liTemp.Value = keyvalue.ToString();
        //        liTemp.Selected = false;
        //        list.Add(liTemp);
        //    }
        //    return list;
        //}
    }
}
