using ContactList.Common.DependencyResolution;
using ContactList.Domain.Service.Interfaces.Repositories;
using ContactList.ORM.Contexts;
using ContactList.Test.Library.DatabaseConfig;
using ContactList.Test.Library.Traits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ContactList.ORM.Test.Features
{
    [TestClass]
    public class ContactRepositoryTest : EffortDataConfig
    {
        private const string CategoryGroup = "ORM";
        private const string CategorySubGroup = "ContactRepository";

        private readonly Lazy<IContactRepository> _contactRepository = IoC.GetLazy<IContactRepository>();


        [TestInitialize]
        public void TestInitialize()
        {
            GenerateContexts(typeof(Context));
        }

        [TestMethod]
        [FunctionalCategory(CategoryGroup, CategorySubGroup)]
        public void Deveria_criar_teste()
        {
            //Arrange


            //Action


            //Assert

        }


    }
}