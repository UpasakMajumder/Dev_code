class ReplaceValue {
  constructor(button) {
    this.fieldFromClass = 'js-replace-value-from';
    this.fieldToClass = 'js-replace-value-to';
    const { replaceValueId } = button.dataset;

    this.fieldsFrom = document.querySelectorAll(`.${this.fieldFromClass}[data-replace-value-id="${replaceValueId}"]`);

    button.addEventListener('click', this.replace.bind(this));
  }

  replace() {
    this.fieldsFrom.forEach((fieldFrom) => {
      const value = fieldFrom.innerHTML;
      const { replaceId } = fieldFrom.dataset;
      const fieldsTo = document.querySelectorAll(`.${this.fieldToClass}[data-replace-id="${replaceId}"]`);

      Array.from(fieldsTo).forEach((fieldTo) => {
        fieldTo.value = value;
      });
    });
  }

}

export default ReplaceValue;
