import React from 'react';
import Product from '../../Product/Order';

export default ({ ui }) => {
  const { title, items } = ui;

  const products = items.map(item => <Product key={item.id} {...item} />);

  return (
    <div className="order-block">
      <h2 className="order-block__header">{title}</h2>
      {products}
    </div>
  );
};
