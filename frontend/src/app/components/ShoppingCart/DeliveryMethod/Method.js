import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Method extends Component {
  render() {
    const { id, title, pricePrefix, price, datePrefix, date, disabled, checkedId, changeShoppingData } = this.props;

    let className = 'input__wrapper select-accordion__item  select-accordion__item--inner';
    if (disabled) className += ' input__wrapper--disabled';

    const priceElement = (pricePrefix && price)
      ? <span> | <span>{pricePrefix} {price}</span></span>
      : (pricePrefix)
      ? <span> | <span>{pricePrefix}</span></span>
      : (price)
      ? <span> | <span>{price}</span></span>
      : null;

    const dateElement = (datePrefix && date)
      ? <span>{datePrefix} {date}</span>
      : (datePrefix)
      ? <span>{datePrefix}</span>
      : (date)
      ? <span>{date}</span>
      : null;

    const extraInfo = (priceElement || dateElement)
      ? <span>({dateElement}{priceElement})</span>
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
