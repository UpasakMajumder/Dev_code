import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';
import Tooler from 'app.dump/Tooler';

class MailingDialog extends Component {
  constructor(props) {
    super(props);
    this.defaultState = { errorList: null };
    this.state = { ...this.defaultState };
  }

  componentWillReceiveProps(nextProps) {
    if (!this.state.errorList && nextProps.errorList) {
      this.setState({ errorList: nextProps.errorList });
    }
  }

  handleCloseDialog = () => {
    this.setState({ ...this.defaultState });
    this.props.closeDialog();
  };

  getFooter = () => {
    const { errorList } = this.state;
    const { reprocessAddresses, formInfo } = this.props;
    const { downloadErrorFile, discardChanges, confirmChanges } = formInfo;

    let downloadErrorFileList = null;
    if (downloadErrorFile.url) {
      downloadErrorFileList = (
        <a href={downloadErrorFile.url} className="btn-action btn-action--secondary">{downloadErrorFile.text}</a>
      );
    }

    return (
      <div className="btn-group btn-group--right">
        { downloadErrorFileList }
        <Button text={discardChanges} onClick={this.handleCloseDialog} type="action" btnClass="btn-action--secondary"/>
        <Button text={confirmChanges.text} onClick={() => reprocessAddresses(errorList)} type="action"/>
      </div>
    );
  };

  handleChange = (index, field, value) => {
    this.setState((prevState) => {
      return {
        errorList: [
          ...prevState.errorList.slice(0, index),
          {
            ...prevState.errorList[index],
            [field]: value
          },
          ...prevState.errorList.slice(index + 1)
        ]
      };
    });
  };

  getErrorMessage = (index, field) => {
    const { formInfo, emptyFields } = this.props;
    if (!Object.keys(emptyFields).length || !emptyFields[index]) return null;
    if (emptyFields[index].includes(field)) return formInfo.message.required;
    return null;
  };

  getBody = () => {
    const { errorList } = this.state;
    const { fields } = this.props.formInfo;

    const theader = (
      <tr>
        <th>{fields.error.header}</th>
        <th>{fields.fullName.header}</th>
        <th>{fields.firstAddressLine.header}</th>
        <th>{fields.secondAddressLine.header}</th>
        <th>{fields.city.header}</th>
        <th>{fields.state.header}</th>
        <th>{fields.postalCode.header}</th>
      </tr>
    );

    const tbody = errorList.map((errorItem, index) => {
      return (
        <tr key={errorItem.id}>
          <td>
            {errorItem.errorMessage}
          </td>
          <td>
            <TextInput onChange={(e) => { this.handleChange(index, 'fullName', e.target.value); }}
                       type="text"
                       error={this.getErrorMessage(index, 'fullName')}
                       value={errorItem.fullName}/>
          </td>
          <td>
            <TextInput onChange={(e) => { this.handleChange(index, 'firstAddressLine', e.target.value); }}
                       type="text"
                       error={this.getErrorMessage(index, 'firstAddressLine')}
                       value={errorItem.firstAddressLine}/>
          </td>
          <td>
            <TextInput onChange={(e) => { this.handleChange(index, 'secondAddressLine', e.target.value); }}
                       type="text"
                       error={this.getErrorMessage(index, 'secondAddressLine')}
                       value={errorItem.secondAddressLine}/>
          </td>
          <td>
            <TextInput onChange={(e) => { this.handleChange(index, 'city', e.target.value); }}
                       type="text"
                       error={this.getErrorMessage(index, 'city')}
                       value={errorItem.city}/>
          </td>
          <td>
            <Select onChange={(e) => { this.handleChange(index, 'state', e.target.value); }}
                    value={errorItem.state}
                    error={this.getErrorMessage(index, 'state')}
                    options={['', ...fields.state.value]}/>
          </td>
          <td>
            <TextInput onChange={(e) => { this.handleChange(index, 'postalCode', e.target.value); }}
                       type="text"
                       error={this.getErrorMessage(index, 'postalCode')}
                       value={errorItem.postalCode}/>
          </td>
        </tr>
      );
    });

    return (
      <div className="processed-list__table-dialog">
        <table className="table">
          <tbody>
          {theader}
          {tbody}
          </tbody>
        </table>
      </div>
    );
  };

  render() {
    const { errorList } = this.state;
    const title = errorList ? this.props.formInfo.title : 'null';
    const body = errorList ? this.getBody() : null;
    const footer = errorList ? this.getFooter() : null;

    return (
      <Dialog
        closeDialog={this.handleCloseDialog}
        hasCloseBtn={true}
        footer={footer}
        body={body}
        title={title}
        open={this.props.open}
      />
    );
  }

  static propTypes = {
    open: PropTypes.bool.isRequired,
    emptyFields: PropTypes.object.isRequired,
    closeDialog: PropTypes.func.isRequired,
    formInfo: PropTypes.shape({
      title: PropTypes.string.isRequired,
      downloadErrorFile: PropTypes.shape({
        url: PropTypes.string,
        text: PropTypes.string.isRequired
      }).isRequired,
      discardChanges: PropTypes.string.isRequired,
      confirmChanges: PropTypes.shape({
        text: PropTypes.string.isRequired
      }).isRequired,
      message: PropTypes.shape({
        required: PropTypes.string.isRequired
      }).isRequired,
      fields: PropTypes.shape({
        fullName: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        firstAddressLine: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        secondAddressLine: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        city: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        state: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        postalCode: PropTypes.shape({
          header: PropTypes.string.isRequired,
          required: PropTypes.bool,
          values: PropTypes.array
        }).isRequired,
        error: PropTypes.shape({
          header: PropTypes.string.isRequired
        }).isRequired
      }).isRequired
    }).isRequired,
    errorList: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.string.isRequired,
      fullName: PropTypes.string.isRequired,
      firstAddressLine: PropTypes.string.isRequired,
      secondAddressLine: PropTypes.string,
      city: PropTypes.string.isRequired,
      state: PropTypes.string.isRequired,
      postalCode: PropTypes.string.isRequired,
      errorMessage: PropTypes.string.isRequired
    }).isRequired).isRequired
  };
}

export default MailingDialog;
