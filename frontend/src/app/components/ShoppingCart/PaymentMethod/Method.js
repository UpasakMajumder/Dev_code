import React, { Component } from 'react';
import PropTypes from 'prop-types';
import SVG from '../../SVG';

class Method extends Component {
  constructor() {
    super();

    this.isInvalidField = this.isInvalidField.bind(this);
  }

  isInvalidField(field) {
    const { validationMessage } = this.props;
    return '';
  }

  render() {
    const { title, icon, disabled, id, hasInput, inputPlaceholder, checkedObj,
      changeShoppingData } = this.props;
    let { className } = this.props;

    const invoiceInvalidMessage = this.isInvalidField('invoice');

    const additionalInput = hasInput
      ? <div className="input__wrapper">
          <input onChange={(e) => { changeShoppingData(e.target.name, id, e.target.value); }}
                 type="text"
                 className={`input__text ${invoiceInvalidMessage ? 'input--error' : ''}`}
                 name="paymentMethod"
                 placeholder={inputPlaceholder}
                 value={checkedObj.invoice} />
          {
            invoiceInvalidMessage
            ? <span className="input__error input__error--noborder">{invoiceInvalidMessage}</span>
            : null
          }
        </div>
      : null;

    if (disabled) className += ' input__wrapper--disabled';

    return (
      <div className={className}>
        <input disabled={disabled}
               onChange={(e) => { changeShoppingData(e.target.name, id); }}
               checked={id === checkedObj.id}
               id={`pm-${id}`}
               name="paymentMethod"
               type="radio"
               className="input__radio" />
        <label htmlFor={`pm-${id}`} className="input__label input__label--radio">
          <SVG name={icon} className='icon-shipping' />
          <span>{title}</span>
        </label>
        {additionalInput}
      </div>
    );
  }
}

export default Method;
