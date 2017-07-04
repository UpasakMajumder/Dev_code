import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';

const ShippingInfo = ({ ui }) => {
  const { title, deliveryMethod, address, tracking } = ui;

  let trackingLink = null;

  if (tracking) {
    const { url, text } = tracking;
    trackingLink = <a className="link" href={url}>{text}</a>;
  }

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

ShippingInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    deliveryMethod: PropTypes.string.isRequired,
    address: PropTypes.string.isRequired,
    tracking: PropTypes.shape({
      url: PropTypes.string,
      text: PropTypes.string
    })
  })
};

export default ShippingInfo;
