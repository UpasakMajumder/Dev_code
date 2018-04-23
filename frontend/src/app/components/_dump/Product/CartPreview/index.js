import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Image from 'app.dump/DefaultImg';
import SVG from 'app.dump/SVG';

const CartPreviewProduct = (props) => {
  const description = props.isMailingList
    ? (
      <p>
        <SVG name="mailing-list"/>
        <span><strong>{props.mailingList}</strong></span>
      </p>
    )
    : (
      <p>
        {props.quantityPrefix} <strong>{props.quantity}</strong> {props.unitOfMeasure}
      </p>
    );

  return (
    <div className="cart-preview__product">
      <div className="cart-preview__img">
        <Image img={props.image} alt={props.template} />
      </div>
      <div className="cart-preview__description">
        <p>
          <SVG name="products"/>
          <span>{props.templatePrefix}: <strong>{props.template}</strong></span>
        </p>
        {description}
      </div>
      <div className="cart-preview__price">
        <span>{props.pricePrefix} {props.price}</span>
      </div>
    </div>
  );
};

CartPreviewProduct.propTypes = {
  id: PropTypes.number.isRequired,
  image: PropTypes.string,
  template: PropTypes.string.isRequired,
  templatePrefix: PropTypes.string.isRequired,
  isMailingList: PropTypes.bool.isRequired,
  mailingList: PropTypes.string,
  pricePrefix: PropTypes.string.isRequired,
  price: PropTypes.string.isRequired,
  unitOfMeasure: PropTypes.string.isRequired,
  quantityPrefix: PropTypes.string,
  quantity: PropTypes.number
};

export default CartPreviewProduct;
