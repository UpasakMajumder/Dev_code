/* helpers */
import { addToCartRequest } from 'app.helpers/api';

class AddToCart {
  constructor(button) {
    this.showMessageClass = 'input--error';
    this.nameElement = document.querySelector('.js-add-to-cart-name');
    this.quantityElement = document.querySelector('.js-add-to-cart-quantity');
    this.wrappers = Array.from(document.querySelectorAll('.js-add-to-cart-error'));
    this.properyFields = Array.from(document.querySelectorAll('.js-add-to-cart-property'));

    if (button) button.addEventListener('click', this.addToCartRequest);
  }

  getBody = () => {
    const customProductName = this.nameElement && this.nameElement.value;
    const quantity = this.quantityElement ? this.quantityElement.value : 0;

    const body = { customProductName, quantity };

    this.properyFields.forEach((field) => {
      const name = field.getAttribute('name');
      body[name] = field.value;
    });

    return body;
  };

  addToCartRequest = (event) => {
    this.wrappers.forEach(wrapper => wrapper.classList.remove(this.showMessageClass));

    addToCartRequest(this.getBody(), event)
      .then((message) => { // show if bad response
        this.wrappers.forEach(wrapper => wrapper.classList.add(this.showMessageClass));
        const messageElement = document.querySelector('.js-add-to-cart-message');
        messageElement.innerHTML = message;
      });
  };
}

export default AddToCart;
