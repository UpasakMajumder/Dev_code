import React from 'react';
/* components */
import DialogAlert from 'app.smart/DialogAlert';
/* 3rd part components */
import ReduxToastr, { toastr } from 'react-redux-toastr';

const Toastr = () => {
  window.toastr = toastr;

  return (
    <div>
      <ReduxToastr
        transitionIn="fadeIn"
        transitionOut="fadeOut"
      />
      <DialogAlert/>
    </div>
  );
};

export default Toastr;
