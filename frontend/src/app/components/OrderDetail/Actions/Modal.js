import React, { Component } from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';

class Modal extends Component {
  static propTypes = {
    title: PropTypes.string.isRequired,
    cancelButton: PropTypes.string.isRequired,
    proceedButton: PropTypes.string.isRequired,
    /* func */
    closeDialog: PropTypes.func.isRequired,
    submit: PropTypes.func.isRequired
  }

  getFooter = () => {
    return (
      <div className="btn-group btn-group--right">
        <Button
          onClick={this.props.closeDialog}
          type="action"
          btnClass="btn-action--secondary"
        >
          {this.props.cancelButton}
        </Button>

        <Button
          onClick={() => {
            this.props.closeDialog();
            this.props.submit();
          }}
          type="action"
        >
          {this.props.proceedButton}
        </Button>
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
        footer={this.getFooter()}
      />
    );
  }
}

export default Modal;
