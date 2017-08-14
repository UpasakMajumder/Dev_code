/* helpers */
import { addToCartRequest } from 'app.helpers/api';
import { getSearchObj } from 'app.helpers/location';
import { consoleException } from 'app.helpers/io';

class AddToCart {
  constructor(button) {
    const showMessageClass = 'input--error';
    const nameElement = document.querySelector('.js-add-to-cart-name');
    const quantityElement = document.querySelector('.js-add-to-cart-quantity');

    button.addEventListener('click', () => {
      const wrapper = document.querySelector('.js-add-to-cart-error');
      wrapper.classList.remove(showMessageClass);

      const name = nameElement && nameElement.value;

      let quantity = 0;
      if (quantityElement) quantity = quantityElement.value;

      const { documentId, templateId, containerId } = getSearchObj();
      const body = { name, documentId, quantity, templateId, containerId };

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
