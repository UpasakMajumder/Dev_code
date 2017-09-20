import React from 'react';
import PropTypes from 'prop-types';
/* components */
import SVG from 'app.dump/SVG';
/* helpers */
import { divideBySlash } from 'app.helpers/time';

const PaymentInfo = ({ ui }) => {
  const { title, paymentIcon, paidBy, paymentDetail, date } = ui;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <SVG name={paymentIcon}/>
        <p>{paidBy},<br /> {paymentDetail}</p>
        <p>Payment date: <span className="weight--bold">{divideBySlash(date)}</span></p>
      </div>
    </div>
  );
};

PaymentInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes.string.isRequired,
    paymentIcon: PropTypes.string.isRequired,
    paidBy: PropTypes.string.isRequired,
    paymentDetail: PropTypes.string.isRequired,
    date: PropTypes.string
  })
};

export default PaymentInfo;
