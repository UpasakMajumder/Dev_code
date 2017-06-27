import React from 'react';
import PropTypes from 'prop-types';
import Product from '../../Product/Order';

const OrderedItems = ({ ui }) => {
  const { title, items } = ui;

  const products = items.map(item => <Product key={item.id} {...item} />);

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
  })
};

export default OrderedItems;
