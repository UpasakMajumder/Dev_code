import React, { Component } from 'react';
import { connect } from 'react-redux';
import DeliveryAddress from './DeliveryAddress';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Total from './Total';
import { getUI, changeShoppingData, sendData } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { shoppingCart } = this.props;
    const { ui, checkedData, isSending, validation } = shoppingCart;

    const content = <div>
      <div className="shopping-cart__block">
        <DeliveryAddress
          validationField={validation.field}
          validationMessage={ui.validationMessage}
          changeShoppingData={this.props.changeShoppingData}
          checkedId={checkedData.deliveryAddress}
          ui={ui.deliveryAddresses} />
      </div>

      <div className="shopping-cart__block">
        <DeliveryMethod
          validationField={validation.field}
          validationMessage={ui.validationMessage}
          changeShoppingData={this.props.changeShoppingData}
          checkedId={checkedData.deliveryMethod}
          ui={ui.deliveryMethod} />
      </div>

      <div className="shopping-cart__block">
        <PaymentMethod
          validationField={validation.field}
          validationMessage={ui.validationMessage}
          changeShoppingData={this.props.changeShoppingData}
          checkedObj={checkedData.paymentMethod}
          ui={ui.paymentMethod} />
      </div>

      <div className="shopping-cart__block">
        <Total ui={ui.totals} />
      </div>

      <div className="shopping-cart__block text--right">
        <button onClick={() => { this.props.sendData(checkedData); }}
                type="button"
                className="btn-action"
                disabled={isSending}>
          {ui.submitLabel}
        </button>
      </div>
    </div>;

    return Object.keys(ui).length ? content : null;
  }
}

export default connect((state) => {
  const { shoppingCart } = state;

  const { status, redirectURL } = shoppingCart.sendData;
  if (status) location.assign(redirectURL);

  return { shoppingCart };
}, {
  getUI,
  changeShoppingData,
  sendData
})(ShoppingCart);
