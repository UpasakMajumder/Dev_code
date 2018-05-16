import React, { Component } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import { toastr } from 'react-redux-toastr';
/* constants */
import { FAILURE } from 'app.consts';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';
import Textarea from 'app.dump/Form/Textarea';

class Modal extends Component {
  static propTypes = {
    accept: PropTypes.bool,
    title: PropTypes.string.isRequired,
    text: PropTypes.string.isRequired,
    cancelButton: PropTypes.string.isRequired,
    proceedButton: PropTypes.string.isRequired,
    proceedUrl: PropTypes.string.isRequired,
    general: PropTypes.shape({
      customerName: PropTypes.string.isRequired,
      customerId: PropTypes.string.isRequired,
      orderId: PropTypes.string.isRequired
    }).isRequired,
    /* func */
    closeDialog: PropTypes.func.isRequired,
    changeStatus: PropTypes.func.isRequired,
    handleProceed: PropTypes.func.isRequired
  }

  state = {
    input: '',
    isLoading: false
  }

  handleChangeInput = event => this.setState({ input: event.target.value });

  getFooter = () => {
    return (
      <div className="btn-group btn-group--right">
        <Button
          onClick={this.props.closeDialog}
          type="action"
          btnClass="btn-action--secondary"
          disabled={this.state.isLoading}
        >
          {this.props.cancelButton}
        </Button>

        <Button
          onClick={this.submit}
          type="action"
          isLoading={this.state.isLoading}
        >
          {this.props.proceedButton}
        </Button>
      </div>
    );
  };

  submit = async () => {
    try {
      this.setState({ isLoading: true });
      const body = {
        ...this.props.general,
        rejectionNote: this.state.input
      };

      const res = await axios.post(this.props.proceedUrl, body);
      const { success, payload, errorMessage } = res.data;

      if (!success || !payload) {
        window.store.dispatch({
          type: FAILURE,
          alert: errorMessage
        });
        this.setState({ isLoading: false });
      } else {
        this.props.handleProceed();
        toastr.success(payload.title, payload.text);
        this.props.closeDialog();
        this.props.changeStatus(payload.newStatus);
      }
    } catch (e) {
      window.store.dispatch({
        type: FAILURE
      });
      this.setState({ isLoading: false });
    }
  };

  getBody = () => {
    const content = this.props.accept
      ? null
      : (
        <Textarea
          value={this.state.input}
          onChange={this.handleChangeInput}
          rows={4}
        />
      );

    return (
      <div style={{ width: 500 }}>
        <p className="text-center">{this.props.text}</p>
        {content}
      </div>
    );
  };

  render() {
    const {
      title,
      closeDialog
    } = this.props;

    return (
      <Dialog
        closeDialog={closeDialog}
        hasCloseBtn={true}
        title={title}
        body={this.getBody()}
        footer={this.getFooter()}
      />
    );
  }
}

export default Modal;
