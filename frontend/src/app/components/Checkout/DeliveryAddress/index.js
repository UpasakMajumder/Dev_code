import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Address from './Address';
import Alert from '../../Alert';

const DeliveryAddress = (props) => {
  const { ui, checkedId, changeShoppingData } = props;
  const { title, description, newAddress, items, emptyMessage } = ui;

  const renderAddresses = (item) => {
    return (
      <div key={`da-${item.id}`} className="input__wrapper">
        <Address changeShoppingData={changeShoppingData} checkedId={checkedId} {...item} />
      </div>
    );
  };

  const addresses = items.map(renderAddresses);

  const alert = items.length ? null : <Alert type="grey" text={emptyMessage} />;

  return (
    <div>
      <div>
        <h2>{title}</h2>
        <div className="cart-fill__block">
          <p className="cart-fill__info">{description}</p>
          {alert}
          <div className="cart-fill__block-inner cart-fill__block--flex">
            {addresses}
            <div className="btn-group btn-grout--left">
              <a
                href={newAddress.url}
                data-dialog="#cart-add-adress"
                className="btn-action btn-action--secondary js-dialog">
                {newAddress.label}
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

DeliveryAddress.propTypes = {
  changeShoppingData: PropTypes.func.isRequired,
  checkedId: PropTypes.number,
  ui: PropTypes.shape({
    items: PropTypes.arrayOf(PropTypes.object.isRequired),
    newAddress: PropTypes.shape({
      label: PropTypes.string.isRequired,
      url: PropTypes.string.isRequired
    }).isRequired,
    isDeliverable: PropTypes.bool.isRequired,
    description: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    unDeliverableText: PropTypes.string,
    emptyMessage: PropTypes.string
  }).isRequired
};

export default DeliveryAddress;
