import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';

class DialogAlert extends Component {
  static propTypes = {
    message: PropTypes.string.isRequired,
    closeDialog: PropTypes.func,
    btns: PropTypes.arrayOf(PropTypes.shape({
      label: PropTypes.string.isRequired,
      func: PropTypes.func
    }).isRequired).isRequired,
    visible: PropTypes.bool.isRequired
  };

  render() {
    const { visible } = this.props;

    if (!visible) return null;

    const btnsList = this.props.btns.map((btn, i) => {
      return (
        <Button
          key={i}
          text={btn.label}
          onClick={btn.func}
          type="action"
        />
      );
    });

    const footer = <div className="dialog-alert__btns">{btnsList}</div>;

    return (
      <Dialog
        closeDialog={this.props.closeDialog}
        title={this.props.message}
        footer={this.props.btns.length ? footer : null}
        open={visible}
      />
    );
  }
}

export default connect((state) => {
  const { dialogAlert } = state;
  return { ...dialogAlert };
}, {})(DialogAlert);
