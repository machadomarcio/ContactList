import { Injectable } from '@angular/core';
import { Person } from './person.model';
import { HttpClient } from "@angular/common/http"

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  formData;
  list;
  lisContact;
  readonly rootUrl = "https://contactlistserviceapi.azurewebsites.net/api/ContactList"
  constructor(private http: HttpClient) { }

  postPerson(formData) {
    return this.http.post(this.rootUrl + "/Save", formData);
  }

  postContact(contact) {
    return this.http.post(this.rootUrl + "/SaveContact", contact);
  }

  refreshList() {
    return this.http.get(this.rootUrl + "/Get").toPromise().then(res => {
      this.list = res;
    });
  }

  refreshListContacts(personId) {
    return this.http.get(this.rootUrl + "/GetContact/" + personId).toPromise().then(res => {
      this.lisContact = res;
    });
  }

  refreshListBySearch(text) {
    return this.http.get(this.rootUrl + "/GetByName/" + text).toPromise().then(res => this.list = res);
  }

  deletePerson(formData) {
    return this.http.post(this.rootUrl + "/DeletePerson", formData);
  }

  deleteContact(contact) {
    return this.http.post(this.rootUrl + "/DeleteContact", contact);
  }

}
