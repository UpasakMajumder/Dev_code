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
import { getUI, checkPDFAvailability, changeShoppingData, sendData, initCheckedShoppingData, removeProduct,
  changeProductQuantity } from '../../AC/checkout';

class Checkout extends Component {
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
    const { getUI } = this.props;
    getUI();
  }

  sendData = (checkedData) => {
    const { sendData } = this.props;
    const invalidFields = Object.keys(checkedData).filter(key => !checkedData[key]);

    if (!checkedData.paymentMethod.id) invalidFields.push('paymentMethod');

    if (checkedData.paymentMethod.id === 3) {
      if (!checkedData.paymentMethod.invoice) {
        invalidFields.push('invoice');
      }
    }

    if (invalidFields.length) {
      Checkout.fireNotification(invalidFields);
    } else {
      sendData(checkedData);
    }
  };

  initCheckedShoppingData = (ui) => {
    const { deliveryAddresses, deliveryMethods, paymentMethods } = ui;
    const { initCheckedShoppingData } = this.props;

    let deliveryAddress = 0;
    let deliveryMethod = 0;
    let paymentMethod = {
      id: 0,
      invoice: ''
    };

    deliveryAddresses.items.forEach((address) => {
      if (address.checked) deliveryAddress = address.id;
    });

    deliveryMethods.items.forEach((methodGroup) => {
      methodGroup.items.forEach((method) => {
        if (method.checked && !deliveryMethod) deliveryMethod = method.id;
      });
    });

    paymentMethods.items.forEach((method) => {
      if (method.checked) paymentMethod = { id: method.id, invoice: '' };
    });

    initCheckedShoppingData({
      deliveryAddress,
      deliveryMethod,
      paymentMethod
    });
  };

  checkPDFAvailability = (nextProps) => {
    const { isWaitingPDF, isAskingPDF } = nextProps.checkout;
    const { checkPDFAvailability } = this.props;
    if (isWaitingPDF && !isAskingPDF) checkPDFAvailability();
  };

  componentWillReceiveProps(nextProps) {
    const { ui: uiNext } = nextProps.checkout;
    const { ui: uiCurr } = this.props.checkout;
    if (uiNext !== uiCurr) this.initCheckedShoppingData(uiNext);
    this.checkPDFAvailability(nextProps);
  }

  render() {
    const { checkout, changeShoppingData, changeProductQuantity, removeProduct } = this.props;
    const { ui, checkedData, isSending, isWaitingPDF } = checkout;

    let content = <Spinner />;

    if (Object.keys(ui).length) {
      const { submit, deliveryAddresses, deliveryMethods, products, paymentMethods, totals, validationMessage } = ui;
      const { paymentMethod, deliveryMethod, deliveryAddress } = checkedData;
      const { disabledText, btnLabel } = submit;

      const submitDisabledText = isWaitingPDF ? <Alert type="info" text={disabledText}/> : null;

      const { isDeliverable, unDeliverableText, title } = deliveryAddresses;

      const deliveryContent = isDeliverable
        ? <div>
          <div className="shopping-cart__block">
            <DeliveryAddress
              changeShoppingData={changeShoppingData}
              checkedId={deliveryAddress}
              ui={deliveryAddresses}/>
          </div>

          <div className="shopping-cart__block">
            <DeliveryMethod
              changeShoppingData={changeShoppingData}
              checkedId={deliveryMethod}
              isSending={isSending}
              ui={deliveryMethods}/>
          </div>
        </div>
        : <div className="shopping-cart__block">
          <h2>{title}</h2>
          <Alert type="grey" text={unDeliverableText}/>
        </div>;

      content = <div>
        <div className="shopping-cart__block">
          <Products
            removeProduct={removeProduct}
            changeProductQuantity={changeProductQuantity}
            ui={products}
          />
        </div>

        {deliveryContent}

        <div className="shopping-cart__block">
          <PaymentMethod
            validationMessage={validationMessage}
            changeShoppingData={changeShoppingData}
            checkedObj={paymentMethod}
            ui={paymentMethods}/>
        </div>

        <div className="shopping-cart__block">
          <Total ui={totals}/>
        </div>

        {submitDisabledText}

        <div className="shopping-cart__block text--right">
          <Button text={btnLabel}
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
  const { checkout } = state;

  const { redirectURL } = checkout.sendData;
  if (redirectURL) location.assign(redirectURL);

  return { checkout };
}, {
  getUI,
  initCheckedShoppingData,
  changeShoppingData,
  sendData,
  removeProduct,
  changeProductQuantity,
  checkPDFAvailability
})(Checkout);
