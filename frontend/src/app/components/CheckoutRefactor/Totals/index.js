import React from 'react';
import PropTypes from 'prop-types';
/* components */
import PriceTable from './PriceTable';

const Total = ({
  ui,
  items
}) => {
  return (
    <div id="totals">
      <h2>{ui.title}</h2>
      <div className="cart-fill__block">
        <div className="cart-fill__block-inner">
          <div className="cart-fill__summary-table">
            <PriceTable items={items} />
          </div>
        </div>
      </div>
    </div>
  );
};

Total.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired
  }).isRequired,
  items: PropTypes.arrayOf(PropTypes.object.isRequired).isRequired
};

export default Total;
