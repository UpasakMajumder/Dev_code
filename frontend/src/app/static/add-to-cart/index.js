import { addToCartRequest } from 'app.helpers/api';


class AddToCart {
  addToCart() {
    addToCartRequest();
  }

  constructor(button) {
    // create promise
    // define id
    // define number
    // define errorMessage

    button.addEventListener('click', this.addToCart.bind(this));
  }
}

export default AddToCart;
