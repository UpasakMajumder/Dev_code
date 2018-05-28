import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Alert from 'app.dump/Alert';
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';
/* globals */
import { STATIC_FIELDS } from 'app.globals';

class AddressDialog extends Component {
  constructor(props) {
    super(props);

    const { address, dialog } = props;
    const fieldValues = (address && typeof address === 'object') ? address : {};

    this.stateIndex = dialog.fields.findIndex(element => element.id === 'state');

    this.state = {
      fieldValues: {
        ...fieldValues,
        id: fieldValues.id || -1
      },
      fields: fieldValues.state ? this.getNewStateFields(dialog.fields, fieldValues.country) : dialog.fields,
      inValidFields: []
    };
  }

  static propTypes = {
    addDataAddress: PropTypes.func.isRequired,
    changeDataAddress: PropTypes.func.isRequired,
    closeDialog: PropTypes.func.isRequired,
    isModifyingDialog: PropTypes.bool.isRequired,
    address: PropTypes.shape({
      city: PropTypes.string,
      id: PropTypes.number,
      isEditButton: PropTypes.bool,
      isRemoveButton: PropTypes.bool,
      state: PropTypes.string,
      address1: PropTypes.string,
      address2: PropTypes.string,
      zip: PropTypes.string
    }),
    dialog: PropTypes.shape({
      buttons: PropTypes.shape({
        discard: PropTypes.string.isRequired,
        save: PropTypes.string.isRequired
      }).isRequired,
      fields: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.string.isRequired,
        label: PropTypes.string.isRequired,
        type: PropTypes.string.isRequired,
        values: PropTypes.arrayOf(PropTypes.shape({
          id: PropTypes.string.isRequired,
          name: PropTypes.string.isRequired,
          values: PropTypes.arrayOf(PropTypes.shape({
            id: PropTypes.string.isRequired,
            name: PropTypes.string.isRequired
          })).isRequired
        })).isRequired
      }).isRequired).isRequired,
      types: PropTypes.shape({
        add: PropTypes.string.isRequired,
        edit: PropTypes.string.isRequired
      }).isRequired
    })
  };

  getNewStateFields = (fields, countryId) => {
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
    return !!this.props.dialog.fields.find(field => field.id === 'country').values.find(country => country.id === countryId).values.length;
  }

  handleChange = (value, id) => {
    const { inValidFields, fieldValues, fields } = this.state;
    const inValidFieldsNew = inValidFields.filter(inValidField => inValidField !== id);

    if (id === 'country') {
      const newState = this.countryHasState(value) ? this.state.fieldValues.state : '';

      this.setState({
        fieldValues: {
          ...fieldValues,
          [id]: value,
          state: newState
        },
        inValidFields: inValidFieldsNew,
        fields: this.getNewStateFields(fields, value)
      });
    } else {
      this.setState({
        fieldValues: {
          ...fieldValues,
          [id]: value
        },
        inValidFields: inValidFieldsNew
      });
    }
  };

  closeDialog = () => {
    const { closeDialog } = this.props;
    closeDialog();
    this.setState({
      fieldValues: {
        id: -1,
        address1: '',
        address2: '',
        city: '',
        state: '',
        zip: '',
        email: '',
        phone: '',
        customerName: ''
      }
    });
  };

  submitForm = (data) => {
    const { addDataAddress, changeDataAddress, isModifyingDialog, dialog } = this.props;
    const { fieldValues } = this.state;
    const requiredFields = dialog.fields.filter(field => !field.isOptional).map(field => field.id);
    this.countryHasState(fieldValues.country) && requiredFields.push('state');
    const inValidFields = requiredFields.filter(requiredFiled => !fieldValues[requiredFiled]);

    if (!inValidFields.length) {
      isModifyingDialog ? changeDataAddress(data) : addDataAddress(data);
    }
    this.setState({ inValidFields });
  };

  getErrorMessage = (id) => {
    const { inValidFields } = this.state;
    if (inValidFields.includes(id)) return STATIC_FIELDS.validation.requiredMessage;
    return '';
  };

  render() {
    const { dialog, isModifyingDialog } = this.props;
    const { fieldValues, fields } = this.state;

    const footer = <div className="btn-group btn-group--right">
      <button onClick={this.closeDialog}
              type="button"
              className="btn-action btn-action--secondary"
      >
        {dialog.buttons.discard}
      </button>

      <button onClick={() => { this.submitForm(fieldValues); }}
              type="button"
              className="btn-action"
      >
        {dialog.buttons.save}
      </button>
    </div>;


    const row1 = [];
    const row2 = [];

    fields.forEach((field, i) => {
      const { label, values, type, id, isOptional } = field;

      let input = null;

      if (type === 'text') {
        input = (
          <TextInput label={label}
            error={this.getErrorMessage(id)}
            value={fieldValues[id]}
            isOptional={isOptional}
            placeholder={label}
            onChange={(e) => { this.handleChange(e.target.value, id); }}
            type="text"
            maximumLength={id}
          />
        );
      } else if (values.length) {
        input = (
          <Select label={label}
            error={this.getErrorMessage(id)}
            options={values}
            value={fieldValues[id] || label}
            onChange={(e) => { this.handleChange(e.target.value, id); }}
          />
        );
      }

      if (i + 1 <= Math.ceil(fields.length / 2)) {
        row1.push(<td key={id}>{input}</td>);
      } else {
        row2.push(<td key={id}>{input}</td>);
      }
    });

    const userNotification = dialog.userNotification ? <Alert type="info" text={dialog.userNotification}/> : null;

    const body = (
      <div>
        {userNotification}

        <table className="cart__dialog-table">
          <tbody>
            <tr>{row1}</tr>
            <tr>{row2}</tr>
          </tbody>
        </table>
      </div>
    );

    const title = isModifyingDialog ? dialog.types.edit : dialog.types.add;

    return <Dialog closeDialog={this.closeDialog}
                   hasCloseBtn={true}
                   title={title}
                   body={body}
                   footer={footer}/>;
  }
}

export default AddressDialog;
