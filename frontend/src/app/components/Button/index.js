import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../SVG';

const Button = ({ text, type, disabled, isLoading, onClick }) => {
  const isDisabled = isLoading || disabled;

  return (
    <button onClick={onClick}
            type="button"
            disabled={isDisabled}
            className={`btn-${type}`}>
      <div className="btn__spinner">
        <SVG name="btn-spinner" />
      </div>
      {text}
    </button>
  );
};

Button.propTypes = {
  text: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  disabled: PropTypes.bool,
  onClick: PropTypes.func,
  isLoading: PropTypes.bool
};

export default Button;
