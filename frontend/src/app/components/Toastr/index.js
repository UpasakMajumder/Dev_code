import React from 'react';
/* 3rd part components */
import ReduxToastr, { toastr } from 'react-redux-toastr';

const Toastr = () => {
  window.toastr = toastr;

  return (
    <ReduxToastr
      transitionIn="fadeIn"
      transitionOut="fadeOut"
    />
  );
};

export default Toastr;
