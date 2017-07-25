import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../SVG';

const Link = ({ text, href, disabled, isLoading, type, className }) => {
  const isDisabled = isLoading || disabled;

  let linkClassName = '';
  if (type) linkClassName += `btn-${type}`;
  if (className) linkClassName += ` ${className}`;

  const spinner = isLoading
    ? (
      <div className="btn__spinner">
        <SVG name="btn-spinner" />
      </div>
      )
    : null;


  return (
    <a href={href}
       disabled={isDisabled}
       className={linkClassName}>
      {spinner}
      {text}
    </a>
  );
};

Link.propTypes = {
  text: PropTypes.string.isRequired,
  href: PropTypes.string.isRequired,
  type: PropTypes.string,
  disabled: PropTypes.bool,
  isLoading: PropTypes.bool
};

export default Link;
