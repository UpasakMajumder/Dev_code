import React from 'react';
import SVG from '../../SVG';

export default ({ ui }) => {
  const { title, deliveryMethod, address, tracking } = ui;

  const trackingLink = tracking ? <a className="link" href={tracking.url}>{tracking.text}</a> : null;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <SVG name={deliveryMethod} />
        <p>{address}</p>
        {trackingLink}
      </div>
    </div>
  );
};
