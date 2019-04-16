import { Component, OnInit } from '@angular/core';
import { PersonService } from 'src/app/shared/person.service';
import { NgForm } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {

  constructor(public service: PersonService, public toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  changeWhatsappOrPhone(value) {
    if (value)
      this.service.formData.email = undefined;
  }

  populateContact(contact) {
    this.service.formData.contactValue = contact.value;
    this.service.formData.email = contact.isEmail;
    this.service.formData.phone = contact.isPhone;
    this.service.formData.whatsapp = contact.isWhatsApp;
    this.service.formData.contactid = contact.id;
  }

  onSearchChange($event: KeyboardEvent) {
    if (this.service.formData.whatsapp || this.service.formData.phone) {
      console.log($event)
      let value = (<HTMLInputElement>event.target).value;
      if ($event.target) {
        if (value == "") {
          value = value.slice(0, 0);
        }

        if (value.length > 11) {
          value = value.slice(0, 11)
        }
        (<HTMLInputElement>event.target).value = value.replace(/\D/g, '');
      }
    }
  }

  changeEmail(value) {
    if (value)
      this.service.formData.whatsapp = undefined;
    this.service.formData.phone = undefined;
  }

  updateContact(valueContact, personId, phone, whatsapp, email, contactId) {
    if ((phone == undefined || phone == false) && (email == undefined || email == false) && (whatsapp == undefined || whatsapp == false))
      this.toastr.warning("Select the type of contact(phone, whatsapp or email)", "Contact Register");
    else if (valueContact == null || valueContact == undefined || valueContact == "") {
      this.toastr.warning("Required contact", "Contact Register");
    } else {
      if (personId == undefined || personId == null) {
        if (whatsapp || phone)
          valueContact = (<HTMLInputElement>document.getElementById("contactEdit")).value;
        else
          valueContact = (<HTMLInputElement>document.getElementById("contactEditEmail")).value;
        var contact = { id: contactId, value: valueContact, personId: personId, isPhone: phone, isEmail: email, isWhatsApp: whatsapp };
        this.service.lisContact.push(contact);
      } else {
        if (whatsapp || phone)
          valueContact = (<HTMLInputElement>document.getElementById("contactEdit")).value;
        else
          valueContact = (<HTMLInputElement>document.getElementById("contactEditEmail")).value;

        var contact = { id: contactId, value: valueContact, personId: personId, isPhone: phone, isEmail: email, isWhatsApp: whatsapp };
        this.service.postContact(contact).subscribe(resp => {
          this.service.lisContact = null;
          this.toastr.success("Contact Registered Successfully", "Contact Register");
          this.service.formData.email = undefined;
          this.service.formData.phone = undefined;
          this.service.formData.whatsapp = undefined;
          this.service.formData.contactValue = undefined;
          this.service.formData.contactid = undefined;
          (<HTMLInputElement>document.getElementById("contactEdit")).value = '';
          this.service.lisContact = this.service.refreshListContacts(personId);
        });
      }
    }
  }

  addContact(valueContact, personId, phone, whatsapp, email) {
    if ((phone == undefined || phone == false) && (email == undefined || email == false) && (whatsapp == undefined || whatsapp == false))
      this.toastr.warning("Select the type of contact(phone, whatsapp or email)", "Contact Register");
    else if (valueContact == null || valueContact == undefined || valueContact == "") {
      this.toastr.warning("Required contact", "Contact Register");
    } else {
      if (personId == undefined || personId == null) {
        if (whatsapp || phone)
          valueContact = (<HTMLInputElement>document.getElementById("contactEdit")).value;
        else
          valueContact = (<HTMLInputElement>document.getElementById("contactEditEmail")).value;
        var contact = { value: valueContact, personId: personId, isPhone: phone, isEmail: email, isWhatsApp: whatsapp };
        this.service.lisContact.push(contact);
      } else {
        if (whatsapp || phone)
          valueContact = (<HTMLInputElement>document.getElementById("contactEdit")).value;
        else
          valueContact = (<HTMLInputElement>document.getElementById("contactEditEmail")).value;

        var contact = { value: valueContact, personId: personId, isPhone: phone, isEmail: email, isWhatsApp: whatsapp };
        this.service.postContact(contact).subscribe(resp => {
          this.service.lisContact.push(contact);
          this.toastr.success("Contact Registered Successfully", "Contact Register");
          this.service.formData.email = undefined;
          this.service.formData.phone = undefined;
          this.service.formData.whatsapp = undefined;
          this.service.formData.contactValue = undefined;
          (<HTMLInputElement>document.getElementById("contactEdit")).value = '';
        });
      }
    }
  }

  deleteContact(contact) {
    if (contact.id) {
      this.service.deleteContact(contact).subscribe(resp => {
        for (var i = this.service.lisContact.length - 1; i >= 0; i--) {
          if (this.service.lisContact[i].id === contact.id) {
            this.service.lisContact.splice(i, 1);
          }
        }
        this.toastr.success("Contact Deleted Successfully", "Person Register");
      });
    } else {
      for (var i = this.service.lisContact.length - 1; i >= 0; i--) {
        if (this.service.lisContact[i].value === contact.value) {
          this.service.lisContact.splice(i, 1);
        }
      }
      this.toastr.success("Contact Deleted Successfully", "Person Register");
    }
    this.service.formData.email = undefined;
    this.service.formData.phone = undefined;
    this.service.formData.whatsapp = undefined;
    this.service.formData.contactValue = undefined;
    this.service.formData.contactid = undefined;
    (<HTMLInputElement>document.getElementById("contactEdit")).value = '';
    this.service.lisContact = this.service.refreshListContacts(this.service.formData.id);
  }

  resetForm(form?: NgForm) {
    if (form != null && form != undefined)
      form.resetForm();
    this.service.formData = {
      id: null,
      firstName: '',
      lastName: '',
      search: undefined,
      whatsApp: undefined
    }

    this.service.lisContact = [];
    this.service.formData.contactid;

    this.service.refreshList();
  }

  onSubmit(form: NgForm) {
    form.value.ContactValues = this.service.lisContact;
    this.saveRecorder(form);
  }

  clearForm(form) {
    this.resetForm(form);
    if ((<HTMLInputElement>document.getElementById("contactEdit")))
      (<HTMLInputElement>document.getElementById("contactEdit")).value = '';

    if ((<HTMLInputElement>document.getElementById("contactEditEmail")))
      (<HTMLInputElement>document.getElementById("contactEditEmail")).value = '';
  }

  saveRecorder(form: NgForm) {
    var msg = form.value.Id === null ? "Inserted" : "Updated";
    this.service.postPerson(form.value).subscribe(resp => {
      this.toastr.success(form.value.FirstName + " " + msg + " Successfully", "Person Register");
      document.location.reload(true);
    });
  }
}
