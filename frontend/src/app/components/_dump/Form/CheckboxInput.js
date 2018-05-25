import React from 'react';
import PropTypes from 'prop-types';
/* utilities */
import { removeProps } from 'app.helpers/object';

export default function CheckboxInput(props) {
  const { error, label, inputClass, disabled, type } = props;

  const inputProps = removeProps(props, ['error', 'key', 'label', 'inputClass']);

  const labelElement = label ? <label dangerouslySetInnerHTML={{ __html: label }} htmlFor={props.id} className={`input__label input__label--${type}`} /> : null;
  const wrapClass = disabled
    ? 'input__wrapper input__wrapper--disabled '
    : 'input__wrapper';
  const errorElement = error ? <span className="input__error">{error}</span> : null;

  return (
    <div className={wrapClass}>
      <input
        className={`${inputClass || ''} input__${type} `}
        {...inputProps} />
      {labelElement}
      {errorElement}
    </div>
  );
}

CheckboxInput.propTypes = {
  id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
  label: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  name: PropTypes.string,
  disabled: PropTypes.bool,
  error: PropTypes.string,
  inputClass: PropTypes.string,
  defaultChecked: PropTypes.bool,
  value: PropTypes.oneOfType([PropTypes.number, PropTypes.string, PropTypes.bool])
};
