import React, { Component } from 'react';
import { connect } from 'react-redux';
import DeliveryAddress from './DeliveryAddress';
import Alert from '../Alert';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Products from './Products';
import Total from './Total';
import Spinner from '../Spinner';
import Button from '../Button';
import { getUI, changeShoppingData, sendData, initCheckedShoppingData, removeProduct, changeProductQuantity } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  static fireNotification(fields) {
    if (fields.length === 1 && fields.includes('invoice')) return;

    let message = 'Please, select one of ';

    fields.forEach((field, index) => {
      if (field !== 'invoice') {
        if (index === fields.length - 1) {
          message += ' and ';
        } else if (index) {
          message += ', ';
        }

        switch (field) {
        case 'deliveryMethod':
          message += 'the delivery methods';
          break;
        case 'paymentMethod':
          message += 'the payment methods';
          break;
        case 'deliveryAddress':
          message += 'the delivery addresses';
          break;
        default:
          break;
        }
      }
    });

    alert(message); // eslint-disable-line no-alert
  }

  componentDidMount() {
    this.props.getUI();
  }

  componentWillReceiveProps(nextProps) {
    const { ui, validation, isWaitingPDF } = nextProps.shoppingCart;

    if (validation.fields.length) ShoppingCart.fireNotification(validation.fields);

    if (!isWaitingPDF && !ui.submit.isDisabled) {
      setTimeout(() => {
        // this.props.askReadyPdf();
      }, 1000);
    }

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
    const { ui, checkedData, isSending, validation, isWaitingPDF } = shoppingCart;
    const { submit } = ui;

    let content = <Spinner />;

    if (Object.keys(ui).length) {
      const submitIsUnavailable = submit.isDisabled;
      const submitDisabledText = submitIsUnavailable ? <Alert type="info" text={submit.disabledText}/> : null;

      const { isDeliverable, unDeliverableText, title } = ui.deliveryAddresses;

      const deliveryContent = isDeliverable
        ? <div>
          <div className="shopping-cart__block">
            <DeliveryAddress
              validationMessage={ui.validationMessage}
              changeShoppingData={this.props.changeShoppingData}
              checkedId={checkedData.deliveryAddress}
              ui={ui.deliveryAddresses}/>
          </div>

          <div className="shopping-cart__block">
            <DeliveryMethod
              validationMessage={ui.validationMessage}
              changeShoppingData={this.props.changeShoppingData}
              checkedId={checkedData.deliveryMethod}
              isSending={isSending}
              ui={ui.deliveryMethods}/>
          </div>
        </div>
        : <div className="shopping-cart__block">
          <h2>{title}</h2>
          <Alert type="grey" text={unDeliverableText}/>
        </div>;

      content = <div>
        <div className="shopping-cart__block">
          <Products
            removeProduct={this.props.removeProduct}
            changeProductQuantity={this.props.changeProductQuantity}
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

        {submitDisabledText}

        <div className="shopping-cart__block text--right">
          <Button text={submit.btnLabel}
                  isLoading={submitIsUnavailable || isWaitingPDF}
                  type="action"
                  onClick={() => { this.props.sendData(checkedData); }} />
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
