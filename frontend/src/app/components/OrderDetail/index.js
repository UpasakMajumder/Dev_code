import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
/* components */
import Spinner from 'app.dump/Spinner';
/* ac */
import { getUI, changeStatus, editOrders } from 'app.ac/orderDetail';
import toogleEmailProof from 'app.ac/emailProof';
/* utilities */
import { getSearchObj } from 'app.helpers/location';
/* globals */
import { ORDER_HISTORY } from 'app.globals';
/* local components */
import CommonInfo from './CommonInfo';
import ShippingInfo from './ShippingInfo';
import PaymentInfo from './PaymentInfo';
import PricingInfo from './PricingInfo';
import OrderedItems from './OrderedItems';
import Actions from './Actions';
import EditModal from './EditModal';
import OrderHistory from './OrderHistory';
import EmailProof from '../EmailProof';

class OrderDetail extends Component {
  static propTypes = {
    getUI: PropTypes.func.isRequired,
    ui: PropTypes.shape({
      dateTimeNAString: PropTypes.string,
      commonInfo: PropTypes.object,
      orderedItems: PropTypes.object,
      paymentInfo: PropTypes.object,
      pricingInfo: PropTypes.object,
      shippingInfo: PropTypes.object,
      actions: PropTypes.object,
      general: PropTypes.object,
      editOrders: PropTypes.shape({
        proceedUrl: PropTypes.string.isRequired,
        dialog: PropTypes.object.isRequired
      })
    }).isRequired,
    emailProof: PropTypes.object.isRequired,
    toogleEmailProof: PropTypes.func.isRequired
  };

  state = {
    showEditModal: false, // managed in Actions component
    orderHistory: ORDER_HISTORY,
    showOrderHistory: false
  }

  componentDidMount() {
    const { getUI } = this.props;
    const { orderID } = getSearchObj();

    getUI(orderID);
  }

  getMaxOrderQuantity = () => {
    if (this.maxOrderQuantity) return this.maxOrderQuantity;

    const maxOrderQuantity = {};
    this.props.ui.orderedItems.items.forEach((orderedItem) => {
      maxOrderQuantity[orderedItem.id] = orderedItem.quantity;
    });

    this.maxOrderQuantity = maxOrderQuantity;

    return maxOrderQuantity;
  };

  changeStatus = newStatus => this.props.changeStatus(newStatus);

  showEditModal = showEditModal => this.setState({ showEditModal });

  showOrderHistoryModal = () => this.setState({ showOrderHistory: !this.state.showOrderHistory });

  render() {
    const { ui, emailProof, toogleEmailProof, changeStatus } = this.props;
    if (!Object.keys(ui).length) return <Spinner />;

    const {
      commonInfo,
      shippingInfo,
      paymentInfo,
      pricingInfo,
      orderedItems,
      dateTimeNAString,
      actions,
      general,
      editOrders
    } = ui;

    const shippingInfoEl = shippingInfo ? <div className="col-lg-4 mb-4"><ShippingInfo ui={shippingInfo} /></div> : null;
    const paymentInfoEl = paymentInfo ? <div className="col-lg-4 mb-4"><PaymentInfo ui={paymentInfo} dateTimeNAString={dateTimeNAString} /></div> : null;
    const pricingInfoEl = pricingInfo ? <div className="col-lg-4 mb-4"><PricingInfo ui={pricingInfo} /></div> : null;

    const editModal = editOrders
      ? (
        <EditModal
          closeModal={() => this.showEditModal(false)}
          open={this.state.showEditModal}
          orderedItems={orderedItems.items}
          {...editOrders.dialog}
          proceedUrl={this.props.ui.editOrders.proceedUrl}
          paidByCreditCard={paymentInfo.paymentIcon === 'credit-card'}
          editOrders={this.props.editOrders}
          general={general}
          maxOrderQuantity={this.getMaxOrderQuantity()}
        />
      ) : null;

    const nonZeroProductsExist = Boolean(orderedItems.items.filter(item => item.quantity > 0).length);

    return (
      <div>
        <EmailProof open={emailProof.show} />
        <OrderHistory
          orderHistory={this.state.orderHistory}
          open={this.state.showOrderHistory}
          closeDialog={this.showOrderHistoryModal}
        />
        {editModal}
        <CommonInfo
          ui={commonInfo}
          dateTimeNAString={dateTimeNAString}
          orderHistoryLabel={this.state.orderHistory ? this.state.orderHistory.label : null}
          showOrderHistoryModal={this.showOrderHistoryModal}
        />

        <div className="order-block">
          <div className="row">
            {shippingInfoEl}
            {paymentInfoEl}
            {pricingInfoEl}
          </div>
        </div>

        {nonZeroProductsExist && (
          <OrderedItems
            toogleEmailProof={toogleEmailProof}
            ui={orderedItems}
            showRejectionLabel={false}
          />
        )}

        <div className="order-block">
          <Actions
            acceptEnabled={nonZeroProductsExist}
            actions={actions}
            editOrders={editOrders}
            editEnabled={nonZeroProductsExist}
            general={general}
            changeStatus={changeStatus}
            showEditModal={this.showEditModal}
          />
        </div>
      </div>
    );
  }
}

export default connect(({ orderDetail, emailProof }) => {
  const { ui } = orderDetail;
  return { ui, emailProof };
}, {
  getUI,
  toogleEmailProof,
  changeStatus,
  editOrders
})(OrderDetail);
