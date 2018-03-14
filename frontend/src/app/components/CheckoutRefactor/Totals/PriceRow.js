import React from 'react';
import PropTypes from 'prop-types';

const Price = ({ title, value, className }) => {
  return (
    <div className={className}>
      <span className="summary-table__info">{title}</span>
      <span className="summary-table__line"> </span>
      <span className="summary-table__amount">{value}</span>
    </div>
  );
};

Price.propTypes = {
  title: PropTypes.string.isRequired,
  value: PropTypes.string,
  className: PropTypes.string
};

export default Price;
