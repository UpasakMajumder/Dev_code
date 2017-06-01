import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Method extends Component {
  render() {
    const { id, title, pricePrefix, price, datePrefix, date, disabled, checkedId, changeShoppingData } = this.props;

    let className = 'input__wrapper select-accordion__item  select-accordion__item--inner';
    if (disabled) className += ' input__wrapper--disabled';

    const dating = (datePrefix && date)
      ? <span> | <span>{datePrefix} {date}</span></span>
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
            (<span>{pricePrefix} {price}</span>{dating} )
          </span>
        </label>
      </div>
    );
  }
}

export default Method;
