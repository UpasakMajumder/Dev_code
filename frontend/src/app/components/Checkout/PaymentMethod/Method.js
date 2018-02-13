import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';

class Method extends Component {
  static propTypes = {
    validationMessage: PropTypes.string.isRequired,
    changePaymentMethod: PropTypes.func.isRequired,
    className: PropTypes.string.isRequired,
    checked: PropTypes.bool.isRequired,
    title: PropTypes.string.isRequired,
    icon: PropTypes.string.isRequired,
    id: PropTypes.number.isRequired,
    inputPlaceholder: PropTypes.string,
    disabled: PropTypes.bool,
    hasInput: PropTypes.bool,
    toggleInput: PropTypes.func.isRequired,
    shownInput: PropTypes.number.isRequired,
    checkedObj: PropTypes.shape({
      id: PropTypes.number.isRequired,
      invoice: PropTypes.string
    }).isRequired
  };

  changePaymentMethod = (name, id) => {
    this.props.changePaymentMethod(name, id);
    this.props.toggleInput(id);
  };

  render() {
    const {
      title,
      icon,
      disabled,
      id,
      inputPlaceholder,
      checkedObj,
      changePaymentMethod,
      shownInput,
      hasInput
    } = this.props;
    let { className } = this.props;

    const additionalInput = shownInput === id && hasInput
      ? (
        <div className="input__wrapper">
          <input
                onChange={(e) => { changePaymentMethod(e.target.name, id, e.target.value); }}
                type="text"
                className="input__text"
                name="paymentMethod"
                placeholder={inputPlaceholder}
                value={checkedObj.invoice}
          />
        </div>
      ) : null;

    if (disabled) className += ' input__wrapper--disabled';

    return (
      <div className={className}>
        <input disabled={disabled}
               onChange={(e) => { this.changePaymentMethod(e.target.name, id); }}
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
