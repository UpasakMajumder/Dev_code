import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* component */
import SVG from 'app.dump/SVG';
import TextInput from 'app.dump/Form/TextInput';

class PaymentForm extends Component {
  getErrorMessage = (field) => {
    const { invalids } = this.props;
    const invalidField = invalids.filter(invalid => invalid.errorField === field)[0];

    return invalidField ? invalidField.errorMessage : '';
  };

  render() {
    const {
      number,
      name,
      cvc,
      expiry,
      changeFieldValue,
      changeFocus,
      addSlashToExpirationDate,
      staticData
    } = this.props;
    const cardNumbersSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card-am'/>
      <SVG className='card-payment__icon' name='card-mc'/>
      <SVG className='card-payment__icon' name='card-vs'/>
    </div>;
    const cvcSvgs = <div className="card-payment__icon-block">
      <SVG className='card-payment__icon' name='card'/>
    </div>;

    return (
      <div>
        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('number', e.target.value); }}
            onFocus={() => { changeFocus('number'); }}
            label={staticData.number.label}
            placeholder={staticData.number.placeholder}
            value={number}
            error={this.getErrorMessage('number')}
          />
        </div>

        <div className="card-payment__field">
          <TextInput
            onChange={(e) => { changeFieldValue('name', e.target.value); }}
            onFocus={() => { changeFocus('name'); }}
            label={staticData.name.label}
            placeholder={staticData.name.placeholder}
            error={this.getErrorMessage('name')}
            value={name}
          />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('expiry', e.target.value); }}
            onFocus={() => { changeFocus('expiry'); }}
            onBlur={addSlashToExpirationDate}
            label={staticData.expiry.label}
            placeholder={staticData.expiry.placeholder}
            error={this.getErrorMessage('expiry')}
            value={expiry}
          />
        </div>

        <div className="card-payment__field card-payment__field--half">
          <TextInput
            onChange={(e) => { changeFieldValue('cvc', e.target.value); }}
            onFocus={() => { changeFocus('cvc'); }}
            label={staticData.cvc.label}
            placeholder={staticData.cvc.placeholder}
            value={cvc}
            error={this.getErrorMessage('cvc')}
          />
        </div>
      </div>
    );
  }
}

PaymentForm.propTypes = {
  number: PropTypes.string.isRequired,
  name: PropTypes.string.isRequired,
  cvc: PropTypes.string.isRequired,
  expiry: PropTypes.string.isRequired,
  changeFieldValue: PropTypes.func.isRequired,
  changeFocus: PropTypes.func.isRequired,
  invalids: PropTypes.arrayOf(PropTypes.shape({
    errorField: PropTypes.string.isRequired,
    errorMessage: PropTypes.string.isRequired
  })).isRequired,
  addSlashToExpirationDate: PropTypes.func.isRequired,
  staticData: PropTypes.shape({
    number: PropTypes.shape({
      label: PropTypes.string.isRequired,
      placeholder: PropTypes.string.isRequired
    }).isRequired,
    name: PropTypes.shape({
      label: PropTypes.string.isRequired,
      placeholder: PropTypes.string.isRequired
    }).isRequired,
    expiry: PropTypes.shape({
      label: PropTypes.string.isRequired,
      placeholder: PropTypes.string.isRequired
    }).isRequired,
    cvc: PropTypes.shape({
      label: PropTypes.string.isRequired,
      placeholder: PropTypes.string.isRequired
    }).isRequired
  }).isRequired
};

export default PaymentForm;
