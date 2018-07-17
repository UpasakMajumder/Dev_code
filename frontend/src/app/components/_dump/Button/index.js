// @flow
import * as React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';

const Button = (props: {
  text: string,
  type: string,
  btnClass: ?string,
  disabled: ?boolean,
  onClick: ?() => void,
  isLoading: ?boolean,
  children: ?React.Children
}) => {
  const { text, type, disabled, isLoading, onClick, btnClass, children } = props;
  const isDisabled: boolean = !!isLoading || !!disabled;

  const spinner: ?{} = isLoading
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
      {children}
    </button>
  );
};

Button.propTypes = {
  text: PropTypes.string,
  type: PropTypes.string.isRequired,
  btnClass: PropTypes.string,
  disabled: PropTypes.bool,
  onClick: PropTypes.func,
  isLoading: PropTypes.bool
};

export default Button;
