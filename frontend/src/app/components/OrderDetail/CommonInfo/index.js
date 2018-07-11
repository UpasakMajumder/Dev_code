import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import timeFormat from 'app.helpers/time';

const CommonInfo = ({
  ui,
  dateTimeNAString,
  orderHistoryLabel
}) => {
  const { status, orderDate, shippingDate, totalCost } = ui;

  const tiles = [
    {
      title: status.title,
      value: status.value,
      icon: 'flag',
      note: status.note
    },
    {
      title: orderDate.title,
      value: timeFormat(orderDate.value, dateTimeNAString),
      icon: 'calendar'
    },
    {
      title: shippingDate.title,
      value: timeFormat(shippingDate.value, dateTimeNAString),
      icon: 'truck'
    },
    {
      title: totalCost.title,
      value: totalCost.value,
      icon: 'dollar'
    }
  ];

  const getViewHistoryButton = (icon) => {
    if (icon !== 'flag') return null;
    if (!orderHistoryLabel) return null;

    return <button type="button" className="btn--off mt-2 link">{orderHistoryLabel}</button>;
  };

  const tileList = tiles.map((tile) => {
    const { value, icon, title, note } = tile;

    const noteElement = note ? <p className={`tile-bar__note ${icon === 'flag' ? 'tile-bar__note--red' : ''}`}>{note}</p> : null;

    return (
      <div key={icon} className="tile-bar__item">
        <SVG name={icon} className="icon-tile"/>
        <div>
          <p className="tile-bar__title">{title}</p>
          <p className="tile-bar__description">{value}</p>
          {getViewHistoryButton(tile.icon)}
        </div>
      </div>
    );
  });

  return (
    <div className="order-block">
      <div className="tile-bar">
        {tileList}
      </div>
    </div>
  );
};

CommonInfo.propTypes = {
  dateTimeNAString: PropTypes.string.isRequired,
  ui: PropTypes.shape({
    status: PropTypes.shape({
      title: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired,
      note: PropTypes.string
    }).isRequired,
    orderDate: PropTypes.shape({
      title: PropTypes.string.isRequired,
      value: PropTypes.string
    }).isRequired,
    shippingDate: PropTypes.shape({
      title: PropTypes.string.isRequired,
      value: PropTypes.string
    }).isRequired,
    totalCost: PropTypes.shape({
      title: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    }).isRequired
  }),
  orderHistoryLabel: PropTypes.string
};

export default CommonInfo;
