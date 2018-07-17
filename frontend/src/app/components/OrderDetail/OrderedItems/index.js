import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Product from 'app.dump/Product/Order';

const OrderedItems = ({ ui, toggleEmailProof, showRejectionLabel }) => {
  const { title, items } = ui;

  const products = items.filter(item => item.quantity).map((item, i) => (
    <Product
      toggleEmailProof={toggleEmailProof}
      key={i}
      showRejectionLabel={showRejectionLabel}
      {...item}
    />
  ));

  return (
    <div className="order-block">
      <h2 className="order-block__header">{title}</h2>
      {products}
    </div>
  );
};

OrderedItems.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    items: PropTypes.array.isRequired
  }),
  toggleEmailProof: PropTypes.func.isRequired,
  showRejectionLabel: PropTypes.bool.isRequired
};

export default OrderedItems;
