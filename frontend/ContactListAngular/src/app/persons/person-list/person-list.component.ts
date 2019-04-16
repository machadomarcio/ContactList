import { Component, OnInit } from '@angular/core';
import { PersonService } from 'src/app/shared/person.service';
import { Person } from 'src/app/shared/person.model';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.css']
})
export class PersonListComponent implements OnInit {

  constructor(public service: PersonService, public toastr: ToastrService) { }

  ngOnInit() {
    this.service.refreshList();
  }

  populateForm(person) {
    this.service.formData = person;
    this.service.lisContact = this.service.refreshListContacts(person.id);
  }

  deletePerson(person) {
    this.service.deletePerson(person).subscribe(res => {
      this.service.refreshList();
      this.toastr.success("Register Deleted Successfully", "Person Register");
      document.location.reload(true);
    });
  }

  onKeydown(text) {
    if (text)
      this.service.refreshListBySearch(text);
    else
      this.service.refreshList();
  }

}
