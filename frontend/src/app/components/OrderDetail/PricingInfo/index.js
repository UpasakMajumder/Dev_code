import React from 'react';
import PriceTable from '../../PriceTable';

export default ({ ui }) => {
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
