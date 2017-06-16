import React, { Component } from 'react';
import { connect } from 'react-redux';
import SVG from '../SVG';
import { closeDialog } from '../../AC/dialog';

class Dialog extends Component {
  render() {
    const { dialog } = this.props;
    const { isOpen, headerTitle, isCloseButton, body, footer } = dialog;

    const closeButton = isCloseButton
    ? <button onClick={this.props.closeDialog} type="button" className="btn--off dialog__closer">
        <SVG name="cross--darker" className="icon-modal" />
      </button>
    : null;

    const bodyElement = body
      ? <div className="dialog__content">{body}</div>
      : null;

    const footerElement = footer
      ? <div className="dialog__footer">{footer}</div>
      : null;

    const content = isOpen
    ? <div className="dialog active">
        <div onClick={this.props.closeDialog} className="dialog__shadow"></div>
        <div className="dialog__block">
          <div className="dialog__header">
            <p>{headerTitle}</p>
            {closeButton}
          </div>
          {bodyElement}
          {footerElement}
        </div>
      </div>
    : null;

    return content;
  }
}

export default connect((state) => {
  const { dialog } = state;
  return { dialog };
}, {
  closeDialog
})(Dialog);
