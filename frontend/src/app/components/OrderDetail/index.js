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
/* local components */
import CommonInfo from './CommonInfo';
import ShippingInfo from './ShippingInfo';
import PaymentInfo from './PaymentInfo';
import PricingInfo from './PricingInfo';
import OrderedItems from './OrderedItems';
import Actions from './Actions';
import EditModal from './EditModal';
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
    showEditModal: false // managed in Actions component
  };

  componentDidMount() {
    const { getUI } = this.props;
    const { orderID } = getSearchObj();

    getUI(orderID);
  }

  changeStatus = newStatus => this.props.changeStatus(newStatus);

  showEditModal = showEditModal => this.setState({ showEditModal });

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

    return (
      <div>
        <EmailProof open={emailProof.show} />
        <EditModal
          closeModal={() => this.showEditModal(false)}
          open={this.state.showEditModal}
          orderedItems={orderedItems.items}
          {...editOrders.dialog}
          proceedUrl={this.props.ui.editOrders.proceedUrl}
          paidByCreditCard={paymentInfo.paymentIcon === 'credit-card'}
          editOrders={this.props.editOrders}
        />
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

        <OrderedItems toogleEmailProof={toogleEmailProof} ui={orderedItems}/>

        <div className="order-block">
          <Actions
            actions={actions}
            editOrders={editOrders}
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
