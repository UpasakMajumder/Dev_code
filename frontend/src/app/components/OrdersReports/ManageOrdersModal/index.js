import React, { Component } from 'react';
import PropTypes from 'prop-types';
import uuid from 'uuid';
import moment from 'moment';
import axios from 'axios';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';
import TextInput from 'app.dump/Form/TextInput';
import Select from 'app.dump/Form/Select';
import Datepicker from 'app.dump/Form/Datepicker';
/* helpers */
import timeFormat, { dateFormat } from 'app.helpers/time';
/* consts */
import { FAILURE, SUCCESS } from 'app.consts';

class ManageOrdersModal extends Component {
  static propTypes = {
    open: PropTypes.bool.isRequired,
    closeModal: PropTypes.func.isRequired,
    ui: PropTypes.shape({
      popupTitle: PropTypes.string.isRequired,
      popupButtons: PropTypes.shape({
        cancel: PropTypes.string.isRequired,
        save: PropTypes.string.isRequired
      }).isRequired,
      headers: PropTypes.arrayOf(PropTypes.shape({
        id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
        label: PropTypes.string.isRequired
      }).isRequired).isRequired,
      submitUrl: PropTypes.string.isRequired
    }).isRequired,
    rows: PropTypes.arrayOf(PropTypes.object).isRequired,
    manage: PropTypes.func.isRequired
  }

  state = {
    isLoading: false
  };

  componentWillReceiveProps(nextProps) {
    if ((nextProps.open && nextProps.open !== this.props.open) || nextProps.rows !== this.props.rows) {
      const fields = [];

      nextProps.rows.forEach((row, i) => {
        const field = {};
        nextProps.ui.headers.forEach((header) => {
          const item = row.items[header.id];

          if (!item) {
            field[header.id] = '';
          } else {
            field[header.id] = item.value;
          }
        });

        fields[i] = field;
      });

      this.setState({ fields });
    } else if (!nextProps.open && nextProps.open !== this.props.open) {
      this.setState({ fields: [] });
    }
  }

  getHeaders = () => {
    return this.props.ui.headers.map((header) => {
      return <th key={header.id}>{header.label}</th>;
    });
  };

  changeField = (rowNumber, headerId, value) => {
    const newFields = JSON.parse(JSON.stringify(this.state.fields));

    newFields[rowNumber] = {
      ...this.state.fields[rowNumber],
      [headerId]: value
    };

    this.setState({ fields: newFields });
  };

  getRows = () => {
    if (!this.props.rows.length) return null;

    return Object.keys(this.state.fields).map((rowNumber) => {
      const items = this.props.ui.headers.map((header, headerIndex) => {
        const item = this.state.fields[rowNumber][header.id];
        if (Array.isArray(item)) {
          return <td key={`${rowNumber}-${header.id}`}>I am array</td>;
        } else if (header.type === 'date') {
          return (
            <td key={`${rowNumber}-${header.id}`}>
              <Datepicker
                selected={moment(item)}
                dateFormat={dateFormat}
                onChange={date => this.changeField(rowNumber, header.id, date)}
                readOnly
              />
            </td>
          );
        } else if (header.type === 'number') {
          return (
            <td key={`${rowNumber}-${header.id}`}>
              <TextInput
                type="number"
                value={item}
                onChange={e => this.changeField(rowNumber, header.id, e.target.value)}
              />
            </td>
          );
        } else if (header.edit) {
          return (
            <td key={`${rowNumber}-${header.id}`}>
              <TextInput
                type="text"
                value={item}
                onChange={e => this.changeField(rowNumber, header.id, e.target.value)}
              />
            </td>
          );
        } else if (header.type === 'select') {
          return (
            <td key={`${rowNumber}-${header.id}`}>
              <Select
                options={header.values}
                placeholder={header.label}
                value={item}
                onChange={e => this.changeField(rowNumber, header.id, e.target.value)}
              />
            </td>
          );
        } else if (headerIndex === this.props.ui.headers.length - 1) {
          return (
            <td key={uuid()}>
              <Button
                text="Add"
                type="action"
                onClick={() => console.log('add')}
              />
            </td>
          );
        }

        return <td key={`${rowNumber}-${header.id}`}>{item}</td>;
      });

      return <tr key={rowNumber}>{items}</tr>;
    });
  };

  getBody = () => {
    return (
      <table className="show-table">
        <tbody>
          <tr>{this.getHeaders()}</tr>
          {this.getRows()}
        </tbody>
      </table>
    );
  }

  getFooter = () => (
    <div className="btn-group btn-group--right">
      <Button
        text={this.props.ui.popupButtons.cancel}
        type="action"
        btnClass="btn-action--secondary"
        onClick={this.props.closeModal}
      />

      <Button
        text={this.props.ui.popupButtons.save}
        type="action"
        isLoading={this.state.isLoading}
        onClick={this.submit}
      />
    </div>
  );

  submit = () => {
    this.setState({ isLoading: true });

    axios
      .post(this.props.ui.submitUrl, this.state.fields)
      .then((response) => {
        this.setState({ isLoading: false });
        const { success, payload, errorMessage } = response.data;

        if (success) {
          this.props.manage(this.state.fields);
          window.store.dispatch({
            type: SUCCESS,
            alert: payload.message
          });
          this.props.closeModal();
        } else {
          window.store.dispatch({
            type: FAILURE,
            alert: errorMessage
          });
        }
      })
      .catch((error) => {
        this.setState({ isLoading: false });
        window.store.dispatch({ type: FAILURE, error });
      });
  }

  render() {
    return (
      <Dialog
        closeDialog={this.props.closeModal}
        hasCloseBtn={true}
        title={this.props.ui.popupTitle}
        body={this.getBody()}
        footer={this.getFooter()}
        open={this.props.open}
      />
    );
  }
}

export default ManageOrdersModal;
