import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import timeFormat from 'app.helpers/time';

const Order = ({
  image,
  template,
  mailingList,
  shippingDate,
  trackingId,
  price,
  quantityPrefix,
  quantity,
  downloadPdfURL,
  quantityShippedPrefix,
  quantityShipped,
  mailingListPrefix,
  shippingDatePrefix,
  trackingIdPrefix,
  templatePrefix,
  productStatusPrefix,
  productStatus,
  options
}) => {
  const downloadPdfLink = downloadPdfURL
    ? <div className="cart-product__file">
      <a className="link" href={downloadPdfURL}>Download PDF</a>
    </div>
    : null;

  const mailingListElement = mailingList
    ? <div className="cart-product__mlist">
      <p>
        <SVG name="mailing-list"/>
        <span>{mailingListPrefix}: <strong>{mailingList}</strong></span>
      </p>
    </div>
    : null;

  const trackingElement = trackingId
    ? <div className="cart-product__tracking">
      <p>
        <SVG name="location"/>
        <span>{shippingDatePrefix}: <strong>{trackingId}</strong></span>
      </p>
    </div>
    : null;

  const shippingElement = shippingDate
    ? <div className="cart-product__tracking">
      <p>
        <SVG name="courier"/>
        <span>{trackingIdPrefix}: <strong>{timeFormat(shippingDate)}</strong></span>
      </p>
    </div>
    : null;

  const shippingElementFixed = shippingDate ? <div> </div> : null; // Keep flex

  const quantityElement = mailingList
    ? (
      <div className="cart-product__optional">
        <p>{quantityPrefix} {quantity}</p>
      </div>
    )
    : (
      <div className="cart-product__optional">
        <p>{quantityPrefix} {quantity}</p>
        <p>{quantityShippedPrefix} {quantityShipped}</p>
      </div>
    );

  const productStatusInfo = productStatus ? <p>{productStatusPrefix} {productStatus}</p> : <p></p>;

  const optionsElement = options.length
    ? <div className="mr-3">{ options.map((option, i) => <p key={i}>{option.name}: {option.value}</p>) }</div>
    : null;

  return (
    <div className="cart-product">
      <div className="cart-product__img">
        <img src={image} alt={template} />
      </div>

      <div className="cart-product__content">
        <div className="cart-product__template">
          <p>
            <SVG name="products"/>
            <span>{templatePrefix}: <strong>{template}</strong></span>
          </p>
        </div>

        {mailingListElement}
        {trackingElement}
        {shippingElement}
        {shippingElementFixed}

      </div>

      <div className="cart-product__options cart-product__options--center">
        {optionsElement}
        <div>
          <div className="cart-product__price">
            <span>{price}</span>
          </div>
          {productStatusInfo}
          {quantityElement}
          {downloadPdfLink}
        </div>
      </div>
    </div>
  );
};

Order.propTypes = {
  quantityPrefix: PropTypes.string.isRequired,
  template: PropTypes.string.isRequired,
  quantity: PropTypes.number.isRequired,
  image: PropTypes.string.isRequired,
  price: PropTypes.string.isRequired,
  downloadPdfURL: PropTypes.string,
  shippingDate: PropTypes.string,
  mailingList: PropTypes.string,
  trackingId: PropTypes.string,
  mailingListPrefix: PropTypes.string.isRequired,
  shippingDatePrefix: PropTypes.string.isRequired,
  trackingIdPrefix: PropTypes.string.isRequired,
  templatePrefix: PropTypes.string.isRequired,
  productStatusPrefix: PropTypes.string.isRequired,
  productStatus: PropTypes.string.isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired
};

export default Order;
