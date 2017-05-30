import React, { Component } from 'react';
import { connect } from 'react-redux';
import DeliveryAddress from './DeliveryAddress';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Total from './Total';
import { getUI, changeShoppingData, initCheckedShoppingData, sendData } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  componentDidMount() {
    this.props.getUI();
    this.props.initCheckedShoppingData();
  }

  render() {
    const { shoppingCart } = this.props;
    const { ui, checkedData } = shoppingCart;

    const content = <div>
      <div className="shopping-cart__block">
        <DeliveryAddress
          changeShoppingData={this.props.changeShoppingData}
          checkedId={checkedData.deliveryAddress}
          ui={ui.deliveryAddresses} />
      </div>

      <div className="shopping-cart__block">
        <DeliveryMethod
          changeShoppingData={this.props.changeShoppingData}
          checkedId={checkedData.deliveryMethod}
          ui={ui.deliveryMethod} />
      </div>

      <div className="shopping-cart__block">
        <PaymentMethod
          changeShoppingData={this.props.changeShoppingData}
          checkedObj={checkedData.paymentMethod}
          ui={ui.paymentMethod} />
      </div>

      <div className="shopping-cart__block">
        <Total ui={ui.totals} />
      </div>

      <div className="shopping-cart__block text--right">
        <button onClick={() => { this.props.sendData(); }} type="button" className="btn-action">{ui.submitLabel}</button>
      </div>
    </div>;

    return Object.keys(ui).length ? content : null;
  }
}

export default connect((state) => {
  const { shoppingCart } = state;

  const { status, redirectUrl } = shoppingCart.sendData;
  if (status) location.assign(redirectUrl);

  return { shoppingCart };
}, {
  getUI,
  changeShoppingData,
  initCheckedShoppingData,
  sendData
})(ShoppingCart);
