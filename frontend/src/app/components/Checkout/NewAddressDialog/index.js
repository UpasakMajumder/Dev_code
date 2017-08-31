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
    return this.props.ui.fields.map((field) => {
      return {
        id: field.id,
        label: field.label,
        placeholder: field.label,
        onChange: (e) => { this.changeInput(e.target.value, field.id); },
        error: this.getValidationError(field.id),
        isOptional: field.isOptional,
        isSelect: field.type === 'select',
        options: field.values
      };
    });
  }
}

export default NewAddressDialog;
