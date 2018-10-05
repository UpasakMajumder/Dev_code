import axios from 'axios';

class ProductOptions {
  constructor(container) {
    this.container = container;
    this.url = container.dataset.url;
    this.priceElement = this.getPriceElement();
    this.stockElement = this.getStockElement();

    this.selected = {};

    const options = [...document.querySelectorAll('.js-product-option')];

    options.forEach((option) => {
      if (option.type === 'radio') {
        if (option.checked) {
          this.selected[option.name] = option.value;
        }
      } else {
        this.selected[option.name] = option.value;
      }

      option.addEventListener('change', this.handleClick);
    });
  }

  handleClick = (e) => {
    const element = e.currentTarget;
    const { value, name } = element;
    this.selected[name] = value;
    this.getNewPrice();
  }

  getNewPrice = () => {
    axios.post(this.url, this.selected)
      .then((response) => {
        const { errorMessage, payload, success } = response.data;

        if (success) {
          this.setNewPrice(payload);
        } else {
          console.error(errorMessage); // eslint-disable-line no-console
        }
      })
      .catch((e) => {
        console.error(e); // eslint-disable-line no-console
      });
  };

  setNewPrice = (data) => {
    const { pricePrefix, priceValue, availabilty } = data;
    if (availabilty) { this.stockElement.availability = `${availabilty}`; }
    this.priceElement.innerHTML = `${pricePrefix} ${priceValue}`;
  };

  getPriceElement = () => {
    const priceElementSelector = this.container.dataset.priceElement;
    const priceElement = document.querySelector(priceElementSelector);

    if (!priceElement) {
      console.error(`Element with selector ${priceElementSelector} is not found`); // eslint-disable-line no-console
    }

    return priceElement;
  }
  
  getStockElement = () => {
    const stockSelector = this.container.dataset.Stock;
    const stockElement = document.querySelector(stockSelector);

    if (!stockElement) {
      console.error(`Element with selector ${stockSelector} is not found`); // eslint-disable-line no-console
    }

    return stockElement;
  }
}

export default ProductOptions;
