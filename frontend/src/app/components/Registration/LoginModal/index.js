// @flow
import React from 'react';
/* components */
import Dialog from 'app.dump/Dialog';
import Button from 'app.dump/Button';

type Props = {
  body: string,
  btnLabel: string,
  btnLink: string,
  title: string
};

const LoginModal = ({
  body,
  btnLabel,
  btnLink,
  title
}: Props) => {
  const getBody = (
    <p>
      {body}
    </p>
  );

  const getFooter = (
    <div className="btn-group btn-group--center">
      <Button
        text={btnLabel}
        type="action"
        onClick={() => window.location.assign(btnLink)}
      />
    </div>
  );

  return (
    <Dialog
      title={title}
      body={getBody}
      footer={getFooter}
      open
    />
  );
};

export default LoginModal;
