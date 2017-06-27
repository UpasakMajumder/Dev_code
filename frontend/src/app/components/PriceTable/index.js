import React from 'react';
import PropTypes from 'prop-types';
import PriceRow from './PriceRow';

const PriceTable = ({ items }) => {
  const prices = items.map((item, index) => {
    const { title, value } = item;
    let className = 'summary-table__row';
    if (index === items.length - 1) className += ' summary-table__amount--emphasized';
    return <PriceRow key={title} className={className} title={title} value={value} />;
  });

  return (
    <div className="summary-table">
      {prices}
    </div>
  );
};

PriceTable.propTypes = {
  items: PropTypes.array.isRequired // [{ title, value }]
};

export default PriceTable;
