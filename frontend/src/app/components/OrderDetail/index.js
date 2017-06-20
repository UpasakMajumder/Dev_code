import React, { Component } from 'react';
import { connect } from 'react-redux';
import CommonInfo from './CommonInfo';
import ShippingInfo from './ShippingInfo';
import PaymentInfo from './PaymentInfo';
import PricingInfo from './PricingInfo';
import OrderedItems from './OrderedItems';
import getUI from '../../AC/orderDetail';
import Spinner from '../Spinner';

class OrderDetail extends Component {
  componentDidMount() {
    this.props.getUI();
  }

  render() {
    const { orderDetail } = this.props;
    const { ui } = orderDetail;
    const { commonInfo, shippingInfo, paymentInfo, pricingInfo, orderedItems } = ui;

    const content = <div>
      <CommonInfo ui={commonInfo} />

      <div className="order-block">
        <div className="row">
          <div className="col-lg-4 mb-4">
            <ShippingInfo ui={shippingInfo} />
          </div>

          <div className="col-lg-4 mb-4">
            <PaymentInfo ui={paymentInfo} />
          </div>

          <div className="col-lg-4 mb-4">
            <PricingInfo ui={pricingInfo} />
          </div>
        </div>
      </div>

      <OrderedItems ui={orderedItems}/>
    </div>;

    return Object.keys(ui).length
      ? content
      : <Spinner />;
  }
}

export default connect((state) => {
  const { orderDetail } = state;
  return { orderDetail };
}, {
  getUI
})(OrderDetail);
