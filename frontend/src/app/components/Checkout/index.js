import React, { Component } from 'react';
import { connect } from 'react-redux';
import { toastr } from 'react-redux-toastr';
/* globals */
import { CHECKOUT, NOTIFICATION } from 'app.globals';
/* helpers */
import { emailRegExp } from 'app.helpers/regexp';
import { scrollTo } from 'app.helpers/dom';
/* components */
import Alert from 'app.dump/Alert';
import Button from 'app.dump/Button';
import Spinner from 'app.dump/Spinner';
import CheckboxInput from 'app.dump/Form/CheckboxInput';
/* ac */
import {
  sendData,
  initCheckedShoppingData,
  removeProduct,
  changeProductQuantity,
  getUI,
  addNewAddress,
  changeDeliveryAddress,
  changeDeliveryMethod,
  changePaymentMethod
} from 'app.ac/checkout';
import { changeProducts } from 'app.ac/cartPreview';
import toggleEmailProof from 'app.ac/emailProof';
import { addAddress as saveAddress } from 'app.ac/settingsAddresses';
/* local components */
import DeliveryAddress from './DeliveryAddress';
import DeliveryMethod from './DeliveryMethod';
import PaymentMethod from './PaymentMethod';
import Products from './Products';
import Total from './Total';
import EmailConfirmation from './EmailConfirmation';
import EmailProof from '../EmailProof';

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
      agreeWithTandC: !CHECKOUT.tAndC.exists,
      initChecked: true,
      scrolled: false
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
    this.props.getUI();
  }

  checkPaymentMethod = (checkedData, invalidFields) => {
    const checkedPM = checkedData.paymentMethod;

    if (!checkedPM.id) invalidFields.push('paymentMethod');

    const itemFromProps = this.props.checkout.ui.paymentMethods.items.find(item => item.id === checkedPM.id);

    if (itemFromProps.hasInput && !checkedPM.invoice) invalidFields.push('invoice');

    if (itemFromProps.items.length && checkedPM.card === undefined) invalidFields.push('paymentMethod'); // new card has id = ''
  }

  placeOrder = (checkedData) => {
    const { sendData, checkout } = this.props;
    const invalidFields = Object.keys(checkedData).filter(key => checkedData[key] === 0);

    this.checkPaymentMethod(checkedData, invalidFields);

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
    const paymentMethod = {
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
      if (method.checked) {
        paymentMethod.id = method.id;

        if (method.items.length) {
          const checkedSubMethod = method.items.find(item => item.checked);
          paymentMethod.card = checkedSubMethod.id;
        }
      }
    });

    initCheckedShoppingData({
      deliveryAddress,
      deliveryMethod,
      paymentMethod
    });
  };

  navigateToContainer = () => {
    if (this.state.scrolled) return;
    const { hash } = location;
    if (!hash) return;
    const scrolled = scrollTo(hash);
    this.setState({ scrolled });
  }

  refreshCartPreview = (products) => {
    const { items, summaryPrice } = products;
    this.props.changeProducts(items, summaryPrice);
  };

  componentWillReceiveProps(nextProps) {
    const { ui: uiNext } = nextProps.checkout;
    const { ui: uiCurr } = this.props.checkout;

    if (uiNext.totals && this.state.initChecked) {
      this.initCheckedShoppingData(uiNext);
      this.setState({ initChecked: false }, this.navigateToContainer);
    }
    if (uiNext.products !== uiCurr.products) this.refreshCartPreview(uiNext.products);
  }

  getDeliveryMethodComponent = () => {
    const {
      ui,
      checkedData,
      isSending
    } = this.props.checkout;

    if (!ui.deliveryAddresses.isDeliverable) return null;

    if (this.props.checkout.ui.totals) {
      return (
        <DeliveryMethod
          changeDeliveryMethod={this.changeDeliveryMethod}
          checkedId={checkedData.deliveryMethod}
          isSending={isSending}
          ui={ui.deliveryMethods}
        />
      );
    }

    return (
      <div className="shopping-cart__block">
        <Spinner/>
      </div>
    );
  };

  changeDeliveryAddress = (id) => {
    this.props.changeDeliveryAddress(id);
  };

  changeDeliveryMethod = (id) => {
    this.props.changeDeliveryMethod(id);
  };

  render() {
    const {
      checkout: {
        ui,
        checkedData,
        newAddress
      },
      changeProductQuantity,
      changePaymentMethod,
      removeProduct,
      addNewAddress,
      saveAddress
    } = this.props;

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
        products,
        paymentMethods,
        totals,
        validationMessage,
        emailConfirmation
      } = ui;

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

      const totalsComponent = !totals
        ? <Spinner/>
        : <Total ui={totals}/>;

      const deliveryAddressComponent = deliveryAddresses.isDeliverable
        ? (
          <div className="shopping-cart__block">
            <DeliveryAddress
              changeDeliveryAddress={this.changeDeliveryAddress}
              checkedId={checkedData.deliveryAddress}
              disableInteractivity={!totals}
              addNewAddress={addNewAddress}
              ui={deliveryAddresses}
              newAddressObject={newAddress}
              saveAddress={saveAddress}
            />
          </div>
        ) : (
          <div className="shopping-cart__block">
            <h2>{deliveryAddresses.title}</h2>
            <Alert type="grey" text={deliveryAddresses.unDeliverableText}/>
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
          <EmailProof
            open={this.props.emailProof.show}
          />
          {welcomeMessage}
          <div className="shopping-cart__block">
            <Products
              removeProduct={removeProduct}
              disableInteractivity={!totals}
              changeProductQuantity={changeProductQuantity}
              ui={products}
              toggleEmailProof={this.props.toggleEmailProof}
            />
          </div>

          {deliveryAddressComponent}
          {emailConfirmationContent}
          {this.getDeliveryMethodComponent()}

          <div className="shopping-cart__block">
            <PaymentMethod
              validationMessage={validationMessage}
              changePaymentMethod={changePaymentMethod}
              checkedObj={checkedData.paymentMethod}
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
              isLoading={!totals}
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
  const { checkout, emailProof } = state;

  const { redirectURL } = checkout.sendData;
  if (redirectURL) location.assign(redirectURL);

  return { checkout, emailProof };
}, {
  getUI,
  initCheckedShoppingData,
  sendData,
  removeProduct,
  changeProductQuantity,
  changeProducts,
  addNewAddress,
  saveAddress,
  changeDeliveryAddress,
  changeDeliveryMethod,
  changePaymentMethod,
  toggleEmailProof
})(Checkout);
