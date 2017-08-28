import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* Components */
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';

class NewAddressDialog extends Component {
  state = {
    invalids: []
  };

  submit = () => {
    const { address, submit, closeDialog } = this.props;
    const invalids = [];
    const bodyData = this.getBodyData();
    bodyData.forEach((data) => {
      if (!data.isOptional) {
        if (!address[data.id]) invalids.push(data.id);
      }
    });

    this.setState({ invalids });
    if (invalids.length) return;
    submit();
    closeDialog();
  };

  getValidationError = (field) => {
    if (this.state.invalids.includes(field)) return this.props.ui.requiredErrorMessage;
    return null;
  };

  render () {
    const { isDialogOpen, closeDialog, ui } = this.props;

    const footer = (
      <div className="btn-group btn-group--right">
        <button onClick={closeDialog}
                type="button"
                className="btn-action btn-action--secondary"
        >
          {ui.discardBtnLabel}
        </button>

        <button onClick={this.submit}
                type="button"
                className="btn-action"
        >
          {ui.submitBtnLabel}
        </button>
      </div>
    );

    const bodyContent1Part = [];
    const bodyContent2Part = [];

    const bodyData = this.getBodyData();

    bodyData.map((data, i) => {
      let element = <TextInput {...data} />;
      if (data.isSelect) element = <Select {...data} />;

      if (i + 1 <= Math.ceil(bodyData.length / 2)) {
        bodyContent1Part.push(<td key={i}>{element}</td>);
      } else {
        bodyContent2Part.push(<td key={i}>{element}</td>);
      }

      return null;
    });

    const row1 = <tr>{bodyContent1Part}</tr>;
    const row2 = <tr>{bodyContent2Part}</tr>;


    const body = (
      <table className="checkout__dialog-table">
        <tbody>
        {row1}
        {row2}
        </tbody>
      </table>
    );

    return isDialogOpen
      ?
      (
        <Dialog
          closeDialog={closeDialog}
          hasCloseBtn={true}
          title={ui.title}
          body={body}
          footer={footer}
        />
      )
      : null;
  }

  static propTypes = {
    isDialogOpen: PropTypes.bool.isRequired,
    address: PropTypes.shape({
      customerName: PropTypes.string,
      address1: PropTypes.string,
      address2: PropTypes.string,
      city: PropTypes.string,
      state: PropTypes.string,
      zip: PropTypes.string,
      country: PropTypes.string,
      phone: PropTypes.string,
      email: PropTypes.string
    }).isRequired,
    submit: PropTypes.func.isRequired,
    closeDialog: PropTypes.func.isRequired,
    ui: PropTypes.shape({
      title: PropTypes.string.isRequired,
      discardBtnLabel: PropTypes.string.isRequired,
      submitBtnLabel: PropTypes.string.isRequired
    }).isRequired
  };

  removeFromInvalids = (field) => {
    const invalids = this.state.invalids.filter(invalid => invalid !== field);
    this.setState({ invalids });
  };

  changeInput = (value, field) => {
    this.removeFromInvalids(field);
    this.props.changeInput(value, field);
  };

  getBodyData = () => {
    const { customerName, address1, address2, city, state, zip, country, phone, email } = this.props.ui.fields;

    return [
      {
        id: customerName.id,
        label: customerName.label,
        value: this.props.address.customerName,
        placeholder: customerName.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, customerName.id); },
        error: this.getValidationError(customerName.id),
        isOptional: customerName.isOptional
      },
      {
        id: address1.id,
        label: address1.label,
        value: this.props.address.address1,
        placeholder: address1.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, address1.id); },
        error: this.getValidationError(address1.id),
        isOptional: address1.isOptional
      },
      {
        id: address2.id,
        label: address2.label,
        value: this.props.address.address2,
        placeholder: 'Address 2',
        onChange: (e) => { this.changeInput(e.target.value, address2.id); },
        error: this.getValidationError(address2.id),
        isOptional: address2.isOptional
      },
      {
        id: city.id,
        label: city.label,
        value: this.props.address.city,
        placeholder: city.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, city.id); },
        error: this.getValidationError(city.id),
        isOptional: city.isOptional
      },
      {
        id: state.id,
        label: state.label,
        value: this.props.address.state,
        placeholder: state.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, state.id); },
        error: this.getValidationError(state.id),
        isOptional: state.isOptional
      },
      {
        id: zip.id,
        label: zip.label,
        value: this.props.address.zip,
        placeholder: zip.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, zip.id); },
        error: this.getValidationError(zip.id),
        isOptional: zip.isOptional
      },
      {
        id: country.id,
        label: country.label,
        value: this.props.address.country || 'Country',
        placeholder: country.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, country.id); },
        error: this.getValidationError(country.id),
        isOptional: country.isOptional,
        isSelect: true,
        options: country.values
      },
      {
        id: phone.id,
        label: phone.label,
        value: this.props.address.phone,
        placeholder: phone.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, phone.id); },
        error: this.getValidationError(phone.id),
        isOptional: phone.isOptional
      },
      {
        id: email.id,
        label: email.label,
        value: this.props.address.email,
        placeholder: email.placeholder,
        onChange: (e) => { this.changeInput(e.target.value, email.id); },
        error: this.getValidationError(email.id),
        isOptional: email.isOptional
      }
    ];
  }
}

export default NewAddressDialog;
