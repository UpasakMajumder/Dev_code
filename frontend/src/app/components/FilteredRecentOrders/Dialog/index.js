import React from 'react';
import PropTypes from 'prop-types';
/* components */
import Dialog from 'app.dump/Dialog';

const getBodyTable = (table) => {
  // header
  const headers = table.headers.map((header, i) => <th key={i}>{header}</th>);
  const tableHeader = <tr>{headers}</tr>;

  // rows
  const tableRows = table.rows.map((row, i) => {
    const tableRow = row.map((cell, j) => <td key={j} colSpan={cell.span || 1}>{cell.value}</td>);

    return <tr key={i}>{tableRow}</tr>;
  });

  return (
    <table className="show-table">
      <tbody>
        {tableHeader}
        {tableRows}
      </tbody>
    </table>
  );
};

const getBody = (distributor, pdf, table) => {
  return (
    <div>
      <div className="mb-3 filtered-recent-orders__dialog-subtitle">
        <div>
          {`${distributor.label}: ${distributor.value}`}
        </div>
        <div>
          <a href={pdf.value} target="_blank" className="btn-action">{pdf.label}</a>
        </div>
      </div>

      {getBodyTable(table)}
    </div>
  );
};

const OrderDialog = ({
  dialog,
  closeDialog
}) => {
  const { orderId, distributor, pdf, table } = dialog;
  const title = `${orderId.label}: ${orderId.value}`;

  return (
    <Dialog
      closeDialog={closeDialog}
      hasCloseBtn
      footer={null}
      body={getBody(distributor, pdf, table)}
      title={title}
    />
  );
};

OrderDialog.propTypes = {
  closeDialog: PropTypes.func.isRequired,
  dialog: PropTypes.shape({
    orderId: PropTypes.shape({
      label: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    }).isRequired,
    distributor: PropTypes.shape({
      label: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    }).isRequired,
    pdf: PropTypes.shape({
      label: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    }).isRequired,
    table: PropTypes.shape({
      headers: PropTypes.arrayOf(PropTypes.string.isRequired),
      rows: PropTypes.arrayOf(PropTypes.arrayOf(PropTypes.shape({
        value: PropTypes.string.isRequired,
        span: PropTypes.number
      })))
    }).isRequired
  }).isRequired
};

export default OrderDialog;
