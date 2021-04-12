using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Component
{
    public class ResultHelper
    {
        private IDictionary<string, Object> _dictionary;
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string ResultMessage { set; get; }
        /// <summary>
        /// 執行狀況
        /// </summary>
        public bool Status { set; get; }
        /// <summary>
        /// 回傳執行後的結果物件
        /// </summary>
        public IDictionary<string, Object> DictionaryValue
        {
            get
            {
                return _dictionary;
            }
        }


        /// <summary>
        /// 建構子
        /// </summary>
        public ResultHelper()
        {
            _dictionary = null;
            this.Status = false;
        }


        /// <summary>
        /// 新增回傳物件
        /// </summary>
        /// <param name="Key">名稱</param>
        /// <param name="Value">物件</param>
        public void AddDictionary(string Key, Object Value)
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<string, Object>();
            }

            _dictionary.Add(Key, Value);
        }

        /// <summary>
        /// 移除回傳物件
        /// </summary>
        /// <param name="Key">名稱</param>
        public void RemoveDictionary(string Key)
        {
            if (_dictionary != null)
            {
                _dictionary.Remove(Key);
            }

        }

        #region Dispose

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">是否執行dispose</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dictionary != null)
                {
                    this._dictionary.Clear();
                    this._dictionary = null;
                }
            }
        }
        #endregion

    }

    #region  列舉

    /// <summary>
    /// 狀態
    /// </summary>
    public enum Status : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 失敗
        /// </summary>
        Failure = 0
    }
    #endregion
}
