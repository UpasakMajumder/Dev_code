import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import PaymentForm from 'app.dump/Form/PaymentForm';
import submitCard from 'app.ac/card-payment';
import { cardPaymentSymbols } from 'app.helpers/validationRules';
import { CARD_PAYMENT } from 'app.globals';
import Cards from 'app.dump/CreditCards';
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
      focused: ''
    };

    this.changeFieldValue = this.changeFieldValue.bind(this);
    this.changeFocus = this.changeFocus.bind(this);
    this.submit = this.submit.bind(this);
  }

  submit() {
    const { proceedCard } = this.props;
    const { fields, cardType } = this.state;
    proceedCard(fields, cardType);
  }

  changeFieldValue(type, value) {
    const cardType = this.refs.card.state.type.issuer;
    let maxLength;

    switch (type) {
    case 'number':
      maxLength = cardType === 'amex' ? cardPaymentSymbols.number.amex : cardPaymentSymbols.number.rest;
      break;
    case 'expiry':
      maxLength = cardPaymentSymbols.expiry.max;
      break;
    case 'cvc':
      maxLength = cardPaymentSymbols.cvc.max;
      break;
    default:
      maxLength = 1000;
      break;
    }

    if (value.length > maxLength) return;

    this.setState({
      ...this.state,
      cardType,
      fields: {
        ...this.state.fields,
        [type]: value
      }
    });
  }

  changeFocus(type) {
    this.setState({
      focused: type
    });
  }

  render() {
    const { fields, focused } = this.state;
    const { errorField, errorMessage, isProceeded } = this.props;

    return (
      <div className="card-payment">
        <div className="card-payment__form">
          <div className="card-payment__block">
            <Cards
              ref="card"
              {...fields}
              focused={focused}
              acceptedCards={CARD_PAYMENT.acceptedCards}
            />
          </div>
          <div className="card-payment__block">
            <PaymentForm
              errorField={errorField}
              errorMessage={errorMessage}
              {...fields}
              changeFocus={this.changeFocus}
              changeFieldValue={this.changeFieldValue}
            />
          </div>
        </div>
        <div className="card-payment__submit">
          <button disabled={isProceeded}
                  onClick={this.submit}
                  type='button'
                  className='btn-action'
          >
            {CARD_PAYMENT.submitButtonText}
          </button>
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
  proceedCard: submitCard
})(Payment);
