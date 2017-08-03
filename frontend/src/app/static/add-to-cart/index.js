/* helpers */
import { addToCartRequest } from 'app.helpers/api';
import { getSearchObj } from 'app.helpers/location';
import { consoleException } from 'app.helpers/io';

class AddToCart {
  constructor(button) {
    const showMessageClass = 'input--error';
    const nameElement = document.querySelector('.js-add-to-cart-name');

    let name = '';
    if (nameElement) {
      name = nameElement.value;
    } else {
      consoleException('Not found element .js-add-to-cart-name');
    }

    const { documentId, quantity, templateId, containerId } = getSearchObj();
    const body = { name, documentId, quantity, templateId, containerId };

    button.addEventListener('click', () => {
      const wrappers = Array.from(document.querySelectorAll('.js-add-to-cart-error'));
      wrappers.forEach(wrapper => wrapper.classList.remove(showMessageClass));

      addToCartRequest(body)
        .then((message) => { // show if bad response
          wrappers.forEach(wrapper => wrapper.classList.add(showMessageClass));
          const messageElement = document.querySelector('.js-add-to-cart-message');
          messageElement.innerHTML = message;
        });
    });
  }
}

export default AddToCart;
