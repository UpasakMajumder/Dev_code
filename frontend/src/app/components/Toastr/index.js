import React from 'react';
/* components */
import DialogAlert from 'app.smart/DialogAlert';
/* 3rd part components */
import ReduxToastr from 'react-redux-toastr';

const Toastr = () => {

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
