import React from 'react';
import PropTypes from 'prop-types';
import uuid from 'uuid';
/* helpers */
import timeFormat from 'app.helpers/time';

const redirectUser = (e, url) => {
  if (e.target.tagName !== 'A') location.assign(url);
};

const Order = ({
  url,
  items,
  headings
}) => {
  const orders = [];
  for (let index = 0; index < Object.keys(items).length; index += 1) {
    const header = headings[index];

    if (header) {
      const item = items[header.id];

      if (header.type === 'tracking') {
        let content = <span>{item.value}</span>;

        if (Array.isArray(item.value)) {
          content = item.value.map((value, index) => {
            const prefix = index === item.value.length - 1 ? '' : ', ';
            return <span key={uuid()}>{value}{prefix}</span>;
          });
        }

        orders.push(<td key={uuid()}>{content}</td>);
      } else if (header.type === 'date') {
        if (Array.isArray(item.value)) {
          const content = item.value.map((value, index) => {
            if (index === item.value.length - 1) {
              return <span key={uuid()}>{timeFormat(value)}</span>;
            }

            return <p key={uuid()}>{timeFormat(value)}</p>;
          });

          orders.push(<td key={uuid()}>{content}</td>);
        } else {
          orders.push(<td key={uuid()}>{timeFormat(item.value)}</td>);
        }
      } else if (Array.isArray(item.value)) {
        const content = item.value.map((value, index) => {
          const prefix = index === item.value.length - 1 ? '' : ', ';
          return <span key={uuid()}>{value}{prefix}</span>;
        });

        orders.push(<td key={uuid()}>{content}</td>);
      } else {
        orders.push(<td key={uuid()}>{item.value}</td>);
      }
    }
  }

  return (
    <tr onClick={e => redirectUser(e, url)}>
      {orders}
    </tr>
  );
};

Order.PropTypes = {
  url: PropTypes.string.isRequired,
  items: PropTypes.object.isRequired,
  headings: PropTypes.array.isRequired
};

export default Order;
