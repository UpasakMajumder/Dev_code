import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';
import Checkbox from 'app.dump/Form/CheckboxInput';

class NewAddressDialog extends Component {
  constructor(props) {
    super(props);

    const { fields } = this.props.ui;

    this.stateIndex = fields.findIndex(element => element.id === 'state');

    const defaultCountry = fields.find(field => field.id === 'country').values.find(country => country.isDefault);

    this.state = {
      saveAddress: false,
      invalids: [],
      address: {
        customerName: '',
        address1: '',
        address2: '',
        city: '',
        state: '',
        zip: '',
        country: defaultCountry && defaultCountry.id,
        phone: '',
        email: ''
      },
      fields: this.getNewStateFields(fields, defaultCountry && defaultCountry.id)
    };
  }

  submit = async () => {
    const { address } = this.state;

    const invalids = [];
    const bodyData = this.getBodyData();

    bodyData.forEach((data) => {
      if (!data.isOptional) {
        if (!address[data.id]) {
          if (data.id === 'state') {
            this.countryHasState(address.country) && invalids.push(data.id);
          } else {
            invalids.push(data.id);
          }
        }
      }
    });

    this.setState({ invalids });
    if (invalids.length) return;
    await this.props.saveAddress({ id: -1, ...address, temporary: !this.state.saveAddress }, true);
    this.props.addNewAddress(); // get totals
    this.props.closeDialog();
  };

  getValidationError = (field) => {
    if (this.state.invalids.includes(field)) return this.props.ui.requiredErrorMessage;
    return null;
  };

  toggleSaveAddress = () => {
    this.setState({ saveAddress: !this.state.saveAddress });
  };

  render () {
    const { closeDialog, ui, userNotification } = this.props;

    const footer = (
      <div className="flex--center--between">
        <div>
          <Checkbox
            id="save-address"
            label={ui.saveAddressCheckbox}
            type="checkbox"
            onChange={this.toggleSaveAddress}
            checked={this.state.saveAddress}
          />
        </div>

        <div className="btn-group btn-group--right">
          <button
            onClick={closeDialog}
            type="button"
            className="btn-action btn-action--secondary"
          >
            {ui.discardBtnLabel}
          </button>

          <button
            onClick={this.submit}
            type="button"
            className="btn-action"
          >
            {ui.submitBtnLabel}
          </button>
        </div>
      </div>
    );

    const bodyContent1Part = [];
    const bodyContent2Part = [];

    const bodyData = this.getBodyData();

    bodyData.forEach((data, i) => {
      let element = <TextInput {...data} />;

      if (data.isSelect) {
        if (data.options.length) {
          element = <Select {...data} />;
        } else {
          return null;
        }
      }

      if (i + 1 <= Math.ceil(bodyData.length / 2)) {
        bodyContent1Part.push(<td key={i}>{element}</td>);
      } else {
        bodyContent2Part.push(<td key={i}>{element}</td>);
      }

      return null;
    });

    const row1 = <tr>{bodyContent1Part}</tr>;
    const row2 = <tr>{bodyContent2Part}</tr>;

    const userNotificationComponent = userNotification ? <Alert type="info" text={userNotification}/> : null;

    const body = (
      <div>
        {userNotificationComponent}

        <table className="checkout__dialog-table">
          <tbody>
          {row1}
          {row2}
          </tbody>
        </table>
      </div>
    );

    return (
      <Dialog
        closeDialog={closeDialog}
        hasCloseBtn={true}
        title={ui.title}
        body={body}
        footer={footer}
      />
    );
  }

  static propTypes = {
    saveAddress: PropTypes.func.isRequired,
    addNewAddress: PropTypes.func.isRequired,
    closeDialog: PropTypes.func.isRequired,
    userNotification: PropTypes.string,
    ui: PropTypes.shape({
      title: PropTypes.string.isRequired,
      discardBtnLabel: PropTypes.string.isRequired,
      submitBtnLabel: PropTypes.string.isRequired,
      saveAddressCheckbox: PropTypes.string.isRequired
    }).isRequired
  };

  removeFromInvalids = (field) => {
    const invalids = this.state.invalids.filter(invalid => invalid !== field);
    this.setState({ invalids });
  };

  getNewStateFields = (fields, countryId) => {
    if (!countryId) return fields;
    const options = fields.find(field => field.id === 'country').values.find(country => country.id === countryId).values;
    const state = fields.find(element => element.id === 'state');

    return [
      ...fields.slice(0, this.stateIndex),
      {
        ...state,
        values: options
      },
      ...fields.slice(this.stateIndex + 1)
    ];
  }

  countryHasState = (countryId) => {
    if (!countryId) return false;
    return !!this.props.ui.fields.find(field => field.id === 'country').values.find(country => country.id === countryId).values.length;
  }

  changeInput = (value, field) => {
    this.removeFromInvalids(field);

    if (field === 'country') {
      const newState = this.countryHasState(value) ? this.state.fields.state : '';
      this.setState({
        address: {
          ...this.state.address,
          [field]: value,
          state: newState
        },
        fields: this.getNewStateFields(this.state.fields, value)
      });
    } else {
      this.setState({
        address: {
          ...this.state.address,
          [field]: value
        }
      });
    }
  };

  getBodyData = () => {
    return this.state.fields.map((field) => {
      return {
        id: field.id,
        label: field.label,
        placeholder: field.label,
        onChange: (e) => { this.changeInput(e.target.value, field.id); },
        error: this.getValidationError(field.id),
        isOptional: field.isOptional,
        isSelect: field.type === 'select',
        options: field.values,
        value: (field.id === 'country' || field.id === 'state') ? this.state.address[field.id] || field.label : this.state.address[field.id]
      };
    });
  }
}

export default NewAddressDialog;
