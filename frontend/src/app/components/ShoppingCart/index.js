import React, { Component } from 'react';
import { connect } from 'react-redux';
import DeliveryAddress from './DeliveryAddress';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Total from './Total';
import { getUI, changeShoppingData, sendData, initCheckedShoppingData } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  componentWillReceiveProps(nextProps) {
    const { ui } = nextProps.shoppingCart;

    if (!Object.keys(ui).length || Object.keys(this.props.shoppingCart.ui).length) return;

    let deliveryAddress = 0;
    let deliveryMethod = 0;
    let paymentMethod = {
      id: 0,
      invoice: ''
    };

    ui.deliveryAddresses.items.forEach((address) => {
      if (address.checked) deliveryAddress = address.id;
    });

    ui.deliveryMethods.items.forEach((methodGroup) => {
      methodGroup.items.forEach((method) => {
        if (method.checked && !deliveryMethod) deliveryMethod = method.id;
      });
    });

    ui.paymentMethods.items.forEach((method) => {
      if (method.checked) paymentMethod = { id: method.id, invoice: '' };
    });

    this.props.initCheckedShoppingData({
      deliveryAddress, deliveryMethod, paymentMethod
    });
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
          ui={ui.deliveryMethods} />
      </div>

      <div className="shopping-cart__block">
        <PaymentMethod
          validationField={validation.field}
          validationMessage={ui.validationMessage}
          changeShoppingData={this.props.changeShoppingData}
          checkedObj={checkedData.paymentMethod}
          ui={ui.paymentMethods} />
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

  const { redirectURL } = shoppingCart.sendData;
  if (redirectURL) location.assign(redirectURL);

  return { shoppingCart };
}, {
  getUI,
  initCheckedShoppingData,
  changeShoppingData,
  sendData
})(ShoppingCart);
