import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import { divideBySlash } from 'app.helpers/time';

const CommonInfo = ({ ui }) => {
  const { status, orderDate, shippingDate, totalCost } = ui;

  const tiles = [
    {
      title: 'Status',
      value: status,
      icon: 'flag'
    },
    {
      title: 'Order date',
      value: orderDate,
      icon: 'calendar'
    },
    {
      title: 'Shipping date',
      value: shippingDate,
      icon: 'truck'
    },
    {
      title: 'Total cost',
      value: totalCost,
      icon: 'dollar'
    }
  ];

  const tileList = tiles.map((tile) => {
    const { value, icon, title } = tile;
    return (
      <div key={icon} className="tile-bar__item">
        <SVG name={icon} className="icon-tile"/>
        <div>
          <p className="tile-bar__title">{title}</p>
          <p className="tile-bar__description">{title.includes('date') ? divideBySlash(value) : value}</p>
        </div>
      </div>
    );
  });

  return (
    <div className="order-block">
      <div className="tile-bar">
        {tileList}
      </div>
    </div>
  );
};

CommonInfo.propTypes = {
  ui: PropTypes.shape({
    status: PropTypes.string.isRequired,
    orderDate: PropTypes.string,
    shippingDate: PropTypes.string,
    totalCost: PropTypes.string.isRequired
  })
};

export default CommonInfo;
