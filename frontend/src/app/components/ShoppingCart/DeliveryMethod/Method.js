import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Method extends Component {
  render() {
    const { id, title, pricePrefix, price, datePrefix, date, disabled, checkedId, changeShoppingData } = this.props;

    let className = 'input__wrapper select-accordion__item  select-accordion__item--inner';
    if (disabled) className += ' input__wrapper--disabled';


    let dateElement = null;
    if (datePrefix && date) {
      dateElement = <span>{datePrefix} {date}</span>;
    } else if (datePrefix) {
      dateElement = <span>{datePrefix}</span>;
    } else if (date) {
      dateElement = <span>{date}</span>;
    }

    let priceElement = null;
    if (pricePrefix && price) {
      priceElement = <span>{pricePrefix} {price}</span>;
    } else if (pricePrefix) {
      priceElement = <span>{pricePrefix}</span>;
    } else if (price) {
      priceElement = <span>{price}</span>;
    }

    const stick = dateElement
      ? <span> | </span>
      : null;

    const extraInfo = (priceElement || dateElement)
      ? <span>({dateElement}{stick}{priceElement})</span>
      : null;

    return (
      <div className={className}>
        <input disabled={disabled}
               onChange={(e) => { changeShoppingData(e.target.name, id); }}
               checked={id === checkedId}
               type="radio"
               name="deliveryMethod"
               className="input__radio"
               id={`dm-${id}`}/>
        <label htmlFor={`dm-${id}`} className="input__label input__label--radio">
          {title}
          <span className="select-accordion__inner-label">
            {extraInfo}
          </span>
        </label>
      </div>
    );
  }
}

export default Method;
