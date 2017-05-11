export default class AddTr {
  constructor(container) {
    this.lastRow = container;
    this.count = 1;
    this.tbody = this.lastRow.parentNode;
    this.firstRowClass = 'js-first-tr';
    this.firstRow = this.tbody.querySelector(`.${this.firstRowClass}`);
    this.firstRowTemplate = this.firstRow.cloneNode(true);

    const togglers = Array.from(this.lastRow.getElementsByClassName('js-add-tr-toggler'));

    togglers.forEach((toggler) => {
      toggler.addEventListener('click', () => {
        this.count += 1;
        const clonnedRow = this.firstRowTemplate.cloneNode(true);
        const newNode = this.getNewRow(clonnedRow);
        this.tbody.insertBefore(newNode, this.lastRow);
      });
    });
  }

  getNewRow(oldRow) {
    oldRow.classList.remove(this.firstRowClass);
    const elements = Array.from(oldRow.querySelectorAll('[name]'));
    elements.forEach((element) => {
      const name = `${element.dataset.name}-${this.count}`;
      element.name = name;
    });
    return oldRow;
  }
}
