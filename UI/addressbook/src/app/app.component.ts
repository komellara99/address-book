import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { CommonModule } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, HttpClientModule, FormsModule, ReactiveFormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'addressbook';

  contacts: any = [];
  selectedContact: Contact | null = null;
  newContact: Contact = new Contact(0, '', '', '', new Address('', '', '', '', ''), '', false);
  searchQuery: string = '';
  errorMessage:string = '';

  API = "http://localhost:8000/api/";

  addModalOpen = false;
  tryDeleting = false;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.get_contacts();
  }

  get_contacts(searchQuery: string = "") {
    const url = `${this.API}Contact?searchQuery=${searchQuery || ''}`;
    this.http.get(url).subscribe((res) => {
      this.contacts = res;
    })
  }
  onSearch() {
    this.get_contacts(this.searchQuery);
  }

  delete_contact(id: number) {
    this.http.delete(this.API + "Contact/" + id).subscribe((res) => {
      console.log(res);
      this.get_contacts();
      this.selectedContact = null;
      this.tryDeleting = false;
      this.closeModal();
    })
  }
  selectContact(contact: Contact) {
    this.selectedContact = JSON.parse(JSON.stringify(contact));
    this.closeAddModal();
  }

  edit_contact() {
    if (!this.selectedContact) return;
    var contact = this.selectedContact;
    const updatedContact = {
      firstName: contact.firstName,
      lastName: contact.lastName,
      phoneNumber: contact.phoneNumber,
      addressId: contact.addressId,
      address: contact.address
    };
    this.http.put(this.API + "Contact/" + contact.id, updatedContact).subscribe({
      next: (res) => {
        contact.isEditing = false;
        this.get_contacts();
        this.selectedContact = null;
      },
      error: (err) => {
        if (err.status === 400) {
          this.errorMessage = err.error?.message || "There was an error, check the format of the data, especially phone number.";
        }
      }
    });
  }
  editing_contact(contact: any) {
    contact.isEditing = true;
  }

  add_contact(contact: Contact) {
    const updatedContact = {
      firstName: contact.firstName,
      lastName: contact.lastName,
      phoneNumber: contact.phoneNumber,
      address: contact.address
    };
  
    this.http.post(this.API + "Contact", updatedContact).subscribe({
      next: (res) => {
        console.log(res);
        this.closeAddModal();
        this.get_contacts();
      },
      error: (err) => {
        if (err.status === 400) {
          this.errorMessage = err.error?.message || "There was an error, check the format of the data.";
        }
      }
    });
  }

  get_contact(id: number) {
    this.http.delete(this.API + "Contact/" + id).subscribe((res) => {
      console.log(res);
    })
  }
  closeModal() {
    this.selectedContact = null;
  }
  openAddModal() {
    this.closeModal();
    this.addModalOpen = true;
    this.newContact = new Contact(0, '', '', '', new Address('', '', '', '', ''), '', false);
  }

  closeAddModal() {
    this.addModalOpen = false;
    this.errorMessage = '';
  }
  try_delete() {
    this.tryDeleting = true;
  }
}


export class Address {
  street: string;
  houseNo: string;
  city: string;
  postCode: string;
  country: string;

  constructor(street: string, houseNo: string, city: string, postCode: string, country: string) {
    this.street = street;
    this.houseNo = houseNo;
    this.city = city;
    this.postCode = postCode;
    this.country = country;
  }
}

export class Contact {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: Address;
  addressId: string;
  isEditing: boolean

  constructor(id: number, firstName: string, lastName: string, phoneNumber: string, address: Address, addressId: string, isEditing: boolean) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.phoneNumber = phoneNumber;
    this.address = address;
    this.addressId = addressId;
    this.isEditing = isEditing;
  }
}

