import React from 'react';
import PropTypes from 'prop-types';

const Method = ({
  id,
  label,
  checked,
  changePaymentCard
}) => {
  const className = 'input__wrapper select-accordion__item  select-accordion__item--inner';

  return (
    <div className={className}>
      <input
        checked={checked}
        type="radio"
        name="paymentSubMethod"
        className="input__radio"
        id={`pm-${id}`}
        onChange={() => changePaymentCard(id)}
      />
      <label htmlFor={`pm-${id}`} className="input__label input__label--radio">
        {label}
      </label>
    </div>
  );
};

Method.propTypes = {
  id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
  label: PropTypes.string.isRequired,
  checked: PropTypes.bool.isRequired,
  changePaymentCard: PropTypes.func.isRequired
};

export default Method;
