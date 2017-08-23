/* helpers */
import { addToCartRequest } from 'app.helpers/api';
import { getSearchObj } from 'app.helpers/location';
import { consoleException } from 'app.helpers/io';

class AddToCart {
  constructor(button) {
    const showMessageClass = 'input--error';
    const nameElement = document.querySelector('.js-add-to-cart-name');
    const quantityElement = document.querySelector('.js-add-to-cart-quantity');
    const properyFields = Array.from(document.querySelectorAll('.js-add-to-cart-property'));

    button.addEventListener('click', () => {
      const wrapper = document.querySelector('.js-add-to-cart-error');
      wrapper.classList.remove(showMessageClass);

      const customProductName = nameElement && nameElement.value;
      const quantity = quantityElement ? quantityElement.value : 0;

      const body = { customProductName, quantity };

      properyFields.forEach((field) => {
        const name = field.getAttribute('name');
        body[name] = field.value;
      });

      addToCartRequest(body)
        .then((message) => { // show if bad response
          wrapper.classList.add(showMessageClass);
          const messageElement = document.querySelector('.js-add-to-cart-message');
          messageElement.innerHTML = message;
        });
    });
  }
}

export default AddToCart;
