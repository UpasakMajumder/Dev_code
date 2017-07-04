import React from 'react';
import PropTypes from 'prop-types';
import SVG from 'app.dump/SVG';

const Button = ({ text, type, disabled, isLoading, onClick, btnClass }) => {
  const isDisabled = isLoading || disabled;

  const spinner = isLoading
    ? (
      <div className="btn__spinner">
        <SVG name="btn-spinner" />
      </div>
    )
    : null;

  const className = `btn-${type} ${btnClass || ''}`;

  return (
    <button onClick={onClick}
            type="button"
            disabled={isDisabled}
            className={className}>
      {spinner}
      {text}
    </button>
  );
};

Button.propTypes = {
  text: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  btnClass: PropTypes.string,
  disabled: PropTypes.bool,
  onClick: PropTypes.func,
  isLoading: PropTypes.bool
};

export default Button;
