using ContactList.Common.DependencyResolution;
using ContactList.Domain.Service.Interfaces.AppServices;
using ContactList.ORM.Contexts;
using ContactList.Test.Library.DatabaseConfig;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContactList.Service.Test.Services
{
    [TestClass]
    public class ContactListAppServiceAppServiceTest : EffortDataConfig
    {
        private readonly Lazy<IContactListAppServiceAppService> _appService = IoC.GetLazy<IContactListAppServiceAppService>();

        [TestInitialize]
        public void TestInitialize()
        {
            GenerateContexts(typeof(Context));
        }

        //[TestMethod]
        //[FunctionalCategory("Application", " ContactListAppServiceAppService")]
        //public void Deveria_recepcionar_a_quantidade_de_pre_documentos_do_shipment()
        //{
        //    //Arrange
            
        //    //Action
            
        //    //Assert
            
        //}
    }
}
