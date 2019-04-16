import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http"
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { PersonComponent } from './persons/person/person.component';
import { PersonsComponent } from './persons/persons.component';
import { PersonListComponent } from './persons/person-list/person-list.component';
import { PersonService } from './shared/person.service';
import { NgxMaskModule } from 'ngx-mask';
import { NgxPhoneMaskBrModule } from 'ngx-phone-mask-br';


@NgModule({
  declarations: [
    AppComponent,
    PersonComponent,
    PersonsComponent,
    PersonListComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    NgxMaskModule.forRoot(),
    NgxPhoneMaskBrModule
  ],
  providers: [PersonService],
  bootstrap: [AppComponent]
})
export class AppModule { }
