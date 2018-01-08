import React, { Component } from 'react';
import { connect } from 'react-redux';
import { toastr } from 'react-redux-toastr';
/* globals */
import { CHECKOUT, NOTIFICATION } from 'app.globals';
/* helpers */
import { emailRegExp } from 'app.helpers/regexp';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import Spinner from 'app.dump/Spinner';
import CheckboxInput from 'app.dump/Form/CheckboxInput';
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
import EmailConfirmation from './EmailConfirmation';

class Checkout extends Component {
  constructor() {
    super();
    const defaultId = +new Date();
    this.state = {
      items: [
        {
          id: defaultId
        }
      ],
      fields: {
        [defaultId]: ''
      },
      agreeWithTandC: !CHECKOUT.tAndC.exists
    };
  }

  changeInput = (id, value) => {
    this.setState({
      fields: {
        ...this.state.fields,
        [id]: value
      }
    });
  };

  removeInput = (id) => {
    const items = this.state.items.filter(item => item.id !== id);

    const fields = Object.assign({}, this.state.fields);
    delete fields[id];

    this.setState({
      items,
      fields
    });
  };

  addInput = () => {
    const id = +new Date();

    this.setState({
      items: [...this.state.items, { id }],
      fields: {
        ...this.state.fields,
        [id]: ''
      }
    });
  };

  static orginizeEmailConfirmation(emails) {
    let invalid = false;
    const list = Object.values(emails).filter((email) => {
      if (!email) return false;
      if (email.match(emailRegExp)) return true;
      invalid = true;
      return false;
    });

    return {
      list,
      invalid
    };
  }

  static fireNotification(fields) {
    fields.forEach((field, index) => {
      const { checkoutValidation } = NOTIFICATION;

      if (checkoutValidation) {
        if (checkoutValidation[field]) {
          toastr.error(checkoutValidation[field].title, checkoutValidation[field].text);
        }
      }
    });
  }

  toggleAgreementWithTandC = () => {
    this.setState((prevState) => {
      return {
        agreeWithTandC: !prevState.agreeWithTandC
      };
    });
  };

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

    const newEmailConfirmation = Checkout.orginizeEmailConfirmation(checkedData.emailConfirmation);

    if (newEmailConfirmation.invalid) invalidFields.push('emailConfirmation');

    if (invalidFields.length) {
      Checkout.fireNotification(invalidFields);
    } else {
      const data = { ...checkedData, emailConfirmation: newEmailConfirmation.list };
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

  static getDeliveryMethodComponent = (
    isDeliverable,
    changeShoppingData,
    deliveryMethod,
    isSending,
    deliveryMethods,
    disableInteractivity
  ) => {
    if (!isDeliverable) return null;

    if (!disableInteractivity) {
      return (
        <DeliveryMethod
          changeShoppingData={changeShoppingData}
          checkedId={deliveryMethod}
          isSending={isSending}
          ui={deliveryMethods}
        />
      );
    }

    return (
      <div className="shopping-cart__block">
        <Spinner/>
      </div>
    );
  };

  render() {
    const { checkout, changeShoppingData, changeProductQuantity, removeProduct, addNewAddress } = this.props;
    const { ui, checkedData, isSending, newAddress } = checkout;

    let content = <Spinner />;

    const tAndCComponent = CHECKOUT.tAndC.exists
      ?
      (
        <div className="shopping-cart__block">
          <CheckboxInput
            id="t&c"
            label={CHECKOUT.tAndC.text}
            type="checkbox"
            value={this.state.agreeWithTandC}
            onChange={this.toggleAgreementWithTandC}
          />
        </div>
      )
      : null;

    if (Object.keys(ui).length) {
      const {
        emptyCart,
        submit,
        deliveryAddresses,
        deliveryMethods,
        products,
        paymentMethods,
        totals,
        validationMessage,
        emailConfirmation
      } = ui;
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

      const totalsComponent = disableInteractivity
        ? <Spinner/>
        : <Total ui={totals}/>;

      const deliveryAddressComponent = isDeliverable
        ? (
          <div className="shopping-cart__block">
            <DeliveryAddress
              changeShoppingData={changeShoppingData}
              checkedId={deliveryAddress}
              disableInteractivity={disableInteractivity}
              addNewAddress={addNewAddress}
              ui={deliveryAddresses}
              newAddressObject={newAddress}
            />
          </div>
        ) : (
          <div className="shopping-cart__block">
            <h2>{title}</h2>
            <Alert type="grey" text={unDeliverableText}/>
          </div>
        );

      const emailConfirmationContent = emailConfirmation.exists
        ? (
          <EmailConfirmation
            changeInput={this.changeInput}
            removeInput={this.removeInput}
            addInput={this.addInput}
            items={this.state.items}
            fields={this.state.fields}
            {...emailConfirmation}
          />
        ) : null;

      const welcomeMessage = CHECKOUT.message
        ? (
          <div className="shopping-cart__block">
            <Alert type="grey" text={CHECKOUT.message}/>
          </div>
        ) : null;

      content = (
        <div>
          {welcomeMessage}
          <div className="shopping-cart__block">
            <Products
              removeProduct={removeProduct}
              disableInteractivity={disableInteractivity}
              changeProductQuantity={changeProductQuantity}
              ui={products}
            />
          </div>

          {deliveryAddressComponent}
          {emailConfirmationContent}
          {Checkout.getDeliveryMethodComponent(
            isDeliverable,
            changeShoppingData,
            deliveryMethod,
            isSending,
            deliveryMethods,
            disableInteractivity
          )}

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

          {tAndCComponent}

          <div className="shopping-cart__block text--right">
            <Button
              text={submit.btnLabel}
              type="action"
              disabled={!this.state.agreeWithTandC}
              isLoading={disableInteractivity}
              onClick={() => this.placeOrder({
                ...checkedData,
                agreeWithTandC: CHECKOUT.tAndC.exists && this.state.agreeWithTandC,
                emailConfirmation: this.state.fields
              })}
            />
          </div>
        </div>
      );
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
