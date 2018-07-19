// @flow
import * as React from 'react';
import PropTypes from 'prop-types';
import uuid from 'uuid';
/* components */
import SVG from 'app.dump/SVG';
import Product from 'app.dump/Product/Order';
/* helpers */
import timeFormat from 'app.helpers/time';
import Order from '../../_dump/Product/Order';

type Tracking = {
  prefix: string,
  url?: string,
  id: string
}

type ShippingDate = {
  prefix: string,
  date: string
}

type Items = {
  tracking?: Tracking,
  shippingDate?: ShippingDate,
  orders: any
}

type Props = {
  title: string,
  items: Array<Items>,
  toggleEmailProof: () => {}
}

const GroupContainer = ({ title, items, toggleEmailProof }: Props) => {
  if (!items.length) return null;
  const groups = items.map((item) => {
    const { tracking, shippingDate, orders } = item;

    const tracker = tracking
      ? (
        <div className="cart-product__tracking mb-3">
          <SVG name="location"/>
          {tracking.prefix}: { tracking.url
            ? <a href={tracking.url} className="link" target="_blank">{tracking.id}</a>
            : tracking.id
          }
        </div>
      ) : null;

    const shipping = shippingDate
      ? (
        <div className="cart-product__tracking mb-3">
          <SVG name="courier"/>
          {shippingDate.prefix}: <strong>{timeFormat(shippingDate.date)}</strong>
        </div>
      ) : null

    const orderItems = orders.map((order) => {
      return (
        <Order
          key={uuid()}
          {...order}
          toggleEmailProof={toggleEmailProof}
          groupped
        />
      );
    });

    return (
      <div key={uuid()}>
        <div>{tracker}</div>
        <div>{shipping}</div>
        <div>{orderItems}</div>
      </div>
    );
  });

  return (
    <div className="mb-5">
      <h2 className="mb-3">{title}</h2>
      <div className="orders-group">
        {groups}
      </div>
    </div>
  );
};

GroupContainer.propTypes = {
  title: PropTypes.string.isRequired,
  items: PropTypes.arrayOf(PropTypes.shape({
    tracking: PropTypes.shape({
      prefix: PropTypes.string.isRequired,
      id: PropTypes.string.isRequired,
      url: PropTypes.string
    }),
    shippingDate: PropTypes.shape({
      prefix: PropTypes.string.isRequired,
      date: PropTypes.string.isRequired
    }),
    orders: PropTypes.arrayOf(PropTypes.object.isRequired).isRequired
  })).isRequired,
  toggleEmailProof: PropTypes.func.isRequired
};

export default GroupContainer;
