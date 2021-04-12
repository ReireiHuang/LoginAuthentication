using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models.Account;
using TestProject.Component;

namespace TestProject.Biz.Account
{
    public class AccountManager : IDisposable
    {
        ResultHelper result;
        public User AccountVerify(UserViewModel user)
        {
            User cu = this.users.FirstOrDefault(a => a.Account == user.Account && a.Password == user.Password);
            if (cu != null)
                return cu;
            else
                return null;
        }

        private readonly IList<User> users = new List<User>
        {
            new User { Account ="test1",Password="test1",Role ="Admin"},
            new User { Account ="test2",Password="test2",Role ="Account"},
            new User { Account ="user",Password ="user",Role ="User"}
        };

        #region /// 物件 Dispose
        /// <summary>
        /// 物件 Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 物件 Dispose
        /// </summary>
        /// <param name="disposing">Dispose 判斷</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.result != null)
                {
                    this.result.Dispose();
                    this.result = null;
                }
            }
        }
        #endregion
    }
}
