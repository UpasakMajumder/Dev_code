import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* component */
import Button from 'app.dump/Button';
import Input from 'app.dump/Form/TextInput';

const Proceed = ({
  addToCart,
  openTemplate,
  handleChangeQuantity,
  quantity,
  proceedProduct
}) => {
  if (!addToCart || !openTemplate) return null;

  if (addToCart) {
    return (
      <div className="product-view__proceed">
        <Input
          min={addToCart.get('minQuantity')}
          max={addToCart.get('maxQuantity')}
          type="number"
          value={quantity}
          onChange={e => handleChangeQuantity(e.target.value)}
        />
        <span className="mx-2">{addToCart.get('unit')}</span>
        <Button
          type="action"
          isLoading={false}
          text={addToCart.get('text')}
          onClick={proceedProduct}
        />
      </div>
    );
  }

  return (
    <div className="product-view__proceed">
      <a
        className="btn-action btn-action"
        href={openTemplate.get('url')}
      >
        {openTemplate.get('text')}
      </a>
    </div>
  );
};

Proceed.propTypes = {
  addToCart: ImmutablePropTypes.mapContains({
    text: PropTypes.string.isRequired,
    unit: PropTypes.string.isRequired
  }),
  openTemplate: ImmutablePropTypes.mapContains({
    url: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired
  }),
  handleChangeQuantity: PropTypes.func.isRequired,
  proceedProduct: PropTypes.func.isRequired,
  quantity: PropTypes.oneOfType([PropTypes.number, PropTypes.string])
};

export default Proceed;
