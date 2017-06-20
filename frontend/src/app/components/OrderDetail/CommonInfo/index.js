import React from 'react';
import SVG from '../../SVG';

export default ({ ui }) => {
  const { status, orderDate, shippingDate, totalCost } = ui;

  return (
    <div className="order-block">
      <div className="tile-bar">
        <div className="tile-bar__item">
          <SVG name="flag" className="icon-tile"/>
          <div>
            <p className="tile-bar__title">Status</p>
            <p className="tile-bar__description">{status}</p>
          </div>
        </div>
        <div className="tile-bar__item">
          <SVG name="calendar" className="icon-tile"/>
          <div>
            <p className="tile-bar__title">Order date</p>
            <p className="tile-bar__description">{orderDate}</p>
          </div>
        </div>
        <div className="tile-bar__item">
          <SVG name="truck" className="icon-tile"/>
          <div>
            <p className="tile-bar__title">Shipping date</p>
            <p className="tile-bar__description">{shippingDate}</p>
          </div>
        </div>
        <div className="tile-bar__item">
          <SVG name="dollar" className="icon-tile"/>
          <div>
            <p className="tile-bar__title">Status</p>
            <p className="tile-bar__description">{totalCost}</p>
          </div>
        </div>
      </div>
    </div>
  );
};
