using ContactList.Domain.Entities;
using ContactList.ORM.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;

namespace ContactList.Service.API.Controllers.OData
{
    public class ContactListODataController : ODataController
    {
        public IQueryable<Person> Get(ODataQueryOptions<Person> queryOptions)
        {
            using (var Repository = new ContactRepository())
            {
                return Repository.GetAll().AsNoTracking();
            }
        }
    }
}