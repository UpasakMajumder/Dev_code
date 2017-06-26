import React from 'react';
import SVG from '../SVG';

const Dialog = (props) => {
  const { closeDialog, hasCloseBtn, title, body, footer } = props;
  const closer = hasCloseBtn
    ? <button onClick={closeDialog} type="button" className="btn--off dialog__closer">
      <SVG name="cross--darker" className="icon-modal" />
    </button>
    : null;

  return (
    <div className="dialog active">
      <div onClick={closeDialog} className="dialog__shadow"> </div>
      <div className="dialog__block">
        <div className="dialog__header">
          <p>{title}</p>
          {closer}
        </div>
        <div className="dialog__content">{body}</div>
        <div className="dialog__footer">{footer}</div>
      </div>
    </div>
  );
};

export default Dialog;
