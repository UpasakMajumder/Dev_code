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
import { getUI, checkPDFAvailability, changeShoppingData, sendData, initCheckedShoppingData, removeProduct, changeProductQuantity } from '../../AC/shoppingCart';

class ShoppingCart extends Component {
  constructor() {
    super();

    this.sendData = this.sendData.bind(this);
    this.initCheckedShoppingData = this.initCheckedShoppingData.bind(this);
    this.checkPDFAvailability = this.checkPDFAvailability.bind(this);
  }

  static fireNotification(fields) {
    let message = 'Please, select one of ';

    fields.forEach((field, index) => {
      if (index === fields.length - 1 && index !== 0) {
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
      case 'invoice':
        message += 'the PO number';
        break;
      default:
        break;
      }
    });

    alert(message); // eslint-disable-line no-alert
  }

  componentDidMount() {
    this.props.getUI();
  }

  sendData(checkedData) {
    const invalidFields = Object.keys(checkedData).filter(key => !checkedData[key]);

    if (!checkedData.paymentMethod.id) invalidFields.push('paymentMethod');

    if (checkedData.paymentMethod.id === 3) {
      if (!checkedData.paymentMethod.invoice) {
        invalidFields.push('invoice');
      }
    }

    if (invalidFields.length) {
      ShoppingCart.fireNotification(invalidFields);
    } else {
      this.props.sendData(checkedData);
    }
  }

  initCheckedShoppingData(ui) {
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

  checkPDFAvailability(nextProps) {
    const { isWaitingPDF, isAskingPDF } = nextProps.shoppingCart;
    if (isWaitingPDF && !isAskingPDF) this.props.checkPDFAvailability();
  }

  componentWillReceiveProps(nextProps) {
    const { ui: uiNext } = nextProps.shoppingCart;
    const { ui: uiCurr } = this.props.shoppingCart;

    const { products } = uiNext;
    if (!products.items.length) location.reload();

    if (uiNext !== uiCurr) this.initCheckedShoppingData(uiNext);
    this.checkPDFAvailability(nextProps);
  }

  render() {
    const { shoppingCart } = this.props;
    const { ui, checkedData, isSending, isWaitingPDF } = shoppingCart;
    const { submit } = ui;

    let content = <Spinner />;

    if (Object.keys(ui).length) {
      const submitDisabledText = isWaitingPDF ? <Alert type="info" text={submit.disabledText}/> : null;

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
                  isLoading={isWaitingPDF}
                  type="action"
                  onClick={() => this.sendData(checkedData)} />
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
  changeProductQuantity,
  checkPDFAvailability
})(ShoppingCart);
