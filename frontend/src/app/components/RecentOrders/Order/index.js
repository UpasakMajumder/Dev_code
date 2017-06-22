import React from 'react';
import PropTypes from 'prop-types';
import Link from '../../Link';

function TableRow(props) {
  const { orderNumber, orderDate, items, orderStatus, deliveryDate, viewBtn } = props;

  const itemsTd = () => {
    const listOfItems = items.map(item => <span className="show-table__list-text">{item}</span>);

    return (
      <td className="show-table__text-appear">
        <span className="badge badge--s badge--empty badge--bold">{items.length}</span>
        {listOfItems}
      </td>
    );
  };

  return (
    <tr>
      <td>{orderNumber}</td>
      <td>{orderDate}</td>
      {itemsTd()}
      <td className="show-table__will-hide">{orderStatus}</td>
      <td className="show-table__will-hide">{deliveryDate}</td>
      <td className="show-table__will-appear">
        <Link text={viewBtn.text} href={viewBtn.url} type="action" />
      </td>
    </tr>
  );
}

TableRow.PropTypes = {
  orderNumber: PropTypes.number.isRequired,
  orderDate: PropTypes.string.isRequired,
  items: PropTypes.arrayOf(PropTypes.string),
  orderStatus: PropTypes.string.isRequired,
  deliveryDate: PropTypes.string.isRequired,
  viewBtn: PropTypes.objectOf(PropTypes.string)
};

export default TableRow;
