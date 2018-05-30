import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Immutable from 'immutable';
import ImmutablePropTypes from 'react-immutable-proptypes';
/* component */
import Button from 'app.dump/Button';
import Input from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';

const Proceed = ({
  addToCart,
  openTemplate,
  handleChangeQuantity,
  quantity,
  proceedProduct,
  isLoading,
  quanityError
}) => {
  if (!addToCart && !openTemplate) return null;

  const getInput = () => {
    const items = addToCart.getIn(['tiers', 'items']);
    if (!items || !items.count()) {
      return [
        <Input
          key={0}
          min={addToCart.get('minQuantity')}
          max={addToCart.get('maxQuantity')}
          type="number"
          value={quantity}
          onChange={e => handleChangeQuantity(e.target.value)}
          className="product-view__proceed-input"
          error={quanityError ? addToCart.get('quantityErrorText') : ''}
        />,
        <span key={1} className="mx-2">{addToCart.get('unit')}</span>
      ];
    }

    const options = [
      <option
        key={0}
        disabled
        value=""
      >
        {addToCart.getIn(['tiers', 'placeholder'])}
      </option>
    ];

    items.forEach((item) => {
      options.push(
        <option
          key={item}
          value={item}
        >
          {item}
        </option>
      );
    });

    const errorElement = quanityError ? <span className="input__error input__error--noborder">{addToCart.get('quantityErrorText')}</span> : null;

    return (
      <div className="input__wrapper product-options__input mr-3">
        <div className={`input__select ${quanityError ? 'input--error' : ''}`}>
          <select
            value={quantity}
            onChange={e => handleChangeQuantity(e.target.value)}
          >
            {options}
          </select>
        </div>
        {errorElement}
      </div>
    );
  };

  if (addToCart) {
    return (
      <div className="product-view__proceed">
        {getInput()}
        <Button
          type="action"
          isLoading={false}
          text={addToCart.get('text')}
          onClick={proceedProduct}
          isLoading={isLoading}
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
    unit: PropTypes.string.isRequired,
    quantityErrorText: PropTypes.string.isRequired,
    tiers: ImmutablePropTypes.mapContains({
      placeholder: PropTypes.string.isRequired,
      items: ImmutablePropTypes.listOf(PropTypes.number)
    })
  }),
  openTemplate: ImmutablePropTypes.mapContains({
    url: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired
  }),
  handleChangeQuantity: PropTypes.func.isRequired,
  proceedProduct: PropTypes.func.isRequired,
  quantity: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  isLoading: PropTypes.bool.isRequired,
  quanityError: PropTypes.bool.isRequired
};

export default Proceed;
