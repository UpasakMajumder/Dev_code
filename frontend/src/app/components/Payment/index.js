import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Cards from 'react-credit-cards';
import PaymentForm from './PaymentForm';
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
      focused: ''
    };

    this.changeFieldValue = this.changeFieldValue.bind(this);
    this.changeFocus = this.changeFocus.bind(this);
  }

  changeFieldValue(type, value) {
    this.setState({
      ...this.state,
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

    return (
      <div className="card-payment">
        <div className="card-payment__form">
          <div className="card-payment__block">
            <Cards
              {...fields}
              focused={focused}
              acceptedCards={['visa', 'mastercard', 'amex']}
            />
          </div>

          <div className="card-payment__block">
            <PaymentForm
              {...fields}
              changeFocus={this.changeFocus}
              changeFieldValue={this.changeFieldValue}
            />
          </div>
        </div>
        <div className="card-payment__submit">
          <button type='button' className='btn-action'>Proceed with payment</button>
        </div>

      </div>
    );
  }
}

export default Payment;
