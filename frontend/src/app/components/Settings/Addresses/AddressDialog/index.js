import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';

class AddressDialog extends Component {
  state = {
    fieldValues: {
      id: -1,
      street1: '',
      street2: '',
      city: '',
      state: '',
      zip: ''
    },
    inValidFields: []
  };

  static propTypes = {
    changeDataAddress: PropTypes.func.isRequired,
    closeDialog: PropTypes.func.isRequired,
    isDialogOpen: PropTypes.bool.isRequired,
    address: PropTypes.shape({
      city: PropTypes.string,
      id: PropTypes.number,
      isEditButton: PropTypes.bool,
      isRemoveButton: PropTypes.bool,
      state: PropTypes.string,
      street1: PropTypes.string,
      street2: PropTypes.string,
      zip: PropTypes.string
    }).isRequired,
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

  componentWillReceiveProps(nextProps) {
    const { address } = nextProps;
    if (Object.keys(address).length > 1) this.setState({ fieldValues: address });
  }

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

  changeDataAddress = (data) => {
    const { changeDataAddress } = this.props;
    const { fieldValues } = this.state;
    const requiredFields = ['street1', 'city', 'state', 'zip'];
    const inValidFields = requiredFields.filter(requiredFiled => !fieldValues[requiredFiled]);

    if (!inValidFields.length) changeDataAddress(data);
    this.setState({ inValidFields });
  };

  getErrorMessage = (id) => {
    const { inValidFields } = this.state;
    if (inValidFields.includes(id)) return 'The field is required';
    return '';
  };

  render() {
    const { isDialogOpen, dialog, address } = this.props;
    const { fieldValues } = this.state;

    const footer = <div className="btn-group btn-group--right">
      <button onClick={this.closeDialog}
              type="button"
              className="btn-action btn-action--secondary"
      >
        {dialog.buttons.discard}
      </button>

      <button onClick={() => { this.changeDataAddress(fieldValues); }}
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

    let title = dialog.types.edit;
    if (Object.keys(address).length === 1) title = dialog.types.add;

    return isDialogOpen
      ? <Dialog closeDialog={this.closeDialog}
                hasCloseBtn={true}
                title={title}
                body={body}
                footer={footer}
      />
      : null;
  }
}

export default AddressDialog;
