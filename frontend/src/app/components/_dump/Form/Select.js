import React from 'react';
/* utilities */
import removeProps from 'app.helpers/object';

const Select = (props) => {
  const { label, error, disabled, isOptional, options, onChange, value } = props;

  const labelElement = label ? <span className="input__label">{label}</span> : null;
  const className = disabled ? 'input__wrapper input__wrapper--disabled' : 'input__wrapper';
  const errorElement = error ? <span className="input__error input__error--noborder">{error}</span> : null;
  const errorClass = error ? 'input--error' : '';
  const optionalLabel = isOptional ? <span className="input__right-label">optional</span> : null;
  const optionList = options.map(option => <option key={option} value={option}>{option}</option>);
  const placeholer = label ? <option disabled={true}>{label}</option> : null;

  return (
    <div className={className}>
      {labelElement}
      {optionalLabel}
      <div className={`input__select ${errorClass}`}>
        <select value={value}
                className={errorClass}
                required={!isOptional}
                onChange={onChange}>
          {placeholer}
          {optionList}
        </select>
      </div>
      {errorElement}
    </div>
  );
};

export default Select;
