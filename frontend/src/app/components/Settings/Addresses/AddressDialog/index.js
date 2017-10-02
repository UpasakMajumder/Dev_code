import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';
/* globals */
import { STATIC_FIELDS } from 'app.globals';

class AddressDialog extends Component {
  constructor(props) {
    super(props);

    const { address } = props;
    const fieldValues = (address && typeof address === 'object') ? address : {};

    this.state = {
      fieldValues: {
        id: fieldValues.id || -1,
        street1: fieldValues.street1 || '',
        street2: fieldValues.street2 || '',
        city: fieldValues.city || '',
        state: fieldValues.state || '',
        zip: fieldValues.zip || ''
      },
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
      street1: PropTypes.string,
      street2: PropTypes.string,
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
        values: PropTypes.arrayOf(PropTypes.string)
      }).isRequired).isRequired,
      types: PropTypes.shape({
        add: PropTypes.string.isRequired,
        edit: PropTypes.string.isRequired
      }).isRequired
    })
  };

  handleChange = (value, id) => {
    const { inValidFields, fieldValues } = this.state;
    const inValidFieldsNew = inValidFields.filter(inValidField => inValidField !== id);

    this.setState({
      fieldValues: {
        ...fieldValues,
        [id]: value
      },
      inValidFields: inValidFieldsNew
    });
  };

  closeDialog = () => {
    const { closeDialog } = this.props;
    closeDialog();
    this.setState({
      fieldValues: {
        id: -1,
        street1: '',
        street2: '',
        city: '',
        state: '',
        zip: ''
      }
    });
  };

  submitForm = (data) => {
    const { addDataAddress, changeDataAddress, isModifyingDialog } = this.props;
    const { fieldValues } = this.state;
    const requiredFields = ['street1', 'city', 'state', 'zip'];
    const inValidFields = requiredFields.filter(requiredFiled => !fieldValues[requiredFiled]);

    if (!inValidFields.length) {
      isModifyingDialog ? changeDataAddress(data) : addDataAddress(data);
      this.closeDialog();
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
    const { fieldValues } = this.state;

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


    const bodyContent = dialog.fields.map((field) => {
      const { label, values, type, id, isOptional } = field;

      const input = (type === 'text')
        ? <TextInput label={label}
                     error={this.getErrorMessage(id)}
                     value={fieldValues[id]}
                     isOptional={isOptional}
                     placeholder={label}
                     onChange={(e) => { this.handleChange(e.target.value, id); }}
                     type="text"
        />
        : <Select label={label}
                  error={this.getErrorMessage(id)}
                  options={values}
                  value={fieldValues[id] || label}
                  onChange={(e) => { this.handleChange(e.target.value, id); }}
        />;

      return (
        <td key={id}>
          {input}
        </td>
      );
    });

    const body = <table className="cart__dialog-table">
      <tbody>
      <tr>
        {bodyContent}
      </tr>
      </tbody>
    </table>;

    const title = isModifyingDialog ? dialog.types.edit : dialog.types.add;

    return <Dialog closeDialog={this.closeDialog}
                   hasCloseBtn={true}
                   title={title}
                   body={body}
                   footer={footer}/>;
  }
}

export default AddressDialog;
