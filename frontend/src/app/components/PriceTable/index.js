import React from 'react';
import PropTypes from 'prop-types';
import PriceRow from './PriceRow';

const PriceTable = ({ items }) => {
  const prices = items.map((item, index) => {
    let className = 'summary-table__row';
    if (index === items.length - 1) className += ' summary-table__amount--emphasized';
    return <PriceRow className={className} key={item.title} {...item} />;
  });

  return (
    <div className="summary-table">
      {prices}
    </div>
  );
};

PriceTable.propTypes = {
  items: PropTypes.array.isRequired
};

export default PriceTable;
