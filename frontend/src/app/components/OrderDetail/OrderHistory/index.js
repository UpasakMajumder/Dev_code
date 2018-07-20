import React from 'react';
import PropTypes from 'prop-types';
import uuid from 'uuid';
/* components */
import Dialog from 'app.dump/Dialog';
/* helpers */
import timeFormat from 'app.helpers/time';

const OrderHistory = ({ orderHistory, closeDialog, open }) => {
  if (!orderHistory) return null;

  const getChangeElement = (changes) => {
    if (!changes.items.length) return null;

    const headers = changes.headers.map(header => <th key={uuid()}>{header}</th>);
    const rows = changes.items.map((row) => {
      const items = row.map((item) => {
        switch (item.type) {
        case 'link':
          return <td key={uuid()}><a target="_blank" href={item.url}>{item.text}</a></td>;
        case 'date':
          return <td key={uuid()}>{timeFormat(item.text, '', true)}</td>;
        default:
          return <td key={uuid()}>{item.text}</td>;
        }
      });

      return <tr key={uuid()}>{items}</tr>;
    });

    return (
      <div className="mb-5">
        <p>{changes.title}</p>
        <table className="table">
          <tbody>
            <tr>{headers}</tr>
            {rows}
          </tbody>
        </table>
      </div>
    );
  };

  const getBody = () => {
    const messageElement = orderHistory.message.text
      ? (
        <div>
          <p>{orderHistory.message.title}</p>
          <p>{orderHistory.message.text}</p>
        </div>
      ) : null;

    return (
      <div>
        {getChangeElement(orderHistory.itemChanges)}
        {getChangeElement(orderHistory.orderChanges)}
        {messageElement}
      </div>
    );
  };

  return (
    <Dialog
      closeDialog={closeDialog}
      hasCloseBtn={true}
      title={orderHistory.title}
      body={getBody()}
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
    }).isRequired,
    itemChanges: PropTypes.shape({
      title: PropTypes.string.isRequired,
      headers: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
      items: PropTypes.arrayOf(PropTypes.arrayOf(PropTypes.shape({
        type: PropTypes.string.isRequired,
        text: PropTypes.string.isRequired,
        url: PropTypes.string
      }).isRequired))
    }).isRequired
  })
};

export default OrderHistory;
