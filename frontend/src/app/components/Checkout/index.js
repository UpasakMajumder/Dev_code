import React, { Component } from 'react';
import { connect } from 'react-redux';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import Spinner from 'app.dump/Spinner';
/* ac */
import { changeShoppingData, sendData, initCheckedShoppingData, removeProduct,
  changeProductQuantity, getUI } from 'app.ac/checkout';
/* local components */
import DeliveryAddress from './DeliveryAddress';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Products from './Products';
import Total from './Total';

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
    const invalidFields = Object.keys(checkedData).filter(key => checkedData[key] === 0);

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

    if (deliveryAddresses.isDeliverable) {
      deliveryAddresses.items.forEach((address) => {
        if (address.checked) deliveryAddress = address.id;
      });


      if (deliveryMethods) {
        deliveryMethods.items.forEach((methodGroup) => {
          methodGroup.items.forEach((method) => {
            if (method.checked && !deliveryMethod) deliveryMethod = method.id;
          });
        });
      }
    } else {
      deliveryAddress = 'non-deliverable';
      deliveryMethod = 'non-deliverable';
    }

    paymentMethods.items.forEach((method) => {
      if (method.checked) paymentMethod = { id: method.id, invoice: '' };
    });

    initCheckedShoppingData({
      deliveryAddress,
      deliveryMethod,
      paymentMethod
    });
  };

  componentWillReceiveProps(nextProps) {
    const { ui: uiNext } = nextProps.checkout;
    const { ui: uiCurr } = this.props.checkout;

    const { products } = uiNext;
    if (!products.items.length) location.reload();

    if (uiNext !== uiCurr) this.initCheckedShoppingData(uiNext);
  }

  render() {
    const { checkout, changeShoppingData, changeProductQuantity, removeProduct } = this.props;
    const { ui, checkedData, isSending } = checkout;

    let content = <Spinner />;

    if (Object.keys(ui).length) {
      const { submit, deliveryAddresses, deliveryMethods, products, paymentMethods, totals, validationMessage } = ui;
      const { paymentMethod, deliveryMethod, deliveryAddress } = checkedData;

      const disableInteractivity = !totals;

      const { isDeliverable, unDeliverableText, title } = deliveryAddresses;

      const deliveryMethodComponent = disableInteractivity
        ? <Spinner/>
        : <DeliveryMethod
          changeShoppingData={changeShoppingData}
          checkedId={deliveryMethod}
          isSending={isSending}
          ui={deliveryMethods}
        />;

      const totalsComponent = disableInteractivity
        ? <Spinner/>
        : <Total ui={totals}/>;

      const deliveryContent = isDeliverable
        ? <div>
          <div className="shopping-cart__block">
            <DeliveryAddress
              changeShoppingData={changeShoppingData}
              checkedId={deliveryAddress}
              disableInteractivity={disableInteractivity}
              ui={deliveryAddresses}/>
          </div>

          <div className="shopping-cart__block">
            {deliveryMethodComponent}
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
            disableInteractivity={disableInteractivity}
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
          {totalsComponent}
        </div>

        <div className="shopping-cart__block text--right">
          <Button text={submit.btnLabel}
                  type="action"
                  isLoading={disableInteractivity}
                  onClick={() => this.sendData(checkedData)}
          />
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
  changeProductQuantity
})(Checkout);
