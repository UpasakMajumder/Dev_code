import React from 'react';
import PropTypes from 'prop-types';
import SVG from '../../SVG';

const PaymentInfo = ({ ui }) => {
  const { title, paymentIcon, paidBy, paymentDetail, date } = ui;

  return (
    <div className="order-block order-block--tile ">
      <h2 className="order-block__header">{title}</h2>
      <div className="order-block__detail">
        <SVG name={paymentIcon}/>
        <p>{paidBy},<br /> {paymentDetail}</p>
        <p>Payment date: <span className="weight--bold">{date}</span></p>
      </div>
    </div>
  );
};

PaymentInfo.propTypes = {
  ui: PropTypes.shape({
    title: PropTypes,
    paymentIcon: PropTypes,
    paidBy: PropTypes,
    paymentDetail: PropTypes,
    date: PropTypes
  })
};

export default PaymentInfo;
