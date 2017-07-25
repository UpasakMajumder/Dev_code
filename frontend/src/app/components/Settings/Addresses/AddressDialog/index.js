import React, { Component } from 'react';
import Dialog from '../../../Dialog/index';
import TextInput from '../../../form/TextInput';
import Select from '../../../form/Select';

class AddressDialog extends Component {
  constructor() {
    super();

    this.state = {
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

    this.handleChange = this.handleChange.bind(this);
    this.closeDialog = this.closeDialog.bind(this);
    this.changeDataAddress = this.changeDataAddress.bind(this);
    this.getErrorMessage = this.getErrorMessage.bind(this);
  }

  handleChange(value, id) {
    const inValidFields = this.state.inValidFields.filter(inValidField => inValidField !== id);

    this.setState({
      fieldValues: {
        ...this.state.fieldValues,
        [id]: value
      },
      inValidFields
    });
  }

  componentWillReceiveProps(nextProps) {
    const { address } = nextProps;
    if (Object.keys(address).length > 1) {
      this.setState({
        fieldValues: address
      });
    }
  }

  closeDialog() {
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
  }

  changeDataAddress(data) {
    const { changeDataAddress } = this.props;
    const requiredFields = ['street1', 'city', 'state', 'zip'];
    const inValidFields = requiredFields.filter(requiredFiled => !this.state.fieldValues[requiredFiled]);

    if (!inValidFields.length) changeDataAddress(data);
    this.setState({ inValidFields });
  }

  getErrorMessage(id) {
    if (this.state.inValidFields.includes(id)) return 'The field is required';
    return '';
  }

  render() {
    const { isDialogOpen, dialog, address } = this.props;

    const { fieldValues } = this.state;

    const footer = <div className="btn-group btn-group--right">
      <button onClick={this.closeDialog} type="button" className="btn-action btn-action--secondary">{dialog.buttons.discard}</button>
      <button onClick={() => { this.changeDataAddress(this.state.fieldValues); }} type="button" className="btn-action">{dialog.buttons.save}</button>
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
                     type="text" />
        : <Select label={label}
                  error={this.getErrorMessage(id)}
                  options={values}
                  value={fieldValues[id] || label}
                  onChange={(e) => { this.handleChange(e.target.value, id); }}/>;

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


    const content = isDialogOpen
      ? <Dialog closeDialog={this.closeDialog}
                hasCloseBtn={true}
                title={title}
                body={body}
                footer={footer} />
      : null;

    return content;
  }
}

export default AddressDialog;
