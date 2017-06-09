import React, { Component } from 'react';
import { connect } from 'react-redux';
import DeliveryAddress from './DeliveryAddress';
import Undeliverable from './DeliveryAddress/Undeliverable';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Products from './Products';
import Total from './Total';
import { getUI, changeShoppingData, sendData, initCheckedShoppingData, removeProduct, changeProductQuantity } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  componentWillReceiveProps(nextProps) {
    const { ui } = nextProps.shoppingCart;

    if (ui === this.props.shoppingCart.ui) return;

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
    const { ui, checkedData, isSending, validation, loadingProducts, loadingQuantities } = shoppingCart;

    let content = null;

    if (Object.keys(ui).length) {
      const { isDeliverable, unDeliverableText, title } = ui.deliveryAddresses;

      const deliveryContent = isDeliverable
        ? <div>
          <div className="shopping-cart__block">
            <DeliveryAddress
              validationFields={validation.fields}
              validationMessage={ui.validationMessage}
              changeShoppingData={this.props.changeShoppingData}
              checkedId={checkedData.deliveryAddress}
              ui={ui.deliveryAddresses}/>
          </div>

          <div className="shopping-cart__block">
            <DeliveryMethod
              validationFields={validation.fields}
              validationMessage={ui.validationMessage}
              changeShoppingData={this.props.changeShoppingData}
              checkedId={checkedData.deliveryMethod}
              isSending={isSending}
              ui={ui.deliveryMethods}/>
          </div>
        </div>
        : <div className="shopping-cart__block">
          <h2>{title}</h2>
          <Undeliverable unDeliverableText={unDeliverableText}/>
        </div>;

      content = <div>
        <div className="shopping-cart__block">
          <Products
            removeProduct={this.props.removeProduct}
            loadingProducts={loadingProducts}
            changeProductQuantity={this.props.changeProductQuantity}
            loadingQuantities={loadingQuantities}
            ui={ui.products}
          />
        </div>

        {deliveryContent}

        <div className="shopping-cart__block">
          <PaymentMethod
            validationFields={validation.fields}
            validationMessage={ui.validationMessage}
            changeShoppingData={this.props.changeShoppingData}
            checkedObj={checkedData.paymentMethod}
            ui={ui.paymentMethods}/>
        </div>

        <div className="shopping-cart__block">
          <Total ui={ui.totals}/>
        </div>

        <div className="shopping-cart__block text--right">
          <button onClick={() => {
            this.props.sendData(checkedData);
          }}
                  type="button"
                  className="btn-action"
                  disabled={isSending}>
            {ui.submitLabel}
          </button>
        </div>
      </div>;
    }

    return content;
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
  sendData,
  removeProduct,
  changeProductQuantity
})(ShoppingCart);
