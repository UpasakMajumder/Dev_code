import React, { Component } from 'react';
import { connect } from 'react-redux';
/* AC */
import { submitCard, saveToProfile } from 'app.ac/card-payment';
/* helpers */
import { cardPaymentSymbols } from 'app.helpers/validationRules';
import { cardExpiration } from 'app.helpers/regexp';
import { getSearchObj } from 'app.helpers/location';
/* globals */
import { CARD_PAYMENT } from 'app.globals';
/* components */
import PaymentForm from 'app.dump/Form/PaymentForm';
import Cards from 'app.dump/CreditCards';
import Button from 'app.dump/Button';
import Checkbox from 'app.dump/Form/CheckboxInput';
/* styles */
import 'react-credit-cards/lib/styles-compiled.css';

class Payment extends Component {
  constructor() {
    super();

    this.state = {
      fields: {
        number: '',
        name: '',
        expiry: '',
        cvc: ''
      },
      cardType: 'unknown',
      focused: '',
      invalids: [],
      saveToProfile: false
    };
  }

  static getSubmissionId = () => {
    const { submissionId } = getSearchObj();
    return submissionId;
  };

  static getNewValueWithSlash = (value) => {
    if (value.includes('/') || value.length < cardPaymentSymbols.expiry.min) return value;
    return `${value.substr(0, 2)}/${value.substr(2)}`;
  };

  addSlashToExpirationDate = () => {
    this.setState({
      fields: {
        ...this.state.fields,
        expiry: Payment.getNewValueWithSlash(this.state.fields.expiry)
      }
    });
  };

  getInvalids = () => {
    const { fields, cardType } = this.state;
    const invalids = [];
    const staticData = CARD_PAYMENT.fields;

    const maxLength = cardType === 'amex' ? cardPaymentSymbols.number.amex : cardPaymentSymbols.number.rest;

    if (fields.number.length < maxLength) {
      invalids.push({
        errorField: 'number',
        errorMessage: staticData.number.inValidMessage
      });
    }

    if (!CARD_PAYMENT.acceptedCards.includes(cardType)) {
      invalids.push({
        errorField: 'number',
        errorMessage: CARD_PAYMENT.cardTypeInValidMessage
      });
    }

    if (fields.name.length < cardPaymentSymbols.name.min) {
      invalids.push({
        errorField: 'name',
        errorMessage: staticData.name.inValidMessage
      });
    }

    if (fields.cvc.length < cardPaymentSymbols.cvc.min) {
      invalids.push({
        errorField: 'cvc',
        errorMessage: staticData.cvc.inValidMessage
      });
    }

    if (fields.expiry.length < cardPaymentSymbols.expiry.min + 1) { // +1 – slash
      invalids.push({
        errorField: 'expiry',
        errorMessage: staticData.expiry.inValidMessage
      });
    }

    if (!fields.expiry.match(cardExpiration)) {
      invalids.push({
        errorField: 'expiry',
        errorMessage: staticData.expiry.inValidMessage
      });
    }

    return invalids;
  };

  submit = () => {
    const submissionId = Payment.getSubmissionId();

    const { proceedCard, saveToProfile } = this.props;
    const { fields, cardType } = this.state;

    const invalids = this.getInvalids();

    if (invalids.length) {
      this.setState({ invalids });
    } else {
      if (this.state.saveToProfile) saveToProfile(fields, CARD_PAYMENT.saveToProfile.url);
      proceedCard(fields, cardType, submissionId);
    }
  }

  static hasValueMaxLength = (type, value, cardType) => {
    let maxLength;

    switch (type) {
    case 'number':
      maxLength = cardType === 'amex' ? cardPaymentSymbols.number.amex : cardPaymentSymbols.number.rest;
      break;
    case 'expiry':
      maxLength = value.includes('/') ? cardPaymentSymbols.expiry.max + 1 : cardPaymentSymbols.expiry.max;
      break;
    case 'cvc':
      maxLength = cardPaymentSymbols.cvc.max;
      break;
    default:
      maxLength = 1000;
      break;
    }

    return value.length > maxLength;
  };

  removeFromInvalids = (type) => {
    const invalids = this.state.invalids.filter(invalid => invalid.errorField !== type);
    this.setState({ invalids });
  };

  changeFieldValue = (type, value) => {
    const cardType = this.refs.card.state.type.issuer;
    if (Payment.hasValueMaxLength(type, value, cardType)) return;

    this.removeFromInvalids(type);

    this.setState({
      cardType,
      fields: {
        ...this.state.fields,
        [type]: value
      }
    });
  };

  changeFocus = (type) => {
    this.setState({
      focused: type
    });
  };

  handleSaveToProfile = () => {
    this.setState({
      saveToProfile: !this.state.saveToProfile
    });
  };

  render() {
    const { fields, focused, invalids } = this.state;
    const { isProceeded } = this.props;
    const { saveToProfile } = CARD_PAYMENT;

    const saveToProfileComponent = saveToProfile.exists
      ? (
        <Checkbox
          id="saveToProfile"
          label={saveToProfile.label}
          type="checkbox"
          checked={this.state.saveToProfile}
          onChange={this.handleSaveToProfile}
        />
      ) : null;

    return (
      <div className="card-payment">
        <div className="card-payment__form">
          <div className="card-payment__block">
            <Cards
              ref="card"
              {...fields}
              focused={focused}
              locale={{ valid: CARD_PAYMENT.fields.expiry.cardLabel }}
              placeholders={{ name: CARD_PAYMENT.fields.name.cardLabel }}
              acceptedCards={CARD_PAYMENT.acceptedCards}
            />
          </div>
          <div className="card-payment__block">
            <PaymentForm
              invalids={invalids}
              staticData={CARD_PAYMENT.fields}
              addSlashToExpirationDate={this.addSlashToExpirationDate}
              {...fields}
              changeFocus={this.changeFocus}
              changeFieldValue={this.changeFieldValue}
            />
          </div>
        </div>
        <div className="card-payment__submit">
          {saveToProfileComponent}

          <Button
            text={CARD_PAYMENT.submitButtonText}
            type="action"
            isLoading={isProceeded}
            onClick={this.submit}
          />
        </div>
      </div>
    );
  }
}

export default connect((state) => {
  const { cardPayment } = state;
  const { errorField, errorMessage, isProceeded } = cardPayment;
  return { errorField, errorMessage, isProceeded };
}, {
  proceedCard: submitCard,
  saveToProfile
})(Payment);
