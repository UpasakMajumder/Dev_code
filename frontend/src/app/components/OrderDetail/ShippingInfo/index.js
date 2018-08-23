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

  const deliveryMethods = deliveryMethod.map(item => <li key={item} className="mr-2"><SVG name={item} /></li>);

  return (
    <div className="order-block order-block--tile">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <ul className="m-0 flex--center--start list--unstyled">{deliveryMethods}</ul>
        <div className="order-block__detail-address">{addressEl}</div>
        {trackingLink}
      </div>
    </div>
  );
};

ShippingInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    deliveryMethod: PropTypes.arrayOf(PropTypes.string).isRequired,
    address: PropTypes.shape({
      customerName: PropTypes.string.isRequired,
      company: PropTypes.string,
      address1: PropTypes.string.isRequired,
      address2: PropTypes.string,
      city: PropTypes.string.isRequired,
      state: PropTypes.string,
      zip: PropTypes.string.isRequired,
      county: PropTypes.string.isRequired,
      phone: PropTypes.string.isRequired,
      email: PropTypes.string.isRequired
    }).isRequired,
    message: PropTypes.string,
    tracking: PropTypes.shape({
      url: PropTypes.string,
      text: PropTypes.string
    })
  })
};

export default ShippingInfo;
