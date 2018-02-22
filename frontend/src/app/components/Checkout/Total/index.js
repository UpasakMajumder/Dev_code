import React from 'react';
import PropTypes from 'prop-types';
/* components */
import PriceTable from 'app.dump/PriceTable';

const Total = ({ ui }) => {
  const { title, description, items } = ui;

  const descriptionElement = description ? <p className="cart-fill__info">{description}</p> : null;

  return (
    <div id="totals">
      <h2>{title}</h2>
      <div className="cart-fill__block">
        {descriptionElement}
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
    title: PropTypes.string.isRequired,
    description: PropTypes.string,
    items: PropTypes.arrayOf(PropTypes.object.isRequired).isRequired
  }).isRequired
};

export default Total;
