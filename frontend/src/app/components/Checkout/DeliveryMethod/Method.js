import React from 'react';
import PropTypes from 'prop-types';

const Method = ({
  id,
  title,
  pricePrefix,
  price,
  datePrefix,
  date,
  disabled,
  checkedId,
  changeDeliveryMethod
}) => {
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

  let extraInfo = null;
  if (priceElement && dateElement) {
    extraInfo = <span>({dateElement}{stick}{priceElement})</span>;
  } else if (dateElement) {
    extraInfo = <span>({dateElement})</span>;
  } else if (priceElement) {
    extraInfo = <span>({priceElement})</span>;
  }

  return (
    <div className={className}>
      <input disabled={disabled}
             onChange={(e) => { changeDeliveryMethod(e.target.name, id); }}
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
};

Method.propTypes = {
  changeDeliveryMethod: PropTypes.func.isRequired,
  checked: PropTypes.bool.isRequired,
  title: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired,
  pricePrefix: PropTypes.string,
  datePrefix: PropTypes.string,
  checkedId: PropTypes.number,
  disabled: PropTypes.bool,
  price: PropTypes.string,
  date: PropTypes.string
};

export default Method;
