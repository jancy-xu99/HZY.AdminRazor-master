using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HZY.EFCore.Repository
{
    public static class HZYEFCoreExtensions
    {
        /// <summary>
        /// 获取 PropertyInfo 集合
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_bindingFlags"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertyInfos(this Type _type, BindingFlags _bindingFlags = (BindingFlags.Instance | BindingFlags.Public))
            => _type.GetProperties(_bindingFlags);

        /// <summary>
        /// 创建 对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            var _Type = typeof(T);
            if (_Type.IsValueType || typeof(T) == typeof(string))
                return default(T);
            return (T)Activator.CreateInstance(_Type);
        }

        /// <summary>
        /// 获取 对象 中 某个属性得 标记
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type _type, string _name)
            where T : Attribute
            => GetPropertyInfo(_type, _name).GetCustomAttribute(typeof(T)) as T;

        /// <summary>
        /// 获取 PropertyInfo 对象
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(this Type _type, string _name) => _type.GetProperty(_name);

        /// <summary>
        /// 获取 模型 有 Key 特性得 属性对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo GetKeyProperty(this Type type)
        {
            //(KeyAttribute)Attribute.GetCustomAttributes(type, true).Where(item => item is KeyAttribute).FirstOrDefault();

            PropertyInfo propertyInfo = null;

            foreach (var item in GetPropertyInfos(type))
            {
                if (item.GetCustomAttribute(typeof(KeyAttribute)) != null)
                {
                    propertyInfo = item;
                    break;
                }
            }

            if (propertyInfo == null) throw new Exception("模型未设置主键特性标记!");

            return propertyInfo;
        }

        /// <summary>
        /// 是否有 KeyAttribute 标记
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool HasKey(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttribute(typeof(KeyAttribute)) != null;

        /// <summary>
        /// 根据实体对象 创建 Expression{Func{T, bool}} 表达式树 例如： Lambda => | ( w=>w.Key==Guid.Empty )
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="PropertyName"></param>
        /// <param name="PropertyValue"></param>
        /// <param name="ExpName"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ToWhere<T>(string PropertyName, object PropertyValue, string ExpName = "w")
        {
            //创建 Where Lambda表达式树
            var type = typeof(T);
            var parameter = Expression.Parameter(type, ExpName);
            var property = type.GetProperty(PropertyName);

            if (PropertyValue == null)
            {
                if (property.PropertyType == typeof(Guid)) PropertyValue = Guid.Empty;

                if (property.PropertyType == typeof(int)) PropertyValue = Int32.MinValue;
            }

            var right = Expression.Constant(PropertyValue);

            try
            {
                if (property.PropertyType == typeof(Guid)) right = Expression.Constant(PropertyValue, typeof(Guid));

                if (property.PropertyType == typeof(int)) right = Expression.Constant(PropertyValue, typeof(int));

                if (property.PropertyType == typeof(Guid?)) right = Expression.Constant(PropertyValue, typeof(Guid?));

                if (property.PropertyType == typeof(int?)) right = Expression.Constant(PropertyValue, typeof(int?));
            }
            catch (Exception ex)
            {
                if (property.PropertyType != PropertyValue.GetType())
                    throw new Exception("请将主键值 转换为 正确的类型值！");
                else
                    throw ex;
            }

            var left = Expression.Property(parameter, property);
            var body = Expression.Equal(left, right);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        /// <summary>
        /// 获取 Expression {Func{T, T}} 树结构 Lambda => | w => new User{ Name="hzy" }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, T>> ToMemberInitByModel<T>(this T Model, string ExpName = "w")
            where T : class, new()
        {
            //创建 Where Lambda表达式树
            var type = typeof(T);
            var parameter = Expression.Parameter(type, ExpName);

            var list = new List<MemberBinding>();

            foreach (var item in GetPropertyInfos(type))
                list.Add(Expression.Bind(item, Expression.Constant(item.GetValue(Model), item.PropertyType)));

            var newExpr = Expression.New(typeof(T));

            return Expression.Lambda<Func<T, T>>(Expression.MemberInit(newExpr, list), parameter);
        }

        /// <summary>
        /// 将 Null 对象转换为 对象的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T ToNewByNull<T>(this T model) 
            where T : class, new()
        {
            if (model != null) return model;
            return CreateInstance<T>();
        }

        #region LINQ 扩展

        public static IQueryable<T> WhereIF<T>(this IQueryable<T> query, Expression<Func<T, bool>> expWhere, bool IF)
        {
            return IF ? query.Where(expWhere) : query;
        }

        #endregion


    }
}