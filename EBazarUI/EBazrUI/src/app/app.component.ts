import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { Stripe, loadStripe } from '@stripe/stripe-js';
import { FloatLabelType, MatFormField } from '@angular/material/form-field';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'EBazrUI';
  stripePromise!: Promise<Stripe | null>;
  panelOpenState = false;
  floatLabelControl = new FormControl('card' as FloatLabelType);
  checkoutForm = this.fb.group({
    nume: ['', Validators.required],
    prenume: ['', Validators.required],
    adresa: ['', Validators.required],
    tara: ['', Validators.required],
    oras: ['', Validators.required],
    telefon: ['', Validators.required],
  });

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.stripePromise = loadStripe('pk_test_51OLSd1DxTNeHRNNESumPaBUaeETi2444rIzsaKWYJo7gH1cyPuXLK7Wf0jPO1j6XTRW46kqldfFrpE3a42jqJS8x00LvCd0G73');
  }

  checkout() {
    let formData = this.checkoutForm.value;
    let requestBody = {
      Username: "rinualex",
      Address: formData.adresa,
      Country: formData.tara,
      Phone: formData.telefon,
      City: formData.oras
    };
    this.http.post('https://localhost:5001/api/ShoppingCart/Checkout', requestBody)
      .subscribe(async (session: any) => {
        const stripe = await this.stripePromise;
        if (stripe) {
          const { error } = await stripe.redirectToCheckout({
            sessionId: session.id,
          });

          if (error) {
            console.error('There was an error redirecting to checkout: ', error);
          }
        }
      });
  }
}

