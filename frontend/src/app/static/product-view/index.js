import { consoleException } from 'app.helpers/io';

class ProductView {
  constructor(button) {
    this.clicked = false;

    const { target } = button.dataset;
    if (!target) {
      consoleException('No data-target in', button);
      return;
    }

    const imgElement = document.querySelector(target);
    if (!imgElement) {
      consoleException(`No element with ${target}`);
      return;
    }

    button.addEventListener('click', this.downloadImage.bind(this, imgElement));
  }

  downloadImage(imgElement) {
    if (this.clicked) return;
    const { src } = imgElement.dataset;
    if (!src) {
      consoleException('No data-src in', imgElement);
      return;
    }

    imgElement.setAttribute('src', src);
    this.clicked = true;
  }
}

export default ProductView;
