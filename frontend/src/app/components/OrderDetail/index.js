import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Spinner from 'app.dump/Spinner';
/* ac */
import getUI from 'app.ac/orderDetail';
/* utilities */
import { getSearchObj } from 'app.helpers/location';
/* local components */
import CommonInfo from './CommonInfo';
import ShippingInfo from './ShippingInfo';
import PaymentInfo from './PaymentInfo';
import PricingInfo from './PricingInfo';
import OrderedItems from './OrderedItems';

class OrderDetail extends Component {
  static propTypes = {
    getUI: PropTypes.func.isRequired,
    ui: PropTypes.shape({
      dateTimeNAString: PropTypes.string,
      commonInfo: PropTypes.object,
      orderedItems: PropTypes.object,
      paymentInfo: PropTypes.object,
      pricingInfo: PropTypes.object,
      shippingInfo: PropTypes.object
    }).isRequired
  };

  componentDidMount() {
    const { getUI } = this.props;
    const { orderID } = getSearchObj();

    getUI(orderID);
  }

  render() {
    const { ui } = this.props;
    if (!Object.keys(ui).length) return <Spinner />;

    const { commonInfo, shippingInfo, paymentInfo, pricingInfo, orderedItems, dateTimeNAString } = ui;

    const shippingInfoEl = shippingInfo ? <div className="col-lg-4 mb-4"><ShippingInfo ui={shippingInfo} /></div> : null;
    const paymentInfoEl = paymentInfo ? <div className="col-lg-4 mb-4"><PaymentInfo ui={paymentInfo} dateTimeNAString={dateTimeNAString} /></div> : null;
    const pricingInfoEl = pricingInfo ? <div className="col-lg-4 mb-4"><PricingInfo ui={pricingInfo} /></div> : null;

    return (
      <div>
        <CommonInfo
          ui={commonInfo}
          dateTimeNAString={dateTimeNAString}
        />

        <div className="order-block">
          <div className="row">
            {shippingInfoEl}
            {paymentInfoEl}
            {pricingInfoEl}
          </div>
        </div>

        <OrderedItems ui={orderedItems}/>
      </div>
    );
  }
}

export default connect(({ orderDetail }) => {
  const { ui } = orderDetail;
  return { ui };
}, {
  getUI
})(OrderDetail);
