import React, { Component } from 'react';
import { connect } from 'react-redux';
import { toastr } from 'react-redux-toastr';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import Spinner from 'app.dump/Spinner';
/* ac */
import { changeShoppingData, sendData, initCheckedShoppingData, removeProduct,
  changeProductQuantity, getUI, addNewAddress } from 'app.ac/checkout';
import { changeProducts } from 'app.ac/cartPreview';
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

    toastr.error(message);
  }

  componentDidMount() {
    const { getUI } = this.props;
    getUI();
  }

  placeOrder = (checkedData) => {
    const { sendData, checkout } = this.props;
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
      const data = { ...checkedData };
      if (checkedData.deliveryAddress === 'non-deliverable') data.deliveryAddress = 0;
      if (checkedData.deliveryMethod === 'non-deliverable') data.deliveryMethod = 0;
      if (checkedData.deliveryAddress === -1) {
        data.deliveryAddress = checkout.newAddress;
      } else {
        data.deliveryAddress = { id: data.deliveryAddress };
      }

      sendData(data);
    }
  };

  initCheckedShoppingData = (ui) => {
    const { deliveryAddresses, deliveryMethods, paymentMethods } = ui;
    const { initCheckedShoppingData } = this.props;

    let deliveryAddress = 0;
    let deliveryMethod = 0;
    let paymentMethod = {
      id: 0
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
      if (method.checked) paymentMethod = { id: method.id };
    });

    initCheckedShoppingData({
      deliveryAddress,
      deliveryMethod,
      paymentMethod
    });
  };

  refreshCartPreview = (products) => {
    const { items, summaryPrice } = products;
    this.props.changeProducts(items, summaryPrice);
  };

  componentWillReceiveProps(nextProps) {
    const { ui: uiNext } = nextProps.checkout;
    const { ui: uiCurr } = this.props.checkout;

    if (uiNext !== uiCurr) this.initCheckedShoppingData(uiNext);
    if (uiNext.products !== uiCurr.products) this.refreshCartPreview(uiNext.products);
  }

  render() {
    const { checkout, changeShoppingData, changeProductQuantity, removeProduct, addNewAddress } = this.props;
    const { ui, checkedData, isSending, addedDataId, newAddress } = checkout;

    let content = <Spinner />;

    if (Object.keys(ui).length) {
      const { emptyCart, submit, deliveryAddresses, deliveryMethods, products, paymentMethods, totals, validationMessage } = ui;
      const { paymentMethod, deliveryMethod, deliveryAddress } = checkedData;

      // cart is empty
      if (!ui.products.items.length) {
        content = <div>
          <div className="alert alert--full alert--info isOpen js-collapse">
            <p className="p-info weight--normal">{emptyCart.text}</p>
          </div>

          <div className="btn-group btn-group--center">
            <a href={emptyCart.dashboardButtonUrl} type="type" className="btn-action btn-action--secondary">{emptyCart.dashboardButtonText}</a>
            <a href={emptyCart.productsButtonUrl} type="type" className="btn-action">{emptyCart.productsButtonText}</a>
          </div>
        </div>;

        return content;
      }

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
              addNewAddress={addNewAddress}
              addedDataId={addedDataId}
              ui={deliveryAddresses}
              newAddressObject={newAddress}
            />
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
            ui={paymentMethods}
          />
        </div>

        <div className="shopping-cart__block">
          {totalsComponent}
        </div>

        <div className="shopping-cart__block text--right">
          <Button
            text={submit.btnLabel}
            type="action"
            isLoading={disableInteractivity}
            onClick={() => this.placeOrder(checkedData)}
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
  changeProductQuantity,
  changeProducts,
  addNewAddress
})(Checkout);
