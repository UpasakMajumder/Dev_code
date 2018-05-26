import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import timeFormat from 'app.helpers/time';

const getPreviewLink = (preview) => {
  if (!preview) return null;
  if (!preview.exists) return null;
  return (
    <div className="cart-product__file">
      <a className="link" target="_blank" href={preview.url}>{preview.text}</a>
    </div>
  );
};

const getEmailProofLink = (emailProof, toogleEmailProof) => {
  if (!emailProof) return null;
  if (!emailProof.exists) return null;

  const onClick = (e) => {
    e.preventDefault();
    toogleEmailProof(true, emailProof.url);
  };

  return (
    <div className="cart-product__file">
      <a onClick={onClick} className="link" href="#">{emailProof.text}</a>
    </div>
  );
};

const Order = ({
  image,
  template,
  mailingList,
  shippingDate,
  price,
  quantityPrefix,
  quantity,
  downloadPdfURL,
  quantityShippedPrefix,
  quantityShipped,
  mailingListPrefix,
  shippingDatePrefix,
  trackingPrefix,
  tracking,
  templatePrefix,
  productStatusPrefix,
  productStatus,
  options,
  preview,
  unitOfMeasure,
  // emailProof
  emailProof,
  toogleEmailProof
}) => {
  const downloadPdfLink = downloadPdfURL
    ? (
      <div className="cart-product__file">
        <a className="link" target="_blank" href={downloadPdfURL}>Download PDF</a>
      </div>
    ) : null;

  const mailingListElement = mailingList
    ? <div className="cart-product__mlist">
      <p>
        <SVG name="mailing-list"/>
        <span>{mailingListPrefix}: <strong>{mailingList}</strong></span>
      </p>
    </div>
    : null;

  const getTrackingElement = () => {
    if (!tracking || !tracking.length) return null;


    const tracks = tracking.map((track, index) => {
      const prefix = index === 0 ? ' ' : ', '
      let id;
      if (track.url) {
        id = <a target="_blank" href={track.url} className="link" >{track.id}</a>;
      } else {
        id = track.id;
      }

      return <strong key={index}>{prefix}{id}</strong>;
    });


    return (
      <div className="cart-product__tracking">
        <SVG name="location"/>
        <span>{trackingPrefix}: {tracks}</span>
      </div>
    );
  };

  const shippingElement = shippingDate
    ? <div className="cart-product__tracking">
      <p>
        <SVG name="courier"/>
        <span>{shippingDatePrefix}: <strong>{timeFormat(shippingDate)}</strong></span>
      </p>
    </div>
    : null;

  const shippingElementFixed = shippingDate ? <div> </div> : null; // Keep flex

  const quantityElement = mailingList
    ? (
      <div className="cart-product__optional">
        <p>{quantityPrefix} {quantity} {unitOfMeasure}</p>
      </div>
    )
    : (
      <div className="cart-product__optional">
        <p>{quantityPrefix} {quantity} {unitOfMeasure}</p>
        <p>{quantityShippedPrefix} {quantityShipped} {unitOfMeasure}</p>
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
        {getTrackingElement()}
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
          {getPreviewLink(preview)}
          {getEmailProofLink(emailProof, toogleEmailProof)}
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
  trackingPrefix: PropTypes.string.isRequired,
  tracking: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string,
    url: PropTypes.string
  })),
  mailingListPrefix: PropTypes.string.isRequired,
  shippingDatePrefix: PropTypes.string.isRequired,
  templatePrefix: PropTypes.string.isRequired,
  productStatusPrefix: PropTypes.string.isRequired,
  productStatus: PropTypes.string.isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  unitOfMeasure: PropTypes.string.isRequired,
  emailProof: PropTypes.object.isRequired,
  toogleEmailProof: PropTypes.func.isRequired
};

export default Order;
