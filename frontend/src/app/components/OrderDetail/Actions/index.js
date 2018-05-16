import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Button from 'app.dump/Button';
/* local components */
import Modal from './Modal';

const actionPropTypes = {
  button: PropTypes.string.isRequired,
  dialog: PropTypes.object.isRequired
};

class Actions extends Component {
  state = {
    showReject: false,
    showAccept: false,
    proceeded: false
  };

  static propTypes = {
    ui: PropTypes.shape({
      accept: { ...actionPropTypes },
      reject: { ...actionPropTypes }
    }).isRequired,
    general: PropTypes.object.isRequired,
    changeStatus: PropTypes.func.isRequired
  };

  handleShowReject = () => this.setState({ showReject: true });
  handleHideReject = () => this.setState({ showReject: false });
  handleShowAccept = () => this.setState({ showAccept: true });
  handleHideAccept = () => this.setState({ showAccept: false });
  handleProceed = () => this.setState({ proceeded: true });

  render() {
    if (!this.props.ui) return null;

    const { ui: { accept, reject }, general, changeStatus } = this.props;
    const { proceeded } = this.state;

    return (
      <div className="text-right">
        {this.state.showAccept && <Modal accept handleProceed={this.handleProceed} changeStatus={changeStatus} general={general} closeDialog={this.handleHideAccept} { ...accept.dialog } />}
        {this.state.showReject && <Modal handleProceed={this.handleProceed} changeStatus={changeStatus} general={general} closeDialog={this.handleHideReject} { ...reject.dialog } />}

        <Button
          text={accept.button}
          type="success"
          btnClass="mr-2"
          disabled={proceeded}
          onClick={proceeded ? () => {} : this.handleShowAccept}
        />

        <Button
          text={reject.button}
          type="danger"
          disabled={proceeded}
          onClick={proceeded ? () => {} : this.handleShowReject}
        />
      </div>
    );
  }
}

export default Actions;
