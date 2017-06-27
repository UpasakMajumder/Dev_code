import React from 'react';
import PropTypes from 'prop-types';
import PriceTable from '../../PriceTable';

const PricingInfo = ({ ui }) => {
  const { title, items } = ui;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <PriceTable items={items} />
      </div>
    </div>
  );
};

PricingInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    items: PropTypes.array.isRequired
  })
};

export default PricingInfo;
