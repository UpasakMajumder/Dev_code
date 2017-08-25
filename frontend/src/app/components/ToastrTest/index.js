import React from 'react';
/* 3rd part components */
import { toastr } from 'react-redux-toastr';
/* components */
import Button from 'app.dump/Button';

const ToastrTest = () => {
  return (
    <div>
      <Button text="Info" type="action" onClick={() => toastr.info('Your credit', 'Yo')}/>
      <Button text="Warning" type="info" onClick={() => toastr.warning('The title', 'The message')}/>
      <Button text="Error" type="main" onClick={() => toastr.error('The validation error', 'You have the validation mistake in the form. Please, check it again.')}/>
      <Button text="Success" type="success" onClick={() => toastr.success('Added', 'Your product has been sucessfully added')}/>
    </div>
  );
};

export default ToastrTest;
