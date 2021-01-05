using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace GBFDesktopTools.Model.abstractModel
{
    /// <summary>
    /// Model : INotifyPropertyChanged, IEditableObject, IDataErrorInfo
    /// </summary>
    public abstract class abstractModel : INotifyPropertyChanged, IEditableObject, ICloneable
    {
        bool _IsChecked = false;
        bool _IsEnabled = false;
        string _BaseRemark = "";

        /// <summary>
        /// 可选
        /// </summary>
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                this.RaisePropertyChanged(x => x.IsChecked);
            }
        }

        /// <summary>
        /// 可用
        /// </summary>
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                _IsEnabled = value;
                this.RaisePropertyChanged(x => x.IsEnabled);
            }
        }

        /// <summary>
        /// 基础备注
        /// </summary>
        public string BaseRemark
        {
            get { return _BaseRemark; }
            set
            {
                _BaseRemark = value;
                this.RaisePropertyChanged(x => x.BaseRemark);
            }
        }

        #region IEditableObject
        /// <summary>
        /// 开始编辑
        /// </summary>
        public void BeginEdit()
        {
            if (!inTX)
            {
                Backup();
                inTX = true;
            }
        }
        /// <summary>
        /// 取消编辑
        /// </summary>
        public void CancelEdit()
        {
            if (inTX)
            {
                Restore();
                inTX = false;
            }
        }
        /// <summary>
        /// 结束编辑
        /// </summary>
        public void EndEdit()
        {
            if (inTX)
            {
                if (Map != null)
                {
                    Map.Clear();
                }
                inTX = false;
            }
        }
        bool inTX = false;
        System.Collections.Generic.Dictionary<string, object> Map;
        void Backup()
        {
            if (Map == null) Map = new Dictionary<string, object>();
            System.Type type = this.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    if (Map.ContainsKey(property.Name)) Map[property.Name] = property.GetValue(this, null);
                    else Map.Add(property.Name, property.GetValue(this, null));
                }
            }
        }
        void Restore()
        {
            System.Type type = this.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    if (Map.ContainsKey(property.Name))
                    {
                        property.SetValue(this, Map[property.Name], null);
                    }
                }
                RaisePropertyChanged(property.Name);
            }
        }
        #endregion

        #region INotifyPropertyChanged
        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// INotifyPropertyChanged 通知属性改变
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// 使用NewModel 赋值本实体属性
        /// </summary>
        /// <param name="NewModel"></param>
        public void Update(abstractModel NewModel = null)
        {
            this.Update(NewModel, x => new { });
        }

        /// <summary>
        /// 创建当前对象的浅表副本
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    /// <summary>
    /// Model扩展方法
    /// </summary>
    public static class PropertyChangedBaseEx
    {
        /// <summary>
        /// RaisePropertyChanged(string propertyName) 扩展方法
        /// this.RaisePropertyChanged(x => x.属性); 关键字 this 不能省略
        /// </summary>
        public static void RaisePropertyChanged<T, TProperty>(this T propertyChangedBase, System.Linq.Expressions.Expression<Func<T, TProperty>> expression) where T : abstractModel
        {
            if (expression == null) return;

            if (expression.Body is System.Linq.Expressions.NewExpression)
            {
                var newExpression = expression.Body as System.Linq.Expressions.NewExpression;
                foreach (var x in newExpression.Members)
                {
                    propertyChangedBase.RaisePropertyChanged(x.Name);
                }
            }
            else if (expression.Body is System.Linq.Expressions.MemberExpression)
            {
                var memberExpression = expression.Body as System.Linq.Expressions.MemberExpression;
                propertyChangedBase.RaisePropertyChanged(memberExpression.Member.Name);
            }
            /*
            var memberExpression = expression.Body as System.Linq.Expressions.MemberExpression;
            if (memberExpression != null)
            {
                string propertyName = memberExpression.Member.Name;
                propertyChangedBase.RaisePropertyChanged(propertyName);
            }
            else
                throw new NotImplementedException();
            */
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <typeparam name="TProperty">Model属性</typeparam>
        /// <param name="Entity">Model</param>
        /// <param name="newModel">新Model</param>
        /// <param name="linqExpression">不包含的属性 x => x.PropertyA 或 new { x.PropertyA, x.PropertyB }</param>
        public static void Update<T, TProperty>(this T Entity, T newModel = null, System.Linq.Expressions.Expression<Func<T, TProperty>> linqExpression = null) where T : abstractModel
        {
            List<string> NotContainProperties = new List<string>();
            if (linqExpression != null)
            {
                if (linqExpression.Body is System.Linq.Expressions.NewExpression)
                {
                    var x1 = linqExpression.Body as System.Linq.Expressions.NewExpression;
                    foreach (var x in x1.Members)
                    {
                        NotContainProperties.Add(x.Name);
                    }
                }
                else if (linqExpression.Body is System.Linq.Expressions.MemberExpression)
                {
                    var x2 = linqExpression.Body as System.Linq.Expressions.MemberExpression;
                    NotContainProperties.Add(x2.Member.Name);
                }
            }
            System.Type type = Entity.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (!NotContainProperties.Contains(property.Name))
                {
                    if (newModel != null)
                    {
                        if (property.CanWrite)
                        {
                            property.SetValue(Entity, property.GetValue(newModel, null), null);
                        }
                    }
                    Entity.RaisePropertyChanged(property.Name);
                }
            }
        }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, TProperty>(this T obj, System.Linq.Expressions.Expression<Func<T, TProperty>> expression) where T : new()
        {
            var memberExpression = expression.Body as System.Linq.Expressions.MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// 判断是否某个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyChangedBase"></param>
        /// <param name="expression"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool IsProperty<T, TProperty>(this T propertyChangedBase, System.Linq.Expressions.Expression<Func<T, TProperty>> expression, string propertyName) where T : abstractModel
        {
            var memberExpression = expression.Body as System.Linq.Expressions.MemberExpression;
            return memberExpression == null ? false : (memberExpression.Member.Name == propertyName);
        }

        /// <summary>
        /// 创建当前对象的浅表副本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Copy<T>(this T value) where T : abstractModel
        {
            return value.Clone() as T;
        }
    }
}
