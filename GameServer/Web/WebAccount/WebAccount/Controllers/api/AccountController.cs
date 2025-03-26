using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LitJson;
using Newtonsoft.Json.Linq;
using WebAccount.DBModel;
using WebAccount.Entity;

namespace WebAccount.Controllers.api
{
    public class AccountController : ApiController
    {
        // GET: api/Account
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Account/5
        public AccountEntity Get(int id)
        {
            return AccountDBModel.Instance.Get(id);
        }

        // POST: api/Account
        [HttpPost]
        public RetValue Post([FromBody] JObject json)
        {
            RetValue retValue = new RetValue();
            if (json != null)
            {
                try
                {
                    int type = Convert.ToInt32(json["Type"]);
                    string userName = json["UserName"]?.ToString();
                    string pwd = json["Pwd"]?.ToString();

                    if (type == 0)
                    {
                        //注册
                        retValue.Data = AccountDBModel.Instance.Register(userName, pwd);
                    }
                    else
                    {
                        //登录
                    }
                }
                catch(Exception e)
                {
                    retValue.HasError = true;
                    retValue.ErrorMsg = e.Message;
                }
            }

            return retValue;
        }

        // PUT: api/Account/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Account/5
        public void Delete(int id)
        {
        }
    }
}
