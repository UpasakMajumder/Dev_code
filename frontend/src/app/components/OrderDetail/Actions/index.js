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
    showAccept: false
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

  render() {
    if (!this.props.ui) return null;

    const { ui: { accept, reject }, general, changeStatus } = this.props;

    return (
      <div className="text-right">
        {this.state.showAccept && <Modal accept changeStatus={changeStatus} general={general} closeDialog={this.handleHideAccept} { ...accept.dialog } />}
        {this.state.showReject && <Modal changeStatus={changeStatus} general={general} closeDialog={this.handleHideReject} { ...reject.dialog } />}

        <Button
          text={accept.button}
          type="success"
          btnClass="mr-2"
          onClick={this.handleShowAccept}
        />

        <Button
          text={reject.button}
          type="danger"
          onClick={this.handleShowReject}
        />
      </div>
    );
  }
}

export default Actions;
