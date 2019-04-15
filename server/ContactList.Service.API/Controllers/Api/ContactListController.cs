using ContactList.Common.DependencyResolution;
using ContactList.Domain.Entities;
using ContactList.Domain.Models;
using ContactList.Domain.Service.Interfaces.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ContactList.Service.API
{
    [RoutePrefix("api/ContactList")]
    public class ContactListController : ApiController
    {

        internal Lazy<IContactListAppServiceAppService> ContactListAppServiceAppService { get; set; }

        public ContactListController()
        {
            ContactListAppServiceAppService = IoC.GetLazy<IContactListAppServiceAppService>();
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult Get(Guid id)
        {
            try
            {
                Person contact = ContactListAppServiceAppService.Value.GetById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetByName/{name}")]
        public IHttpActionResult GetByName(string name)
        {
            try
            {
                List<PersonModel> contacts = ContactListAppServiceAppService.Value.GetAll()
                    .Select(x => new PersonModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ContactValues = x.ContactValues.Select(y => new ContactValueModel
                        {
                            Id = y.Id,
                            PersonId = y.PersonId,
                            Value = y.Value,
                            IsWhatsApp = y.IsWhatsApp,
                            IsEmail = y.IsEmail,
                            IsPhone = y.IsPhone
                        }).ToList()
                    }).ToList();
                return Ok(contacts.Where(x => x.FirstName.ToUpper().Contains(name.ToUpper()) || x.LastName.ToUpper().Contains(name.ToUpper())));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Get()
        {
            try
            {
                List<PersonModel> contacts = ContactListAppServiceAppService.Value.GetAll()
                    .Select(x => new PersonModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        ContactValues = x.ContactValues.Select(y => new ContactValueModel
                        {
                            Id = y.Id,
                            PersonId = y.PersonId,
                            Value = y.Value,
                            IsWhatsApp = y.IsWhatsApp,
                            IsEmail = y.IsEmail,
                            IsPhone = y.IsPhone
                        }).ToList()
                    }).ToList();
                return Ok(contacts);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost, Route("Save")]
        public IHttpActionResult Save([FromBody] Person person)
        {
            try
            {
                ContactListAppServiceAppService.Value.Save(person);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost, Route("SaveContact")]
        public IHttpActionResult SaveContact([FromBody] ContactValue contact)
        {
            try
            {
                ContactListAppServiceAppService.Value.SaveContact(contact);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        

        [HttpPost]
        [Route("DeletePerson")]
        public IHttpActionResult DeletePerson([FromBody] Person person)
        {
            try
            {
                ContactListAppServiceAppService.Value.Delete(person.Id);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("DeleteContact")]
        public IHttpActionResult DeleteContact([FromBody] ContactValue contact)
        {
            try
            {
                ContactListAppServiceAppService.Value.DeleteContact(contact.Id);
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}