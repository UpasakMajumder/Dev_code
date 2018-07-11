import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';

const OrderHistory = ({ orderHistory, closeDialog, open }) => {
  if (!orderHistory) return null;

  return (
    <Dialog
      closeDialog={closeDialog}
      hasCloseBtn={true}
      title={orderHistory.title}
      body={null}
      footer={null}
      open={open}
    />
  );
};

OrderHistory.propTypes = {
  closeDialog: PropTypes.func.isRequired,
  open: PropTypes.bool.isRequired,
  orderHistory: PropTypes.shape({
    title: PropTypes.string.isRequired,
    message: PropTypes.shape({
      title: PropTypes.string.isRequired,
      text: PropTypes.string
    }).isRequired
  })
};

export default OrderHistory;
