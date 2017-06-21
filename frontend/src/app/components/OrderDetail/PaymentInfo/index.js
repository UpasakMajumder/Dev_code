import React from 'react';
import SVG from '../../SVG';

export default ({ ui }) => {
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
