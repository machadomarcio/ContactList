using Owin;
using System.Web.Http;

namespace ContactList.Service.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);

            
            app.UseWebApi(config);


        }

        #region Private Methods


        #endregion
    }
}