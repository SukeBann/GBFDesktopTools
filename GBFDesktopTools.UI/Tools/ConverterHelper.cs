using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace GBFDesktopTools.View
{
    public class ConverterHelper : MarkupExtension,IValueConverter
    {
        public ConverterHelper()
        {
            GetConverterDic();
        }

        /// <summary>
        /// 转换器类型
        /// </summary>
        public enum ConverterEnumSelect
        {
            /// <summary>
            /// 如果数据源有数据则显示 无数据隐藏 可见性
            /// </summary>
            ItemsSourceCVisibility
        }

        private ConverterEnumSelect _ConverterType;
        /// <summary>
        /// 使用的转换器
        /// </summary>
        public ConverterEnumSelect ConverterType
        {
            get => _ConverterType;
            set
            {
                ConverterTypeStr = value.ToString();
                _ConverterType = value;
            }
        }

        /// <summary>
        /// 要使用的转换类型
        /// </summary>
        public string ConverterTypeStr { get; set; }
        /// <summary>
        /// 封装转换方法的字典集 使用ConverterTypeStr作为索引
        /// </summary>
        readonly Dictionary<string,Func<object, Type, object, CultureInfo, object>> ConverterFuncsDic = new Dictionary<string, Func<object, Type, object, CultureInfo, object>>();

        /// <summary>
        /// 获取转换方法字典集
        /// </summary>
        private void GetConverterDic()
        {
            //可见性转换
            var mscvFunc = new Func<object, Type, object, CultureInfo, object>(MItemsSourceCVisibility);
            ConverterFuncsDic.Add("ItemsSourceCVisibility", mscvFunc);
            
            /*
             *待添加
             */
        }

        #region Method

        /// <summary>
        /// 如果数据源有数据则显示 无数据隐藏 可见性
        /// </summary>
        /// <returns>Visibility</returns>
        private object MItemsSourceCVisibility(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var count = System.Convert.ToInt32(value);
            return count > 0 ? Visibility.Visible : Visibility.Hidden;
        }
        
        #endregion


        #region MainFunc

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConverterFuncsDic[ConverterTypeStr](value,targetType,parameter,culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new ConverterHelper(){ ConverterType  =this.ConverterType};
        }

        #endregion
    }
}