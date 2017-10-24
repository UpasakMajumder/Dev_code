import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
import USAddress from 'app.dump/USAddress';

const ShippingInfo = ({ ui }) => {
  const { title, deliveryMethod, address, message, tracking } = ui;

  let trackingLink = null;

  if (tracking) {
    const { url, text } = tracking;
    trackingLink = <a className="link" href={url}>{text}</a>;
  }

  const addressEl = address
    ? <USAddress {...address} />
    : <p>{message}</p>;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <SVG name={deliveryMethod} />
        <div className="order-block__detail-address">{addressEl}</div>
        {trackingLink}
      </div>
    </div>
  );
};

ShippingInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    deliveryMethod: PropTypes.string.isRequired,
    address: PropTypes.shape({
      address1: PropTypes.string.isRequired,
      city: PropTypes.string.isRequired,
      state: PropTypes.string.isRequired,
      zip: PropTypes.string.isRequired
    }).isRequired,
    message: PropTypes.string,
    tracking: PropTypes.shape({
      url: PropTypes.string,
      text: PropTypes.string
    })
  })
};

export default ShippingInfo;
