import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import timeFormat from 'app.helpers/time';

const PaymentInfo = ({ ui, dateTimeNAString }) => {
  const { title, paymentIcon, paidBy, paymentDetail, date, datePrefix, bUnitName, bUnitLabel } = ui;

  const paymentMethodInfo = paymentDetail ? <p>{paidBy},<br /> {paymentDetail}</p> : <p>{paidBy}</p>;

  const bUnitInfo = bUnitName ? <p>{bUnitLabel}: {bUnitName}</p> : <p></p>;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <SVG name={paymentIcon}/>
        {paymentMethodInfo}
        <p>{datePrefix}: <span className="weight--bold">{timeFormat(date, dateTimeNAString)}</span></p>
        {bUnitInfo}
      </div>
    </div>
  );
};

PaymentInfo.propTypes = {
  dateTimeNAString: PropTypes.string.isRequired,
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    paymentIcon: PropTypes.string.isRequired,
    paidBy: PropTypes.string.isRequired,
    paymentDetail: PropTypes.string.isRequired,
    date: PropTypes.string.isRequired,
    datePrefix: PropTypes.string.isRequired,
    bUnitName: PropTypes.string.isRequired,
    bUnitLabel: PropTypes.string.isRequired
  })
};

export default PaymentInfo;
