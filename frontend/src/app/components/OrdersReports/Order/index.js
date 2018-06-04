import React from 'react';
import PropTypes from 'prop-types';
/* helpers */
import timeFormat from 'app.helpers/time';

const redirectUser = url => location.assign(url);

const Order = ({
  url,
  items,
  headings
}) => {
  const orders = items.map((item, index) => {
    let value = item;
    if (headings[index].isDate) value = timeFormat(value);
    if (typeof value === 'object') {
      if (value.type === 'tracking') {
        const content = value.items.map((item, i) => {
          const prefix = i === 0 ? ' ' : ', ';
          if (item.url) {
            return <span key={i}>{prefix}<a target="_blank" href={item.url} className="link">{item.id}</a></span>;
          }

          return <span key={i}>{prefix}{item.id}</span>;
        });

        return <td key={index}>{content}</td>;
      }
    }

    return <td key={index}>{value}</td>;
  });

  return (
    <tr
      onClick={() => redirectUser(url)}
    >
      {orders}
    </tr>
  );
};

Order.PropTypes = {
  url: PropTypes.string.isRequired,
  items: PropTypes.arrayOf(PropTypes.string.isRequired).isRequired,
  headings: PropTypes.arrayOf(PropTypes.shape({
    isDate: PropTypes.bool
  }).isRequired).isRequired
};

export default Order;
