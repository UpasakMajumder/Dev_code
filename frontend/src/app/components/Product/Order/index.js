import React from 'react';
import SVG from '../../SVG';

export default (props) => {
  const { image, template, mailingList, shippingDate, trackingId,
    price, quantityPrefix, quantity, downloadPdfURL } = props;

  const downloadPdfLink = downloadPdfURL
    ? <div className="cart-product__file">
      <a className="link" href={downloadPdfURL}>Download PDF</a>
    </div>
    : null;

  const mailingListElement = mailingList
    ? <div className="cart-product__mlist">
      <p>
        <SVG name="mailing-list"/>
        <span>Mailing list: <strong>{mailingList}</strong></span>
      </p>
    </div>
    : null;

  const trackingElement = trackingId
    ? <div className="cart-product__tracking">
      <p>
        <SVG name="location"/>
        <span>Tracking ID: <strong>{trackingId}</strong></span>
      </p>
    </div>
    : null;

  const shippingElement = shippingDate
    ? <div className="cart-product__tracking">
      <p>
        <SVG name="courier"/>
        <span>Shipping data: <strong>{shippingDate}</strong></span>
      </p>
    </div>
    : null;

  const shippingElementFixed = shippingDate ? <div> </div> : null; // Keep flex

  return (
    <div className="cart-product">
      <div className="cart-product__img">
        <img src={image} alt={template} />
      </div>

      <div className="cart-product__content">
        <div className="cart-product__template">
          <p>
            <SVG name="products"/>
            <span>Template: <strong>{template}</strong></span>
          </p>
        </div>

        {mailingListElement}
        {trackingElement}
        {shippingElement}
        {shippingElementFixed}

      </div>

      <div className="cart-product__options">
        <div className="cart-product__price">
          <span>{price}</span>
        </div>
        <div className="cart-product__optional">
          <span>{quantityPrefix} {quantity}</span>
        </div>
        {downloadPdfLink}
      </div>
    </div>
  );
};
