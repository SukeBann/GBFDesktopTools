using System;
using System.Collections.Generic;

namespace GBFDesktopTools.Model.abstractModel
{
    /// <summary>
    /// 返回结果对象接口
    /// </summary>
    public interface IResultObj
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorMsg { get; set; }
    }

    /// <summary>
    /// 封装了目标对象，以及错误信息
    /// </summary>
    /// <typeparam name="T">泛型对象</typeparam>
    public class ObjectResult<T> : IResultObj
    {
        #region ErrorAndMark

        private string _ErrorMsg;
        private int _ErrorCode = 0;
        private string _Mark = string.Empty;

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="IsSystemError">是否为系统错误</param>
        /// <returns></returns>
        public ObjectResult<T> SetError(object error, int errorCode = 0, bool IsSystemError = false)
        {
            this.ErrorMsg = error == null ? null : ((IsSystemError ? "[SystemError]" : "") + error.ToString());
            this._ErrorCode = errorCode;
            return this;
        }

        /// <summary>
        /// 判断是否系统错误
        /// </summary>
        public bool HasSystemError
        {
            get
            {
                return this.hasError && this.ErrorMsg.IndexOf("SystemError", StringComparison.OrdinalIgnoreCase) > -1;
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set { _ErrorMsg = value; }
        }

        /// <summary>
        /// 是否包含错误
        /// </summary>
        public bool hasError
        {
            get
            {
                return ErrorMsg != null;
            }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Mark
        {
            get { return _Mark; }
            set { _Mark = value; }
        }

        #endregion ErrorAndMark

        #region Object

        private T _Obj;
        private List<T> _ObjList;
        private Dictionary<string, List<T>> _ObjStrDic;

        /// <summary>
        /// 单个对象
        /// </summary>
        public T Obj
        {
            get { return _Obj; }
            set { _Obj = value; }
        }

        /// <summary>
        /// 列表对象
        /// </summary>
        public List<T> ObjList
        {
            get { return _ObjList; }
            set { _ObjList = value; }
        }

        /// <summary>
        /// 键:字符串 值：对象list 字典集
        /// </summary>
        public Dictionary<string, List<T>> ObjStrDic
        {
            get { return _ObjStrDic; }
            set { _ObjStrDic = value; }
        }

        #endregion Object
    }
}