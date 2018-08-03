// @flow
import React from 'react';
import PropTypes from 'prop-types';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';

import { HAS_DIALOG } from 'app.consts';

/* components */
import SVG from 'app.dump/SVG';
import BodyClassToggler from 'app.dump/BodyClassToggler';
/* flow-types */

type Props = {
  closeDialog: ?() => void,
  hasCloseBtn: ?boolean,
  title: string,
  body: ?{},
  footer: ?{},
  open: boolean
};

const Dialog = (props: Props) => {
  const { closeDialog, hasCloseBtn, title, body, footer, open } = props;
  const closer = hasCloseBtn
    ? <button onClick={closeDialog} type="button" className="btn--off dialog__closer">
      <SVG name="cross--darker" className="icon-modal" />
    </button>
    : null;

  const bodyElement = body
    ? <div className="dialog__content">{body}</div>
    : null;

  const footerElement = footer
    ? <div className="dialog__footer">{footer}</div>
    : null;

  const content = open
    ? (
      <div className="dialog active">
        <div onClick={closeDialog} className="dialog__shadow"/>
        <div className="dialog__block">
          <div className="dialog__header">
            <p>{title}</p>
            {closer}
          </div>
          {bodyElement}
          {footerElement}
        </div>
      </div>
    ) : null;

  return (
    <BodyClassToggler
      className={HAS_DIALOG}
      isActive={open}
    >
      <ReactCSSTransitionGroup
        transitionName="dialog"
        transitionEnterTimeout={400}
        transitionLeaveTimeout={400}
        component="div"
      >
        {content}
      </ReactCSSTransitionGroup>
    </BodyClassToggler>
  );
};

Dialog.propTypes = {
  closeDialog: PropTypes.func,
  title: PropTypes.string.isRequired,
  hasCloseBtn: PropTypes.bool,
  footer: PropTypes.object,
  body: PropTypes.object,
  open: PropTypes.bool.isRequired
};

export default Dialog;
